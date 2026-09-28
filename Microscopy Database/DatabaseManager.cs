using System.Data.Common;
using System.IO;
using MicroscopyApp.Models;
using Microsoft.Data.Sqlite;

namespace Microscopy_Database;

public class DatabaseManager
{
    private static DatabaseManager? _instance;
    private static readonly object _lock = new();
    private readonly string _connectionString;

    public static string AppDataFolder => AppDomain.CurrentDomain.BaseDirectory;

    // Root directory where all entry folders live
    public static string EntriesFolder => Path.Combine(AppDataFolder, "Entries");

    public static DatabaseManager Instance
    {
        get
        {
            lock(_lock)
            {
                return _instance ??= new DatabaseManager();
            }
        }
    }

    private DatabaseManager()
    {
        Directory.CreateDirectory(EntriesFolder);

        string dbPath = Path.Combine(AppDataFolder, "microscopy.db");
        _connectionString = $"Data Source={dbPath}";

        InitializeDatabase();
    }

    private SqliteConnection GetConnection()
    {
        var connection = new SqliteConnection(_connectionString);
        connection.Open();
        using var command = connection.CreateCommand();
        command.CommandText = "PRAGMA foreign_keys = ON;";
        command.ExecuteNonQuery();
        return connection;
    }

    public void InitializeDatabase()
    {
        using var connection = GetConnection();
        connection.Open();
        using var command = connection.CreateCommand();

        command.CommandText = @"
        CREATE TABLE IF NOT EXISTS MicroscopyEntries (
            Id TEXT PRIMARY KEY,
            ObservationId TEXT,
            Name TEXT NOT NULL,
            EntryDate TEXT NOT NULL,
            EntryTime TEXT NOT NULL,
            Notes TEXT,
            SampleSource TEXT,
            CollectionLocation TEXT,
            SampleType TEXT,
            OrganismGroup INTEGER,  -- Changed to INTEGER for Enum mapping            Identification TEXT,
            IdentificationConfidence INTEGER
        );

        CREATE TABLE IF NOT EXISTS EntryImages (
            ImageId INTEGER PRIMARY KEY AUTOINCREMENT,
            EntryId TEXT NOT NULL,
            ImagePath TEXT NOT NULL,
            FOREIGN KEY (EntryId) REFERENCES MicroscopyEntries(Id) ON DELETE CASCADE
        );
    ";
        command.ExecuteNonQuery();

        // Automatically add columns if upgrading an existing database file
        EnsureColumnExists(connection, "MicroscopyEntries", "ObservationId", "TEXT");
        EnsureColumnExists(connection, "MicroscopyEntries", "SampleSource", "TEXT");
        EnsureColumnExists(connection, "MicroscopyEntries", "CollectionLocation", "TEXT");
        EnsureColumnExists(connection, "MicroscopyEntries", "SampleType", "TEXT");
        EnsureColumnExists(connection, "MicroscopyEntries", "OrganismGroup", "TEXT");
        EnsureColumnExists(connection, "MicroscopyEntries", "Identification", "TEXT");
        EnsureColumnExists(connection, "MicroscopyEntries", "IdentificationConfidence", "INTEGER");
    }

    private void EnsureColumnExists(DbConnection connection, string tableName, string columnName, string columnType)
    {
        using var command = connection.CreateCommand();
        command.CommandText = $"PRAGMA table_info({tableName});";
        var exists = false;
        using (var reader = command.ExecuteReader())
        {
            while (reader.Read())
            {
                if (reader["name"].ToString()!.Equals(columnName, StringComparison.OrdinalIgnoreCase))
                {
                    exists = true;
                    break;
                }
            }
        }

        if (!exists)
        {
            using var altCommand = connection.CreateCommand();
            altCommand.CommandText = $"ALTER TABLE {tableName} ADD COLUMN {columnName} {columnType};";
            altCommand.ExecuteNonQuery();
        }
    }

