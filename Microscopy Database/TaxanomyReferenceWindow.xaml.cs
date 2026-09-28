using System.Windows;
using System.Windows.Data;
using MicroscopyApp.Models; // Adjust namespace to match your project

namespace Microscopy_Database;

public partial class TaxonomyReferenceWindow : Window
{
    public TaxonomyReferenceWindow()
    {
        InitializeComponent();
        LoadOrganisms();
    }

    private void LoadOrganisms()
    {
        // Get all items from the catalog
        var allOrganisms = OrganismCatalog.GetAll();

        // Create a CollectionView that groups items by their 'Category' property
        var collectionView = CollectionViewSource.GetDefaultView(allOrganisms);
        collectionView.GroupDescriptions.Add(new PropertyGroupDescription("Category"));

        // Bind the grouped view to the ItemsControl
        OrganismItemsControl.ItemsSource = collectionView;
    }

    private void Card_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        // Your existing click logic to open WikipediaUrl
        if (sender is FrameworkElement { DataContext: OrganismReferenceInfo info } && !string.IsNullOrEmpty(info.WikipediaUrl))
        {
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = info.WikipediaUrl,
                UseShellExecute = true
            });
        }
    }
}