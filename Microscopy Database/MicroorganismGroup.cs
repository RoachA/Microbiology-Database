namespace MicroscopyApp.Models;

public enum MicroorganismGroup
{
    Unidentified = 0,
    
    // Protozoa & Ciliates
    Paramecium,
    Vorticella,
    Stentor,
    Spirostomum,
    Euplotes,
    ColpidiumColpoda,
    Lacrymaria,
    Blepharisma,
    Halteria,
    Didinium,
    
    // Amoeboids & Flagellates
    AmoebaProteus,
    Arcella,
    Difflugia,
    Actinophrys,
    Euglena,
    Volvox,
    Phacus,
    Peranema,
    
    // Pond Invertebrates
    Rotifera,
    Hydra,
    Daphnia,
    Cyclops,
    Ostracod,
    Tardigrade,
    Nematoda,
    Gastrotrich,
    Planarian,
    Bryozoan,
    
    // Algae & Phytoplankton
    Spirogyra,
    Closterium,
    Cosmarium,
    Pediastrum,
    Scenedesmus,
    DiatomCentric,
    DiatomPennate,
    Cyanobacteria,
    Cladophora,
    
    // Fungi & Bacteria
    Yeast,
    Actinomycetes,
    Zoogloea,
    
    // Other
    Detritus
}