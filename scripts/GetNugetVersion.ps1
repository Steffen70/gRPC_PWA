function Get-NugetVersion {
    param (
        $projectFile,
        [string[]]$assemblyNames
    )

    [xml]$projectXml = Get-Content "./$projectFile"

    $versions = @{}

    foreach ($assemblyName in $assemblyNames) {
        $version = $projectXml.Project.ItemGroup.PackageReference | Where-Object { $_.Include -eq $assemblyName } | Select-Object -ExpandProperty Version

        $versions.Add($assemblyName, $version)
    }

    return $versions
}