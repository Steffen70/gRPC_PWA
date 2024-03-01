# Template: PWA Client with .NET Core gRPC Backend

## Prerequisites

You can run the following commands to check the versions of the required tools:

```bash
pwsh -File ".\scripts\CheckPrerequisites.ps1"
```

To install the required tools, follow the instructions below:
(You need to start the terminal as an administrator to install the tools via chocolatey.)

-   Install **.NET** version **8.0** via chocolatey `choco install dotnet-sdk --version=8.0`.
-   Install **Node** version **v20.11.1** via chocolatey `choco install nodejs --version=20.11.1`.
-   Install **PowerShell** version **7.4.1** via Chocolaty `choco install powershell-core --version=7.4.1`
-   Install **Protobuf compiler** version **25.2** via chocolatey `choco install protoc --version=25.2`.
-   Download the **Protobuf gRPC-Web Plugin** version **1.5.0** from the [official GitHub repository](https://github.com/grpc/grpc-web/releases) and rename the file to `protoc-gen-grpc-web.exe`.

Make sure all these tools are installed on your machine and are available in your PATH.

**Note**:
Confirm that your NuGet cache is located at the default location: `%USERPROFILE%\.nuget\packages\`.

## Guide on how to Run the Project

### Admin User

When the webserver is started, a default Admin user is seeded into the database. The login password is shown in the console.
Keep in mind: the database needs to be deleted if you want to reseed the database.

### Generate dotnet solution

If you don't need a solution file and just build the project, you can skip this step.

To generate the dotnet solution, run the following command in the root directory of the project:

```bash
pwsh -File ".\scripts\MakeSolution.ps1" -directoryPath ".\server_side"
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
