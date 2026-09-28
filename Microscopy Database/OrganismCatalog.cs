using System.Collections.Generic;

namespace MicroscopyApp.Models;

public static class OrganismCatalog
{
    public static List<OrganismReferenceInfo> GetAll()
    {
        return new()
        {
            // --- PROTOZOA & CILIATES ---
            new() { 
                Group = MicroorganismGroup.Paramecium, 
                DisplayName = "Paramecium (Ciliate)", 
                Category = "Protozoa & Ciliates", 
                WikipediaUrl = "https://en.wikipedia.org/wiki/Paramecium", 
                ImageFileName = "paramecium.jpg" 
            },
            new() { 
                Group = MicroorganismGroup.Vorticella, 
                DisplayName = "Vorticella (Stalked Ciliate)", 
                Category = "Protozoa & Ciliates", 
                WikipediaUrl = "https://en.wikipedia.org/wiki/Vorticella", 
                ImageFileName = "vorticella.jpg" 
            },
            new() { 
                Group = MicroorganismGroup.Stentor, 
                DisplayName = "Stentor (Trumpet Ciliate)", 
                Category = "Protozoa & Ciliates", 
                WikipediaUrl = "https://en.wikipedia.org/wiki/Stentor_(protozoan)", 
                ImageFileName = "stentor.jpg" 
            },
            new() { 
                Group = MicroorganismGroup.Spirostomum, 
                DisplayName = "Spirostomum (Worm Ciliate)", 
                Category = "Protozoa & Ciliates", 
                WikipediaUrl = "https://en.wikipedia.org/wiki/Spirostomum", 
                ImageFileName = "spirostomum.jpg" 
            },
            new() { 
                Group = MicroorganismGroup.Euplotes, 
                DisplayName = "Euplotes (Hypotrich Ciliate)", 
                Category = "Protozoa & Ciliates", 
                WikipediaUrl = "https://en.wikipedia.org/wiki/Euplotes", 
                ImageFileName = "euplotes.jpg" 
            },
            new() { 
                Group = MicroorganismGroup.ColpidiumColpoda, 
                DisplayName = "Colpidium / Colpoda", 
                Category = "Protozoa & Ciliates", 
                WikipediaUrl = "https://en.wikipedia.org/wiki/Colpoda", 
                ImageFileName = "colpoda.jpg" 
            },
            new() { 
                Group = MicroorganismGroup.Lacrymaria, 
                DisplayName = "Lacrymaria (Swan Animalcule)", 
                Category = "Protozoa & Ciliates", 
                WikipediaUrl = "https://en.wikipedia.org/wiki/Lacrymaria_olor", 
                ImageFileName = "lacrymaria.jpg" 
            },
            new() { 
                Group = MicroorganismGroup.Blepharisma, 
                DisplayName = "Blepharisma (Pink Ciliate)", 
                Category = "Protozoa & Ciliates", 
                WikipediaUrl = "https://en.wikipedia.org/wiki/Blepharisma", 
                ImageFileName = "blepharisma.jpg" 
            },
            new() { 
                Group = MicroorganismGroup.Halteria, 
                DisplayName = "Halteria", 
                Category = "Protozoa & Ciliates", 
                WikipediaUrl = "https://en.wikipedia.org/wiki/Halteria", 
                ImageFileName = "halteria.jpg" 
            },
            new() { 
                Group = MicroorganismGroup.Didinium, 
                DisplayName = "Didinium", 
                Category = "Protozoa & Ciliates", 
                WikipediaUrl = "https://en.wikipedia.org/wiki/Didinium", 
                ImageFileName = "didinium.jpg" 
            },

            // --- AMOEBOIDS & FLAGELLATES ---
            new() { 
                Group = MicroorganismGroup.AmoebaProteus, 
                DisplayName = "Amoeba proteus (Amoeba)", 
                Category = "Amoeboids & Flagellates", 
                WikipediaUrl = "https://en.wikipedia.org/wiki/Amoeba_proteus", 
                ImageFileName = "amoeba.jpg" 
            },
            new() { 
                Group = MicroorganismGroup.Arcella, 
                DisplayName = "Arcella (Testate Amoeba)", 
                Category = "Amoeboids & Flagellates", 
                WikipediaUrl = "https://en.wikipedia.org/wiki/Arcella", 
                ImageFileName = "arcella.jpg" 
            },
            new() { 
                Group = MicroorganismGroup.Difflugia, 
                DisplayName = "Difflugia (Shell-building Amoeba)", 
                Category = "Amoeboids & Flagellates", 
                WikipediaUrl = "https://en.wikipedia.org/wiki/Difflugia", 
                ImageFileName = "difflugia.jpg" 
            },
            new() { 
                Group = MicroorganismGroup.Actinophrys, 
                DisplayName = "Actinophrys (Sun Animalcule)", 
                Category = "Amoeboids & Flagellates", 
                WikipediaUrl = "https://en.wikipedia.org/wiki/Actinophrys", 
                ImageFileName = "actinophrys.jpg" 
            },
            new() { 
                Group = MicroorganismGroup.Euglena, 
                DisplayName = "Euglena (Euglenoid)", 
                Category = "Amoeboids & Flagellates", 
                WikipediaUrl = "https://en.wikipedia.org/wiki/Euglena", 
                ImageFileName = "euglena.jpg" 
            },
            new() { 
                Group = MicroorganismGroup.Volvox, 
                DisplayName = "Volvox (Colonial Flagellate)", 
                Category = "Amoeboids & Flagellates", 
                WikipediaUrl = "https://en.wikipedia.org/wiki/Volvox", 
                ImageFileName = "volvox.jpg" 
            },
            new() { 
                Group = MicroorganismGroup.Phacus, 
                DisplayName = "Phacus", 
                Category = "Amoeboids & Flagellates", 
                WikipediaUrl = "https://en.wikipedia.org/wiki/Phacus", 
                ImageFileName = "phacus.jpg" 
            },
            new() { 
                Group = MicroorganismGroup.Peranema, 
                DisplayName = "Peranema", 
                Category = "Amoeboids & Flagellates", 
                WikipediaUrl = "https://en.wikipedia.org/wiki/Peranema", 
                ImageFileName = "peranema.jpg" 
            },

            // --- MICRO-ANIMALS & POND INVERTEBRATES ---
            new() { 
                Group = MicroorganismGroup.Rotifera, 
                DisplayName = "Rotifera (Rotifers / Wheel Animalcules)", 
                Category = "Pond Invertebrates", 
                WikipediaUrl = "https://en.wikipedia.org/wiki/Rotifer", 
                ImageFileName = "rotifer.jpg" 
            },
            new() { 
                Group = MicroorganismGroup.Hydra, 
                DisplayName = "Hydra (Cnidaria)", 
                Category = "Pond Invertebrates", 
                WikipediaUrl = "https://en.wikipedia.org/wiki/Hydra_(genus)", 
                ImageFileName = "hydra.jpg" 
            },
            new() { 
                Group = MicroorganismGroup.Daphnia, 
                DisplayName = "Daphnia (Water Flea / Cladocera)", 
                Category = "Pond Invertebrates", 
                WikipediaUrl = "https://en.wikipedia.org/wiki/Daphnia", 
                ImageFileName = "daphnia.jpg" 
            },
            new() { 
                Group = MicroorganismGroup.Cyclops, 
                DisplayName = "Cyclops (Copepod)", 
                Category = "Pond Invertebrates", 
                WikipediaUrl = "https://en.wikipedia.org/wiki/Cyclops_(genus)", 
                ImageFileName = "cyclops.jpg" 
            },
            new() { 
                Group = MicroorganismGroup.Ostracod, 
                DisplayName = "Ostracod (Seed Shrimp)", 
                Category = "Pond Invertebrates", 
                WikipediaUrl = "https://en.wikipedia.org/wiki/Ostracoda", 
                ImageFileName = "ostracod.jpg" 
            },
            new() { 
                Group = MicroorganismGroup.Tardigrade, 
                DisplayName = "Tardigrade (Water Bear)", 
                Category = "Pond Invertebrates", 
                WikipediaUrl = "https://en.wikipedia.org/wiki/Tardigrade", 
                ImageFileName = "tardigrade.jpg" 
            },
            new() { 
                Group = MicroorganismGroup.Nematoda, 
                DisplayName = "Nematoda (Roundworm)", 
                Category = "Pond Invertebrates", 
                WikipediaUrl = "https://en.wikipedia.org/wiki/Nematode", 
                ImageFileName = "nematoda.jpg" 
            },
            new() { 
                Group = MicroorganismGroup.Gastrotrich, 
                DisplayName = "Gastrotrich", 
                Category = "Pond Invertebrates", 
                WikipediaUrl = "https://en.wikipedia.org/wiki/Gastrotricha", 
                ImageFileName = "gastrotrich.jpg" 
            },
            new() { 
                Group = MicroorganismGroup.Planarian, 
                DisplayName = "Planarian (Flatworm)", 
                Category = "Pond Invertebrates", 
                WikipediaUrl = "https://en.wikipedia.org/wiki/Planarian", 
                ImageFileName = "planarian.jpg" 
            },
            new() { 
                Group = MicroorganismGroup.Bryozoan, 
                DisplayName = "Bryozoan (Moss Animal statoblast)", 
                Category = "Pond Invertebrates", 
                WikipediaUrl = "https://en.wikipedia.org/wiki/Bryozoa", 
                ImageFileName = "bryozoan.jpg" 
            },

            // --- ALGAE & PHYTOPLANKTON ---
            new() { 
                Group = MicroorganismGroup.Spirogyra, 
                DisplayName = "Spirogyra (Green Algae Filament)", 
                Category = "Algae & Phytoplankton", 
                WikipediaUrl = "https://en.wikipedia.org/wiki/Spirogyra", 
                ImageFileName = "spirogyra.jpg" 
            },
            new() { 
                Group = MicroorganismGroup.Closterium, 
                DisplayName = "Closterium (Desmid)", 
                Category = "Algae & Phytoplankton", 
                WikipediaUrl = "https://en.wikipedia.org/wiki/Closterium", 
                ImageFileName = "closterium.jpg" 
            },
            new() { 
                Group = MicroorganismGroup.Cosmarium, 
                DisplayName = "Cosmarium (Desmid)", 
                Category = "Algae & Phytoplankton", 
                WikipediaUrl = "https://en.wikipedia.org/wiki/Cosmarium", 
                ImageFileName = "cosmarium.jpg" 
            },
            new() { 
                Group = MicroorganismGroup.Pediastrum, 
                DisplayName = "Pediastrum (Colonial Green Algae)", 
                Category = "Algae & Phytoplankton", 
                WikipediaUrl = "https://en.wikipedia.org/wiki/Pediastrum", 
                ImageFileName = "pediastrum.jpg" 
            },
            new() { 
                Group = MicroorganismGroup.Scenedesmus, 
                DisplayName = "Scenedesmus", 
                Category = "Algae & Phytoplankton", 
                WikipediaUrl = "https://en.wikipedia.org/wiki/Scenedesmus", 
                ImageFileName = "scenedesmus.jpg" 
            },
            new() { 
                Group = MicroorganismGroup.DiatomCentric, 
                DisplayName = "Diatoms - Centric", 
                Category = "Algae & Phytoplankton", 
                WikipediaUrl = "https://en.wikipedia.org/wiki/Diatom", 
                ImageFileName = "diatom_centric.jpg" 
            },
            new() { 
                Group = MicroorganismGroup.DiatomPennate, 
                DisplayName = "Diatoms - Pennate", 
                Category = "Algae & Phytoplankton", 
                WikipediaUrl = "https://en.wikipedia.org/wiki/Diatom", 
                ImageFileName = "diatom_pennate.jpg" 
            },
            new() { 
                Group = MicroorganismGroup.Cyanobacteria, 
                DisplayName = "Cyanobacteria (Blue-Green Algae)", 
                Category = "Algae & Phytoplankton", 
                WikipediaUrl = "https://en.wikipedia.org/wiki/Cyanobacteria", 
                ImageFileName = "cyanobacteria.jpg" 
            },
            new() { 
                Group = MicroorganismGroup.Cladophora, 
                DisplayName = "Cladophora", 
                Category = "Algae & Phytoplankton", 
                WikipediaUrl = "https://en.wikipedia.org/wiki/Cladophora", 
                ImageFileName = "cladophora.jpg" 
            },

            // --- FUNGI & BACTERIA ---
            new() { 
                Group = MicroorganismGroup.Yeast, 
                DisplayName = "Saccharomycetaceae (Budding Yeast)", 
                Category = "Fungi & Bacteria", 
                WikipediaUrl = "https://en.wikipedia.org/wiki/Yeast", 
                ImageFileName = "yeast.jpg" 
            },
            new() { 
                Group = MicroorganismGroup.Actinomycetes, 
                DisplayName = "Actinomycetes (Filamentous Bacteria)", 
                Category = "Fungi & Bacteria", 
                WikipediaUrl = "https://en.wikipedia.org/wiki/Actinomycetota", 
                ImageFileName = "actinomycetes.jpg" 
            },
            new() { 
                Group = MicroorganismGroup.Zoogloea, 
                DisplayName = "Zoogloea (Bacterial Matrix)", 
                Category = "Fungi & Bacteria", 
                WikipediaUrl = "https://en.wikipedia.org/wiki/Zoogloea", 
                ImageFileName = "zoogloea.jpg" 
            },

            // --- OTHER / UNKNOWN ---
            new() { 
                Group = MicroorganismGroup.Unidentified, 
                DisplayName = "Unidentified Specimen", 
                Category = "Other / Unknown", 
                WikipediaUrl = "https://en.wikipedia.org/wiki/Microorganism", 
                ImageFileName = "unidentified.jpg" 
            },
            new() { 
                Group = MicroorganismGroup.Detritus, 
                DisplayName = "Detritus / Mineral Particle", 
                Category = "Other / Unknown", 
                WikipediaUrl = "https://en.wikipedia.org/wiki/Detritus", 
                ImageFileName = "detritus.jpg" 
            }
        };
    }
}