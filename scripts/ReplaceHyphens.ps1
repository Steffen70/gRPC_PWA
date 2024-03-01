Get-ChildItem -Filter *.svg -Recurse | Rename-Item -NewName {
    $_.Name -replace '-', '_' 
}