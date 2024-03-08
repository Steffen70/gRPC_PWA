$toolVersions = @{}

# PowerShell Core version
$pwshVersion = $PSVersionTable.PSVersion.ToString()
$toolVersions["pwsh"] = $pwshVersion

function Get-CommandVersion {
    param (
        [string]$command,
        [string]$arguments = "--version",
        [string]$successPattern = ""
    )
    try {
        # Split the command and its arguments
        $output = & $command $arguments
        if ($successPattern -ne "") {
            if ($output -match $successPattern) {
                $output = $matches[1]
            }
        }
        $toolVersions[$command] = $output
    } catch {
        $toolVersions[$command] = "Not installed"
    }
}

function Get-CommandInPath {
    param (
        [string]$commandName
    )

    $output = $null
    if ($PSVersionTable.Platform -eq "Win32NT") {
        $null = where.exe $commandName 2>&1

        $output = if ($LASTEXITCODE -eq 0) { "/" } else { "" }
    } else {
        $output = whereis $commandName
    }

    if ($output -match "\/") {
        return "Installed"
    } else {
        return "Not installed"
    }
}


# .NET version
Get-CommandVersion -command "dotnet"

# Node.js version
Get-CommandVersion -command "node"

# Protobuf Compiler version
Get-CommandVersion -command "protoc" -successPattern "libprotoc (\d+\.\d+)"

# Protobuf JavaScript has no CLI to get its version, so we can check if it's installed
$toolVersions["protoc-gen-js"] = Get-CommandInPath -commandName "protoc-gen-js"

# Assuming you have a specific execution command for the Protobuf gRPC-Web Plugin
Get-CommandVersion -command "protoc-gen-grpc-web" -arguments "--version" -successPattern "(\d+\.\d+\.\d+)"

$toolVersions.GetEnumerator() | Sort-Object Name | ForEach-Object { 
    Write-Host "$($_.Name): $($_.Value)" 
}
