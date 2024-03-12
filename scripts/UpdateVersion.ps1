param(
    [string]$projectFile
)

# Path to the JSON file, assuming it's in the same directory as the script
$scriptPath = Split-Path -Parent -Path $MyInvocation.MyCommand.Definition

# Modify the path to point to the parent directory
$scriptPath = Split-Path -Parent -Path $scriptPath

$jsonFilePath = Join-Path $scriptPath "versionconfig.json"

# Load and parse the JSON file
$jsonContent = Get-Content $jsonFilePath -Raw | ConvertFrom-Json
$version = $jsonContent.version
$versionString = "$($version.major).$($version.minor).$($version.patch)"

# Read the content of the project file
$projectContent = Get-Content $projectFile -Raw

# Regex to find and replace the <Version> tag
$regex = "<Version>.*?</Version>"
$replacement = "<Version>$versionString</Version>"
$newProjectContent = [regex]::Replace($projectContent, $regex, $replacement)

# Trim trailing whitespace and newline characters
$newProjectContent = $newProjectContent.TrimEnd()

# Save the changes back to the project file
Set-Content -Path $projectFile -Value $newProjectContent

Write-Host "Project file '$projectFile' has been updated to version $versionString."