    public int GetTotalEntryCount()
    {
        using var connection = GetConnection();
        connection.Open();
        using var command = connection.CreateCommand();
        command.CommandText = "SELECT COUNT(*) FROM MicroscopyEntries;";
        object? result = command.ExecuteScalar();
        return result != null && result != DBNull.Value ? Convert.ToInt32(result) : 0;
    }

   public void AddEntry(MicroscopyEntry entry)
{
    using var connection = GetConnection();
    connection.Open();
    using var transaction = connection.BeginTransaction();

    using (var command = connection.CreateCommand())
    {
        command.Transaction = transaction;
        command.CommandText = @"
            INSERT INTO MicroscopyEntries (Id, ObservationId, Name, EntryDate, EntryTime, Notes, SampleSource, CollectionLocation, SampleType, OrganismGroup, Identification, IdentificationConfidence)
            VALUES ($id, $obsId, $name, $date, $time, $notes, $source, $location, $type, $group, $ident, $conf);
        ";
        command.Parameters.AddWithValue("$id", entry.Id.ToString());
        command.Parameters.AddWithValue("$obsId", entry.ObservationId ?? string.Empty);
        command.Parameters.AddWithValue("$name", entry.Name);
        command.Parameters.AddWithValue("$date", entry.Date.ToString("yyyy-MM-dd"));
        command.Parameters.AddWithValue("$time", entry.Time.ToString(@"hh\:mm\:ss"));
        command.Parameters.AddWithValue("$notes", entry.Notes ?? string.Empty);
        command.Parameters.AddWithValue("$source", entry.SampleSource ?? string.Empty);
        command.Parameters.AddWithValue("$location", entry.CollectionLocation ?? string.Empty);
        command.Parameters.AddWithValue("$type", entry.SampleType ?? string.Empty);
        command.Parameters.AddWithValue("$group", (int)entry.OrganismGroup);
        command.Parameters.AddWithValue("$ident", entry.Identification ?? string.Empty);
        command.Parameters.AddWithValue("$conf", (int)entry.IdentificationConfidence);
        command.ExecuteNonQuery();
    }

    foreach (string path in entry.ImagePaths)
    {
        using var imgCommand = connection.CreateCommand();
        imgCommand.Transaction = transaction;
        imgCommand.CommandText = @"
            INSERT INTO EntryImages (EntryId, ImagePath)
            VALUES ($entryId, $imagePath);
        ";
        imgCommand.Parameters.AddWithValue("$entryId", entry.Id.ToString());
        imgCommand.Parameters.AddWithValue("$imagePath", path);
        imgCommand.ExecuteNonQuery();
    }

    transaction.Commit();
}

public List<MicroscopyEntry> GetAllEntries()
{
    var entries = new List<MicroscopyEntry>();
    using var connection = GetConnection();
    connection.Open();

    using var command = connection.CreateCommand();
    command.CommandText = "SELECT * FROM MicroscopyEntries;";

    using var reader = command.ExecuteReader();
    while (reader.Read())
    {
        // Robustly parse OrganismGroup whether stored as int, text, or empty
        var organismGroupVal = MicroorganismGroup.Unidentified;
        if (reader["OrganismGroup"] != DBNull.Value)
        {
            string groupStr = reader["OrganismGroup"].ToString()!;
            if (int.TryParse(groupStr, out int groupInt))
            {
                organismGroupVal = (MicroorganismGroup)groupInt;
            }
            else if (Enum.TryParse<MicroorganismGroup>(groupStr, true, out var parsedEnum))
            {
                organismGroupVal = parsedEnum;
            }
        }

        var entry = new MicroscopyEntry
        {
            Id = Guid.Parse(reader["Id"].ToString()!),
            ObservationId = reader["ObservationId"]?.ToString() ?? string.Empty,
            Name = reader["Name"].ToString()!,
            Date = DateTime.Parse(reader["EntryDate"].ToString()!),
            Time = TimeSpan.Parse(reader["EntryTime"].ToString()!),
            Notes = reader["Notes"]?.ToString() ?? string.Empty,
            
            SampleSource = reader["SampleSource"]?.ToString() ?? string.Empty,
            CollectionLocation = reader["CollectionLocation"]?.ToString() ?? string.Empty,
            SampleType = reader["SampleType"]?.ToString() ?? string.Empty,
            OrganismGroup = organismGroupVal, // Safely assigned parsed enum
            Identification = reader["Identification"]?.ToString() ?? string.Empty,
            IdentificationConfidence = reader["IdentificationConfidence"] != DBNull.Value 
                ? (IdentificationConfidence)Convert.ToInt32(reader["IdentificationConfidence"]) 
                : IdentificationConfidence.Possible
        };

        entry.ImagePaths = GetImagePathsForEntry(entry.Id);
        entries.Add(entry);
    }

    return entries;
}

