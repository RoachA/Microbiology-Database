# Create target directory if it doesn't exist
$outputDir = ".\Assets\Taxonomy"
if (!(Test-Path $outputDir)) {
    New-Item -ItemType Directory -Force -Path $outputDir | Out-Null
}

# Define list of organisms with their Wikipedia URL and target filename
$organisms = @(
    @{ Name = "Paramecium"; File = "paramecium.jpg"; Url = "https://en.wikipedia.org/wiki/Paramecium" },
    @{ Name = "Vorticella"; File = "vorticella.jpg"; Url = "https://en.wikipedia.org/wiki/Vorticella" },
    @{ Name = "Stentor"; File = "stentor.jpg"; Url = "https://en.wikipedia.org/wiki/Stentor_(protozoan)" },
    @{ Name = "Spirostomum"; File = "spirostomum.jpg"; Url = "https://en.wikipedia.org/wiki/Spirostomum" },
    @{ Name = "Euplotes"; File = "euplotes.jpg"; Url = "https://en.wikipedia.org/wiki/Euplotes" },
    @{ Name = "Colpoda"; File = "colpoda.jpg"; Url = "https://en.wikipedia.org/wiki/Colpoda" },
    @{ Name = "Lacrymaria"; File = "lacrymaria.jpg"; Url = "https://en.wikipedia.org/wiki/Lacrymaria_olor" },
    @{ Name = "Blepharisma"; File = "blepharisma.jpg"; Url = "https://en.wikipedia.org/wiki/Blepharisma" },
    @{ Name = "Halteria"; File = "halteria.jpg"; Url = "https://en.wikipedia.org/wiki/Halteria" },
    @{ Name = "Didinium"; File = "didinium.jpg"; Url = "https://en.wikipedia.org/wiki/Didinium" },
    @{ Name = "Amoeba", File = "amoeba.jpg"; Url = "https://en.wikipedia.org/wiki/Amoeba_proteus" },
    @{ Name = "Arcella"; File = "arcella.jpg"; Url = "https://en.wikipedia.org/wiki/Arcella" },
    @{ Name = "Difflugia"; File = "difflugia.jpg"; Url = "https://en.wikipedia.org/wiki/Difflugia" },
    @{ Name = "Actinophrys"; File = "actinophrys.jpg"; Url = "https://en.wikipedia.org/wiki/Actinophrys" },
    @{ Name = "Euglena"; File = "euglena.jpg"; Url = "https://en.wikipedia.org/wiki/Euglena" },
    @{ Name = "Volvox"; File = "volvox.jpg"; Url = "https://en.wikipedia.org/wiki/Volvox" },
    @{ Name = "Phacus"; File = "phacus.jpg"; Url = "https://en.wikipedia.org/wiki/Phacus" },
    @{ Name = "Peranema"; File = "peranema.jpg"; Url = "https://en.wikipedia.org/wiki/Peranema" },
    @{ Name = "Rotifer"; File = "rotifer.jpg"; Url = "https://en.wikipedia.org/wiki/Rotifer" },
    @{ Name = "Hydra"; File = "hydra.jpg"; Url = "https://en.wikipedia.org/wiki/Hydra_(genus)" },
    @{ Name = "Daphnia"; File = "daphnia.jpg"; Url = "https://en.wikipedia.org/wiki/Daphnia" },
    @{ Name = "Cyclops"; File = "cyclops.jpg"; Url = "https://en.wikipedia.org/wiki/Cyclops_(genus)" },
    @{ Name = "Ostracod"; File = "ostracod.jpg"; Url = "https://en.wikipedia.org/wiki/Ostracoda" },
    @{ Name = "Tardigrade"; File = "tardigrade.jpg"; Url = "https://en.wikipedia.org/wiki/Tardigrade" },
    @{ Name = "Nematode"; File = "nematoda.jpg"; Url = "https://en.wikipedia.org/wiki/Nematode" },
    @{ Name = "Gastrotrich"; File = "gastrotrich.jpg"; Url = "https://en.wikipedia.org/wiki/Gastrotricha" },
    @{ Name = "Planarian"; File = "planarian.jpg"; Url = "https://en.wikipedia.org/wiki/Planarian" },
    @{ Name = "Bryozoan"; File = "bryozoan.jpg"; Url = "https://en.wikipedia.org/wiki/Bryozoa" },
    @{ Name = "Spirogyra"; File = "spirogyra.jpg"; Url = "https://en.wikipedia.org/wiki/Spirogyra" },
    @{ Name = "Closterium"; File = "closterium.jpg"; Url = "https://en.wikipedia.org/wiki/Closterium" },
    @{ Name = "Cosmarium"; File = "cosmarium.jpg"; Url = "https://en.wikipedia.org/wiki/Cosmarium" },
    @{ Name = "Pediastrum"; File = "pediastrum.jpg"; Url = "https://en.wikipedia.org/wiki/Pediastrum" },
    @{ Name = "Scenedesmus"; File = "scenedesmus.jpg"; Url = "https://en.wikipedia.org/wiki/Scenedesmus" },
    @{ Name = "DiatomCentric"; File = "diatom_centric.jpg"; Url = "https://en.wikipedia.org/wiki/Diatom" },
    @{ Name = "DiatomPennate"; File = "diatom_pennate.jpg"; Url = "https://en.wikipedia.org/wiki/Diatom" },
    @{ Name = "Cyanobacteria"; File = "cyanobacteria.jpg"; Url = "https://en.wikipedia.org/wiki/Cyanobacteria" },
    @{ Name = "Cladophora"; File = "cladophora.jpg"; Url = "https://en.wikipedia.org/wiki/Cladophora" },
    @{ Name = "Yeast"; File = "yeast.jpg"; Url = "https://en.wikipedia.org/wiki/Yeast" },
    @{ Name = "Actinomycetes"; File = "actinomycetes.jpg"; Url = "https://en.wikipedia.org/wiki/Actinomycetota" },
    @{ Name = "Zoogloea"; File = "zoogloea.jpg"; Url = "https://en.wikipedia.org/wiki/Zoogloea" },
    @{ Name = "Detritus"; File = "detritus.jpg"; Url = "https://en.wikipedia.org/wiki/Detritus" }
)

