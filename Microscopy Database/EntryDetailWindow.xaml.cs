using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using MicroscopyApp.Models;

namespace Microscopy_Database;

public partial class EntryDetailWindow : Window
{
    private readonly MicroscopyEntry _entry;
    private bool _isEditing = false;

    public EntryDetailWindow(MicroscopyEntry entry)
    {
        InitializeComponent();
        _entry = entry;
        LoadEntryDataToUI();
        SetEditMode(false);
    }

    private void LoadEntryDataToUI()
    {
        TxtObservationId.Text = string.IsNullOrEmpty(_entry.ObservationId) ? "OBS000" : _entry.ObservationId;
        TxtName.Text = _entry.Name;
        TxtDate.Text = $"📅 {_entry.Date:yyyy-MM-dd}";
        TxtTime.Text = $"⏰ {_entry.Time:hh\\:mm\\:ss}";
        TxtConfidence.Text = $"Confidence: {_entry.IdentificationConfidence}";

        // Metadata fields
        TxtSource.Text = _entry.SampleSource;
        TxtLocation.Text = _entry.CollectionLocation;
        TxtSampleType.Text = _entry.SampleType;
        
        // Match the database Enum value to the correct ComboBoxItem in CbOrganismGroup
        foreach (ComboBoxItem item in CbOrganismGroup.Items)
        {
            if (item.Tag is string tagStr && Enum.TryParse<MicroorganismGroup>(tagStr, out var groupVal))
            {
                if (groupVal == _entry.OrganismGroup)
                {
                    CbOrganismGroup.SelectedItem = item;
                    break;
                }
            }
        }

        TxtIdentification.Text = _entry.Identification;
        TxtNotes.Text = _entry.Notes;
        ImagesGalleryContainer.ItemsSource = _entry.ImagePaths;
    }

    private void BtnEditToggle_Click(object sender, RoutedEventArgs e)
    {
        if (!_isEditing)
        {
            // Switch to Edit Mode
            SetEditMode(true);
            BtnEditToggle.Content = "💾 Save Changes";
            BtnEditToggle.Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(40, 167, 69)); // Green
        }
        else
        {
            // Save Changes
            _entry.Name = TxtName.Text.Trim();
            _entry.SampleSource = TxtSource.Text.Trim();
            _entry.CollectionLocation = TxtLocation.Text.Trim();
            _entry.SampleType = TxtSampleType.Text.Trim();
            
            MicroorganismGroup selectedGroup = MicroorganismGroup.Unidentified;
            if (CbOrganismGroup.SelectedItem is ComboBoxItem selectedItem && selectedItem.Tag is string tagStr)
            {
                Enum.TryParse(tagStr, out selectedGroup);
            }
            _entry.OrganismGroup = selectedGroup;           
            
            _entry.Identification = TxtIdentification.Text.Trim();
            _entry.Notes = TxtNotes.Text.Trim();

            // Update database
            DatabaseManager.Instance.UpdateEntry(_entry);

            // Update notes.txt export file on disk
            string entryFolder = Path.Combine(DatabaseManager.EntriesFolder, _entry.Id.ToString());
            string notesFilePath = Path.Combine(entryFolder, "notes.txt");
            string notesContent =
                $"Observation ID: {_entry.ObservationId}\n" +
                $"ID: {_entry.Id}\n" +
                $"Name: {_entry.Name}\n" +
                $"Date: {_entry.Date:yyyy-MM-dd}\n" +
                $"Time: {_entry.Time:hh\\:mm\\:ss}\n" +
                $"Sample Source: {_entry.SampleSource}\n" +
                $"Collection Location: {_entry.CollectionLocation}\n" +
                $"Sample Type: {_entry.SampleType}\n" +
                $"Organism Group: {_entry.OrganismGroup}\n" +
                $"Identification: {_entry.Identification}\n" +
                $"Confidence: {_entry.IdentificationConfidence}\n" +
                $"----------------------------------------\n" +
                $"Notes:\n{_entry.Notes}\n";

            File.WriteAllText(notesFilePath, notesContent);

            MessageBox.Show("Record updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

            SetEditMode(false);
            BtnEditToggle.Content = "✏️ Edit Record";
            BtnEditToggle.Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(0, 122, 204)); // Blue
        }
    }

    private void SetEditMode(bool editable)
    {
        _isEditing = editable;

        var editableBackground = editable ? new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(45, 45, 48)) : System.Windows.Media.Brushes.Transparent;
        var editableBorder = editable ? new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(0, 122, 204)) : System.Windows.Media.Brushes.Transparent;
        double thickness = editable ? 1 : 0;

        void ApplyStyle(TextBox box)
        {
            box.IsReadOnly = !editable;
            box.Background = editableBackground;
            box.BorderBrush = editableBorder;
            box.BorderThickness = new Thickness(thickness);
        }

        ApplyStyle(TxtName);
        ApplyStyle(TxtSource);
        ApplyStyle(TxtLocation);
        ApplyStyle(TxtSampleType);
        ApplyStyle(TxtIdentification);
        ApplyStyle(TxtNotes);

        // Handle the ComboBox editing state instead of treating it like a TextBox
        CbOrganismGroup.IsEnabled = editable;
    }

    private void BtnOpenFolder_Click(object sender, RoutedEventArgs e)
    {
        string folderPath = Path.Combine(DatabaseManager.EntriesFolder, _entry.Id.ToString());
        if (Directory.Exists(folderPath))
        {
            Process.Start(new ProcessStartInfo { FileName = folderPath, UseShellExecute = true, Verb = "open" });
        }
    }

    private void BtnOpenImage_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.Tag is string imagePath && File.Exists(imagePath))
        {
            Process.Start(new ProcessStartInfo(imagePath) { UseShellExecute = true });
        }
    }

    private void BtnClose_Click(object sender, RoutedEventArgs e) => Close();
}