    private List<string> GetImagePathsForEntry(Guid entryId)
    {
        var paths = new List<string>();
        using var connection = GetConnection();
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = "SELECT ImagePath FROM EntryImages WHERE EntryId = $entryId;";
        command.Parameters.AddWithValue("$entryId", entryId.ToString());

        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            paths.Add(reader["ImagePath"].ToString()!);
        }
        return paths;
    }

    public void DeleteEntry(Guid id)
    {
        using var connection = GetConnection();
        // Note: connection.Open() is already handled inside GetConnection(), 
        // but leaving it won't break SqliteConnection.

        using var command = connection.CreateCommand();
        command.CommandText = "DELETE FROM MicroscopyEntries WHERE Id = $id;";
        command.Parameters.AddWithValue("$id", id.ToString());

        command.ExecuteNonQuery();
    }
    
    public void UpdateEntry(MicroscopyEntry entry)
    {
        using var connection = GetConnection();
        connection.Open();
        using var command = connection.CreateCommand();
        command.CommandText = @"
        UPDATE MicroscopyEntries 
        SET Name = $name, 
            EntryDate = $date, 
            EntryTime = $time, 
            Notes = $notes, 
            SampleSource = $source, 
            CollectionLocation = $location, 
            SampleType = $type, 
            OrganismGroup = $group, 
            Identification = $ident, 
            IdentificationConfidence = $conf
        WHERE Id = $id;
    ";
        command.Parameters.AddWithValue("$id", entry.Id.ToString());
        command.Parameters.AddWithValue("$name", entry.Name);
        command.Parameters.AddWithValue("$date", entry.Date.ToString("yyyy-MM-dd"));
        command.Parameters.AddWithValue("$time", entry.Time.ToString(@"hh\:mm\:ss"));
        command.Parameters.AddWithValue("$notes", entry.Notes ?? string.Empty);
        command.Parameters.AddWithValue("$source", entry.SampleSource ?? string.Empty);
        command.Parameters.AddWithValue("$location", entry.CollectionLocation ?? string.Empty);
        command.Parameters.AddWithValue("$type", entry.SampleType ?? string.Empty);
        command.Parameters.AddWithValue("$group", (int)entry.OrganismGroup);
        command.Parameters.AddWithValue("$ident", entry.Identification ?? string.Empty);
        command.Parameters.AddWithValue("$conf", (int)entry.IdentificationConfidence);
        command.ExecuteNonQuery();
    }

    public void ClearAllData()
    {
        using var connection = GetConnection();
        using var transaction = connection.BeginTransaction();

        using (var command = connection.CreateCommand())
        {
            command.Transaction = transaction;
            // Foreign key constraints will cascade-delete EntryImages as well
            command.CommandText = "DELETE FROM MicroscopyEntries;";
            command.ExecuteNonQuery();
        }

        transaction.Commit();

        // Clean up physical files on disk
        if (Directory.Exists(EntriesFolder))
        {
            Directory.Delete(EntriesFolder, true);
            Directory.CreateDirectory(EntriesFolder); // Re-create empty root folder
        }
    }
}