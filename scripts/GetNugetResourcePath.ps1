function Get-NugetResourcePath {
    param(
        [string]$assemblyName,
        [string]$version,
        [string]$framework
    )

    if(-not $framework) {
        $framework = "netstandard2.0"
    }

    $assemblyNameLowerCase = $assemblyName.ToLower()

    $assemblyPath = Join-Path $Env:USERPROFILE ".nuget/packages/${assemblyNameLowerCase}/${version}/lib/${framework}/${assemblyName}.dll"

    return $assemblyPath
}