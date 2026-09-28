using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq; // Added for LINQ filtering (.Where)
using System.Windows;
using System.Windows.Controls;
using MicroscopyApp.Models;
using Microsoft.Win32;

namespace Microscopy_Database;

public partial class MainWindow : Window
{
    public ObservableCollection<MicroscopyEntry> Entries { get; set; } = new();

    private List<string> _selectedSourceImagePaths = new();

    public MainWindow()
    {
        InitializeComponent();

        DpDate.SelectedDate = DateTime.Today;
        TxtTime.Text = DateTime.Now.ToString(@"hh\:mm\:ss");

        // Bind both layout containers to the ObservableCollection
        GridCardsContainer.ItemsSource = Entries;
        ListViewContainer.ItemsSource = Entries;

        LoadEntriesFromDatabase();
    }

    private void LoadEntriesFromDatabase()
    {
        Entries.Clear();
        var data = DatabaseManager.Instance.GetAllEntries();
        foreach (var entry in data)
        {
            Entries.Add(entry);
        }

        // Re-apply filter in case search box has existing text
        ApplySearchFilter();
    }

    // Helper method to filter records across Name, Date, Time, and Notes
    private void ApplySearchFilter()
    {
        string query = TxtSearch.Text.Trim();

        // If search field is empty, show all items
        if (string.IsNullOrWhiteSpace(query))
        {
            GridCardsContainer.ItemsSource = Entries;
            ListViewContainer.ItemsSource = Entries;
            return;
        }

        // Filter entries across all available fields (case-insensitive)
        var filteredList = Entries.Where(entry =>
            (entry.ObservationId != null && entry.ObservationId.Contains(query, StringComparison.OrdinalIgnoreCase)) ||
            (entry.Name != null && entry.Name.Contains(query, StringComparison.OrdinalIgnoreCase)) ||
            entry.Date.ToString("yyyy-MM-dd").Contains(query, StringComparison.OrdinalIgnoreCase) ||
            entry.Time.ToString(@"hh\:mm\:ss").Contains(query, StringComparison.OrdinalIgnoreCase) ||
            (entry.SampleSource != null && entry.SampleSource.Contains(query, StringComparison.OrdinalIgnoreCase)) ||
            (entry.CollectionLocation != null && entry.CollectionLocation.Contains(query, StringComparison.OrdinalIgnoreCase)) ||
            (entry.SampleType != null && entry.SampleType.Contains(query, StringComparison.OrdinalIgnoreCase)) ||
            entry.OrganismGroup.ToString().Contains(query, StringComparison.OrdinalIgnoreCase) ||  
            (entry.Identification != null && entry.Identification.Contains(query, StringComparison.OrdinalIgnoreCase)) ||
            entry.IdentificationConfidence.ToString().Contains(query, StringComparison.OrdinalIgnoreCase) ||
            (entry.Notes != null && entry.Notes.Contains(query, StringComparison.OrdinalIgnoreCase))
        ).ToList();

        GridCardsContainer.ItemsSource = filteredList;
        ListViewContainer.ItemsSource = filteredList;
    }

