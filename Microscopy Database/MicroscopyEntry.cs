using System;
using System.Collections.Generic;

namespace MicroscopyApp.Models;

public class MicroscopyEntry
{
    public Guid Id { get; set; }
    public string ObservationId { get; set; } = string.Empty; // e.g., OBS001
    public string Name { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public TimeSpan Time { get; set; }
    public string Notes { get; set; } = string.Empty;
    public List<string> ImagePaths { get; set; } = new();

    // New detailed metadata fields
    public string SampleSource { get; set; } = string.Empty;
    public string CollectionLocation { get; set; } = string.Empty;
    public string SampleType { get; set; } = string.Empty;
    public MicroorganismGroup OrganismGroup { get; set; }
    public string Identification { get; set; } = string.Empty;
    public IdentificationConfidence IdentificationConfidence { get; set; } = IdentificationConfidence.Possible;
}