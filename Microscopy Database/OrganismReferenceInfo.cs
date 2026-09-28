using System;
using System.IO;

namespace MicroscopyApp.Models;

public class OrganismReferenceInfo
{
    public MicroorganismGroup Group { get; set; }
    public string DisplayName { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string WikipediaUrl { get; set; } = string.Empty;

    private string _imageFileName = string.Empty;

    public string ImageFileName
    {
        get => _imageFileName;
        set => _imageFileName = value;
    }

    // Returns the absolute physical path so WPF Image control can load it successfully
    public string ImagePath
    {
        get
        {
            try
            {
                if (!string.IsNullOrEmpty(_imageFileName))
                {
                    string relativePath = Path.Combine("Assets", "Taxonomy", _imageFileName);
                    string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePath);

                    if (File.Exists(fullPath))
                    {
                        return fullPath;
                    }
                }
            }
            catch
            {
                // Fallback on error
            }

            // Fallback to default.jpg absolute path
            string defaultRelative = Path.Combine("Assets", "Taxonomy", "default.jpg");
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, defaultRelative);
        }
    }
}