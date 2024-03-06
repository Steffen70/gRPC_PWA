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

    $userHome = if ($PSVersionTable.Platform -eq "Win32NT") { $Env:USERPROFILE } else { $Env:HOME }

    $assemblyPath = Join-Path $userHome ".nuget/packages/${assemblyNameLowerCase}/${version}/lib/${framework}/${assemblyName}.dll"

    return $assemblyPath
}