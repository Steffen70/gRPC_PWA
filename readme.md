# Template: PWA Client with .NET Core gRPC Backend

## Installation Instructions

### Prerequisites

You can run the following commands to check the versions of the required tools:

```bash
pwsh -File "./scripts/CheckPrerequisites.ps1"
```

Make sure all tools are installed on your machine and are available in your PATH.

This project requires specific versions of .NET, Node.js, PowerShell, and the Protobuf compiler to be installed. Below are the instructions for setting up your environment on both Linux (specifically for Debian-based distributions such as Ubuntu) and Windows.

### Linux (Debian/Ubuntu)

Ensure your package list and installed packages are up to date:

```bash
sudo apt update && sudo apt upgrade -y
```

**Install .NET SDK 8.0:**

```bash
sudo apt-get install -y dotnet-sdk-8.0
```

**Install Node.js:**

```bash
sudo apt-get install -y nodejs
```

**Additional dependencies:**

```bash
sudo apt-get install -y wget apt-transport-https software-properties-common
```

**Configure the Microsoft package repository:**

```bash
source /etc/os-release
wget -q "https://packages.microsoft.com/config/ubuntu/$(lsb_release -rs)/packages-microsoft-prod.deb"
sudo dpkg -i packages-microsoft-prod.deb
rm packages-microsoft-prod.deb
sudo apt-get update
```

**Install PowerShell:**

```bash
sudo apt-get install -y powershell
```

For **Ubuntu 23.10**, you can install PowerShell using the snap package manager:

```bash
sudo snap install powershell --classic
```

**Install the Protobuf compiler:**

```bash
sudo apt install -y protobuf-compiler
```

This installs the `protoc` command, which is used to compile `.proto` files into language-specific code.

**Install the gRPC-Web plugin:**

The Protobuf gRPC-Web Plugin is required for generating gRPC-Web client code.

1. Download the Protobuf gRPC-Web Plugin version 1.5.0 for Linux:

   ```bash
   wget https://github.com/grpc/grpc-web/releases/download/1.5.0/protoc-gen-grpc-web-1.5.0-linux-x86_64 -O protoc-gen-grpc-web
   ```

2. Make the downloaded file executable:

   ```bash
   chmod +x protoc-gen-grpc-web
   ```

3. Move the file to a directory in your `PATH` to make it globally accessible:

   ```bash
   sudo mv protoc-gen-grpc-web /usr/local/bin/
   ```

After completing these steps, `protoc-gen-grpc-web` will be installed and available for use in generating gRPC-Web client code from `.proto` files.

### Windows

I recommend you to use [Chocolatey](https://chocolatey.org/install#individual), a package manager for Windows, for the installations.

To install the required tools on Windows, you will need to start the terminal as an administrator. 

**Install .NET SDK 8.0:**

- Download and install directly from the [official .NET website](https://dotnet.microsoft.com/download/dotnet/8.0).

**Install Node.js:**

```powershell
choco install nodejs
```

**Install PowerShell:**

```powershell
choco install powershell-core
```

**Install the Protobuf compiler:**

```powershell
choco install protoc
```

**Install the gRPC-Web plugin:**

- The plugin can be downloaded from the [official GitHub repository](https://github.com/grpc/grpc-web/releases). After downloading, rename the file to `protoc-gen-grpc-web.exe` and ensure it's accessible in your PATH.

## Guide on how to Run the Project

### Admin User

When the webserver is started, a default Admin user is seeded into the database. The login password is shown in the console.
Keep in mind: the database needs to be deleted if you want to reseed the database.

### Generate dotnet solution

If you don't need a solution file and just build the project, you can skip this step.

To generate the dotnet solution, run the following command in the root directory of the project:

```bash
pwsh -File "./scripts/MakeSolution.ps1" -directoryPath "./server_side"
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

When the solution is cleaned or rebuilt, a PowerShell script automatically updates all project versions. The version is defined in `./versionconfig.json`, with the following structure:

```json
{
    "version": {
        "major": 1,
        "minor": 2,
        "patch": 2
    }
}
```
