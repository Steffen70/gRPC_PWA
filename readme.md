# Template: PWA Client with .NET Core gRPC Backend

## Prerequisites

-   **PowerShell Version**: Ensure you have PowerShell version 7.4.1 installed to match the .NET 8.0 target version.
-   **.NET Version**: This project targets .NET 8.0.

## Installation

1. **Install .NET 8.0**:

    - Download and install .NET 8.0 SDK from the official website.
    - Make sure it is added to your system's PATH.

2. **Install Node.js**:

    - Install Node version v20.11.1 via chocolatey `choco install nodejs --version=20.11.1`.
    - Make sure it is added to your system's PATH.

3. **Install PowerShell 7.4.1**:

    - Download and install PowerShell 7.4.1
    - Make sure it is added to your system's PATH.
    - E.g. install with Chocolaty `choco install powershell-core --version=7.4.1`

4. **NuGet Cache Location**:

    - Confirm that your NuGet cache is located at the default location: `%USERPROFILE%\.nuget\packages\`.

## Guide on how to Run the Project

### Admin User

When the webserver is started, a default Admin user is seeded into the database. The login password is shown in the console.
Keep in mind: the database needs to be deleted if you want to reseed the database.

### Generate dotnet solution

If you don't need a solution file and just build the project, you can skip this step.

To generate the dotnet solution, run the following command in the root directory of the project:

```bash
.\scripts\MakeSolution.ps1 -directoryPath ".\server_side"
```

This script will generate a dotnet solution in the specified directory and add all projects to it.

## Versioning in the Project

### Process Overview

When the solution is cleaned or rebuilt, a PowerShell script automatically updates all project versions. The version is defined in `.\versionconfig.json`, with the following structure:

```json
{
    "version": {
        "major": 1,
        "minor": 2,
        "patch": 2
    }
}
```
