# Template: PWA Client with .NET Core gRPC Backend

## Prerequisites

You can run the following commands to check the versions of the required tools:

```bash
pwsh -File ".\scripts\CheckPrerequisites.ps1"
```

To install the required tools, follow the instructions below:

(You need to start the terminal as administrator to install the tools via chocolatey.)

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

## Protocol Buffers and gRPC

### Generate gRPC files for the .NET Core Backend

The gRPC files are referenced in the `Common.csproj` file. The Protobuf files will automatically be compiled to C# files when the project is built.

### Generate Protobuf and gRPC-Web files

I've added a batch file to the `client_side` directory that will generate the Protobuf and gRPC-Web files for the client.

## Image Resources

I've incorporated an additional script into the build process.
This script dynamically updates the `Seventy.Common.ResourceAccessor.ResourceName` enum with the names of all images.

To retrieve SVG images programmatically, utilize the `GetSvg` method available on the enum, as shown below:

```csharp
var svg = ResourceAccessor.ResourceName.image_name.GetSvg();
```

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