    private void BtnDeleteEntry_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.Tag is Guid entryId)
        {
            // Find the entry in our active collection
            var entryToDelete = Entries.FirstOrDefault(x => x.Id == entryId);
            if (entryToDelete == null) return;

            // 1. Show confirmation dialog
            var result = MessageBox.Show(
                $"Are you sure you want to delete the record '{entryToDelete.Name}'?\n\nThis will remove the entry and delete its folder from disk.",
                "Confirm Delete Entry",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning,
                MessageBoxResult.No
            );

            if (result == MessageBoxResult.Yes)
            {
                // 2. Remove entry from SQLite database
                DatabaseManager.Instance.DeleteEntry(entryId);

                // 3. Delete entry folder on disk
                string folderPath = Path.Combine(DatabaseManager.EntriesFolder, entryId.ToString());
                if (Directory.Exists(folderPath))
                {
                    try
                    {
                        Directory.Delete(folderPath, true);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Entry removed from database, but failed to delete folder on disk: {ex.Message}",
                            "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }

                // 4. Remove from ObservableCollection & refresh search view
                Entries.Remove(entryToDelete);
                ApplySearchFilter();
            }
        }
    }

    private void BtnHelp_Click(object sender, RoutedEventArgs e)
    {
        var helpWindow = new MicrobiologyResourcesWindow
        {
            Owner = this
        };
        helpWindow.ShowDialog();
    }

    private void EntryCard_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        // Ensure clicking buttons (like folder or image buttons) inside card won't trigger the main window popup twice
        if (e.OriginalSource is Button || e.OriginalSource is Image)
            return;

        if (sender is FrameworkElement element && element.DataContext is MicroscopyEntry entry)
        {
            var detailWindow = new EntryDetailWindow(entry)
            {
                Owner = this // Centers detail pop-up on top of MainWindow
            };
            detailWindow.ShowDialog();
        }
    }

    private void TxtSearch_TextChanged(object sender, TextChangedEventArgs e)
    {
        ApplySearchFilter();
    }

    private void BtnBrowseImages_Click(object sender, RoutedEventArgs e)
    {
        var openFileDialog = new OpenFileDialog
        {
            Multiselect = true,
            Filter = "Image Files (*.png;*.jpg;*.jpeg;*.tiff;*.bmp)|*.png;*.jpg;*.jpeg;*.tiff;*.bmp|All Files (*.*)|*.*"
        };

        if (openFileDialog.ShowDialog() == true)
        {
            foreach (string file in openFileDialog.FileNames)
            {
                if (!_selectedSourceImagePaths.Contains(file))
                {
                    _selectedSourceImagePaths.Add(file);
                }
            }

            LstSelectedImages.ItemsSource = null;
            LstSelectedImages.ItemsSource = _selectedSourceImagePaths;

            TxtImages.Text = $"{_selectedSourceImagePaths.Count} image(s) attached";
        }
    }

    private void BtnClearDatabase_Click(object sender, RoutedEventArgs e)
    {
        var result = MessageBox.Show(
            "Are you sure you want to clear the entire database?\n\nThis will permanently delete all records and entry folders on disk. This action cannot be undone.",
            "Confirm Clear Database",
            MessageBoxButton.YesNo,
            MessageBoxImage.Warning,
            MessageBoxResult.No
        );

        if (result == MessageBoxResult.Yes)
        {
            DatabaseManager.Instance.ClearAllData();
            Entries.Clear();
            ApplySearchFilter(); // Refresh containers

            MessageBox.Show("Database has been successfully cleared.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }

    private void BtnOpenImage_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.Tag is string imagePath)
        {
            if (File.Exists(imagePath))
            {
                Process.Start(new ProcessStartInfo(imagePath)
                {
                    UseShellExecute = true
                });
            }
            else
            {
                MessageBox.Show("Image file could not be found at destination path.",
                    "File Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    private void BtnOpenFolder_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.Tag is Guid entryId)
        {
            string folderPath = Path.Combine(DatabaseManager.EntriesFolder, entryId.ToString());
            if (Directory.Exists(folderPath))
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = folderPath,
                    UseShellExecute = true,
                    Verb = "open"
                });
            }
            else
            {
                MessageBox.Show("Entry folder does not exist on disk.",
                    "Folder Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    private void LstSelectedImages_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        if (LstSelectedImages.SelectedItem is string selectedPath)
        {
            _selectedSourceImagePaths.Remove(selectedPath);

            LstSelectedImages.ItemsSource = null;
            LstSelectedImages.ItemsSource = _selectedSourceImagePaths;

            TxtImages.Text = _selectedSourceImagePaths.Count > 0
                ? $"{_selectedSourceImagePaths.Count} image(s) attached"
                : string.Empty;
        }
    }
    
    private void BtnOpenTaxonomyGuide_Click(object sender, RoutedEventArgs e)
    {
        var taxonomyWindow = new TaxonomyReferenceWindow
        {
            Owner = this
        };
        taxonomyWindow.ShowDialog();
    }

private void BtnSave_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(TxtName.Text))
        {
            MessageBox.Show("Please enter a name for the record.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        if (!TimeSpan.TryParse(TxtTime.Text, out var parsedTime))
        {
            MessageBox.Show("Invalid time format. Use HH:mm:ss.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        // Parse the Enum from the ComboBox dropdown
        MicroorganismGroup selectedGroup = MicroorganismGroup.Unidentified;
        if (CbOrganismGroup.SelectedItem is ComboBoxItem selectedItem && selectedItem.Tag is string tagStr)
        {
            Enum.TryParse(tagStr, out selectedGroup);
        }

        // Auto-generate sequential Observation ID (OBS001, OBS002...)
        int nextNumber = DatabaseManager.Instance.GetTotalEntryCount() + 1;
        string observationId = $"OBS{nextNumber:D3}";

        var entryId = Guid.NewGuid();
        string entryFolder = Path.Combine(DatabaseManager.EntriesFolder, entryId.ToString());
        string imagesFolder = Path.Combine(entryFolder, "images");

        Directory.CreateDirectory(entryFolder);
        Directory.CreateDirectory(imagesFolder);

        // Read Confidence enum value from ComboBox selection
        IdentificationConfidence confidence = IdentificationConfidence.Possible;
        if (CbConfidence.SelectedItem is ComboBoxItem confItem && 
            Enum.TryParse<IdentificationConfidence>(confItem.Content?.ToString(), true, out var parsedConf))
        {
            confidence = parsedConf;
        }

        string notesFilePath = Path.Combine(entryFolder, "notes.txt");
        string notesContent =
            $"Observation ID: {observationId}\n" +
            $"ID: {entryId}\n" +
            $"Name: {TxtName.Text.Trim()}\n" +
            $"Date: {DpDate.SelectedDate:yyyy-MM-dd}\n" +
            $"Time: {parsedTime:hh\\:mm\\:ss}\n" +
            $"Sample Source: {TxtSampleSource.Text.Trim()}\n" +
            $"Collection Location: {TxtCollectionLocation.Text.Trim()}\n" +
            $"Sample Type: {TxtSampleType.Text.Trim()}\n" +
            $"Organism Group: {selectedGroup}\n" +
            $"Identification: {TxtIdentification.Text.Trim()}\n" +
            $"Confidence: {confidence}\n" +
            $"----------------------------------------\n" +
            $"Notes:\n{TxtNotes.Text.Trim()}\n";

        File.WriteAllText(notesFilePath, notesContent);

        var savedImagePaths = new List<string>();
        foreach (string sourcePath in _selectedSourceImagePaths)
        {
            if (File.Exists(sourcePath))
            {
                string fileName = Path.GetFileName(sourcePath);
                string destinationPath = Path.Combine(imagesFolder, fileName);

                File.Copy(sourcePath, destinationPath, true);
                savedImagePaths.Add(destinationPath);
            }
        }

        var newEntry = new MicroscopyEntry
        {
            Id = entryId,
            ObservationId = observationId,
            Name = TxtName.Text.Trim(),
            Date = DpDate.SelectedDate ?? DateTime.Today,
            Time = parsedTime,
            Notes = TxtNotes.Text.Trim(),
            ImagePaths = savedImagePaths,
            
            // Populate metadata from UI inputs
            SampleSource = TxtSampleSource.Text.Trim(),
            CollectionLocation = TxtCollectionLocation.Text.Trim(),
            SampleType = TxtSampleType.Text.Trim(),
            OrganismGroup = selectedGroup, 
            Identification = TxtIdentification.Text.Trim(),
            IdentificationConfidence = confidence
        };

        DatabaseManager.Instance.AddEntry(newEntry);

        Entries.Add(newEntry);
        
        // Refresh display grid/list based on current search filter
        ApplySearchFilter();

        // Clear all inputs after save
        TxtName.Clear();
        TxtSampleSource.Clear();
        TxtCollectionLocation.Clear();
        TxtSampleType.Clear();
        CbOrganismGroup.SelectedIndex = 0; // Reset ComboBox selection
        TxtIdentification.Clear();
        CbConfidence.SelectedIndex = 0;
        TxtNotes.Clear();
        TxtImages.Clear();
        _selectedSourceImagePaths.Clear();
        LstSelectedImages.ItemsSource = null;
        TxtTime.Text = DateTime.Now.ToString(@"hh\:mm\:ss");
    }
}