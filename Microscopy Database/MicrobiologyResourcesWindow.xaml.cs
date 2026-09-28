using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Microscopy_Database
{
    // Helper model class placed outside the window class
    public class ResourceLink
    {
        public string Category { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
    }

    public partial class MicrobiologyResourcesWindow : Window
    {
        public MicrobiologyResourcesWindow()
        {
            InitializeComponent();
            LoadResourceLinks();
        }

        private void LoadResourceLinks()
        {
            var resources = new List<ResourceLink>
            {
                // Educational & Learning
                new ResourceLink {
                    Category = "Educational Courses & Learning",
                    Title = "CDC – MicrobeNet",
                    Description = "Virtual portal for microbial identification and information.",
                    Url = "https://www.cdc.gov/microbenet/"
                },
                new ResourceLink {
                    Category = "Educational Courses & Learning",
                    Title = "ASM – American Society for Microbiology",
                    Description = "Educational materials, protocols, and microbiology research.",
                    Url = "https://asm.org/"
                },
                new ResourceLink {
                    Category = "Educational Courses & Learning",
                    Title = "Microbiology Online (Microbiology Society)",
                    Description = "A hub for understanding microbes, staining methods, and cell biology.",
                    Url = "https://microbiologysociety.org/"
                },

                // Staining & Microscopy Guides
                new ResourceLink {
                    Category = "Microscopy & Staining Guides",
                    Title = "Microscope World – Educational Guides",
                    Description = "Guides on Gram staining, brightfield setup, and magnification standards.",
                    Url = "https://www.microscopeworld.com/"
                },
                new ResourceLink {
                    Category = "Microscopy & Staining Guides",
                    Title = "Biolib – Microorganism Library",
                    Description = "Taxonomy and images of algae, fungi, bacteria, and protozoa.",
                    Url = "https://www.biolib.cz/en/"
                },

                // Databases & Taxonomy
                new ResourceLink {
                    Category = "Databases & Taxonomy",
                    Title = "NCBI Taxonomy Database",
                    Description = "Comprehensive taxonomy browser for bacteria, archaea, and fungi.",
                    Url = "https://www.ncbi.nlm.nih.gov/taxonomy"
                },
                new ResourceLink {
                    Category = "Databases & Taxonomy",
                    Title = "AlgaeBase",
                    Description = "Database of terrestrial, freshwater, and marine algae organisms.",
                    Url = "https://www.algaebase.org/"
                },
                new ResourceLink {
                    Category = "Databases & Taxonomy",
                    Title = "Organism Types",
                    Description = "A decent list of micro-organisms and categories.",
                    Url = "http://www.microscopy-uk.org.uk/index.html?http://www.microscopy-uk.org.uk/pond/algae.html"
                }
                
            };

            string currentCategory = "";
            foreach (var item in resources)
            {
                if (item.Category != currentCategory)
                {
                    currentCategory = item.Category;
                    
                    var categoryHeader = new TextBlock
                    {
                        Text = currentCategory,
                        FontSize = 14,
                        FontWeight = FontWeights.Bold,
                        Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#007ACC")),
                        Margin = new Thickness(0, 15, 0, 8)
                    };
                    ResourcesStackPanel.Children.Add(categoryHeader);
                }

                var card = new Border
                {
                    Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1E1E1E")),
                    BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2D2D30")),
                    BorderThickness = new Thickness(1),
                    CornerRadius = new CornerRadius(6),
                    Padding = new Thickness(12, 10, 12, 10),
                    Margin = new Thickness(0, 0, 0, 8)
                };

                var grid = new Grid();
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

                var textStack = new StackPanel { VerticalAlignment = VerticalAlignment.Center };
                var titleText = new TextBlock { Text = item.Title, FontWeight = FontWeights.SemiBold, Foreground = Brushes.White, FontSize = 13 };
                var descText = new TextBlock { Text = item.Description, Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#AAAAAA")), FontSize = 11, Margin = new Thickness(0, 2, 0, 0) };

                textStack.Children.Add(titleText);
                textStack.Children.Add(descText);
                Grid.SetColumn(textStack, 0);

                var openBtn = new Button
                {
                    Content = "🔗 Open",
                    Tag = item.Url,
                    Padding = new Thickness(10, 5, 10, 5),
                    Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3E3E42")),
                    Foreground = Brushes.White,
                    BorderThickness = new Thickness(0),
                    Cursor = System.Windows.Input.Cursors.Hand,
                    VerticalAlignment = VerticalAlignment.Center
                };
                openBtn.Click += OpenUrl_Click;
                Grid.SetColumn(openBtn, 1);

                grid.Children.Add(textStack);
                grid.Children.Add(openBtn);
                card.Child = grid;

                ResourcesStackPanel.Children.Add(card);
            }
        }

        private void OpenUrl_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is string url)
            {
                Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
            }
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}