# Wikimedia requires a descriptive User-Agent header or it blocks requests with 403 Forbidden
$headers = @{ 
    "User-Agent" = "MicroscopyDatabaseApp/1.0 (Educational WPF App; contact@local.dev)" 
}

foreach ($item in $organisms) {
    $pageTitle = $item.Url.Split('/')[-1]
    $destPath = Join-Path $outputDir $item.File
    
    Write-Host "Processing: $($item.Name) ($pageTitle)..." -NoNewline
    
    try {
        # Query Wikipedia REST API v1 for page summary (which contains the primary page image thumbnail)
        $apiUrl = "https://en.wikipedia.org/api/rest_v1/page/summary/$pageTitle"
        $response = Invoke-RestMethod -Uri $apiUrl -Headers $headers -Method Get -ErrorAction Stop

        if ($response.thumbnail -and $response.thumbnail.source) {
            $imageUrl = $response.thumbnail.source
            
            # Download the image file
            Invoke-WebRequest -Uri $imageUrl -OutFile $destPath -Headers $headers -ErrorAction Stop
            Write-Host " [SUCCESS]" -ForegroundColor Green
        } else {
            Write-Host " [SKIPPED: No thumbnail found]" -ForegroundColor Yellow
        }
    }
    catch {
        Write-Host " [FAILED: $_]" -ForegroundColor Red
    }
    
    # Polite delay to avoid hammering the Wikipedia API rate limit
    Start-Sleep -Milliseconds 300
}

Write-Host "`nAll tasks completed! Check the '$outputDir' folder." -ForegroundColor Cyan