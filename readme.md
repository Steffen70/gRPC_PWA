# Template: gRPC-Web Client with .NET Core Backend

## Installation Instructions

### Prerequisites

You can run the following commands to check the versions of the required tools:

```bash
pwsh -File "./scripts/CheckPrerequisites.ps1"
```

Make sure all tools are installed on your machine and are available in your PATH.

This project requires specific versions of .NET, Node.js, PowerShell, and the Protobuf compiler to be installed. Below are the instructions for setting up your environment on both Linux (specifically for Debian-based distributions such as Ubuntu) and Windows.

#### Trust the development certificate

I've generated a development certificate for the web server, because gRPC-Web requires HTTPS. And dotnet dev-certs --trust command is not available on Linux.
Trust the certificate on your host windows and WSL2 using the following commands:

**Linux:**

```bash
sudo cp cert/localhost.crt /usr/local/share/ca-certificates/localhost.crt
sudo update-ca-certificates
```

WSL2 is not required, just trust the certificate on your host machine. And ignore the linux instructions.

**Windows:**

```powershell
Import-Certificate -FilePath .\cert\localhost.crt -CertStoreLocation Cert:\LocalMachine\Root
```

You may need to run PowerShell as an administrator for the command to succeed. After importing the certificate, you might need to restart your browser to ensure the changes take effect.

**Note**: The development certificate is only for development purposes. It is not recommended to use it in a production environment.
Replace the certificate with a valid one if you are deploying the application to a production environment.

### Linux Ubuntu

**Configure the Microsoft package repository:**

```bash
source /etc/os-release
wget -q "https://packages.microsoft.com/config/ubuntu/$(lsb_release -rs)/packages-microsoft-prod.deb"
sudo dpkg -i packages-microsoft-prod.deb
rm packages-microsoft-prod.deb
sudo apt-get update
```

**Install the required tools:**

```bash
sudo apt update && sudo apt upgrade -y

sudo apt-get install -y dotnet-sdk-8.0

sudo apt-get install -y nodejs

sudo apt-get install -y wget apt-transport-https software-properties-common unzip

sudo apt-get install -y powershell

sudo apt-get install -y protobuf-compiler
```

**Install the Javascript plugin for Protobuf:**

```bash
wget https://github.com/protocolbuffers/protobuf-javascript/releases/download/v3.21.2/protobuf-javascript-3.21.2-linux-x86_64.zip -O protobuf-javascript.zip


unzip protobuf-javascript.zip bin/protoc-gen-js

chmod +x bin/protoc-gen-js

sudo mv bin/protoc-gen-js /usr/local/bin/

rm -f protobuf-javascript.zip
rm -r -f bin
```

**Install the gRPC-Web plugin:**

The Protobuf gRPC-Web Plugin is required for generating gRPC-Web client code.

```bash
wget https://github.com/grpc/grpc-web/releases/download/1.5.0/protoc-gen-grpc-web-1.5.0-linux-x86_64 -O protoc-gen-grpc-web

chmod +x protoc-gen-grpc-web

sudo mv protoc-gen-grpc-web /usr/local/bin/
```

### Windows

I recommend you to use [Chocolatey](https://chocolatey.org/install#individual), a package manager for Windows, for the installations.

To install the required tools on Windows, you will need to start the terminal as an administrator.

**Install .NET SDK 8.0:**

-   Download and install directly from the [official .NET website](https://dotnet.microsoft.com/download/dotnet/8.0).

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

**Install the Javascript plugin:**

-   The plugin can be downloaded from the [official GitHub repository](https://github.com/protocolbuffers/protobuf-javascript/releases). After downloading, rename the file to `protoc-gen-js.exe` and ensure it's accessible in your PATH.

**Install the gRPC-Web plugin:**

-   The plugin can be downloaded from the [official GitHub repository](https://github.com/grpc/grpc-web/releases). After downloading, rename the file to `protoc-gen-grpc-web.exe` and ensure it's accessible in your PATH.

### Common Steps

**Note**: The following steps are common for both Linux and Windows.

Run the following command in the 'client_side' directory to **generate the gRPC-Web client code**:

You need to have yarn installed to run the following command. If you don't have yarn installed, you can install it using the following command:

```bash
npm install --global yarn
```

```bash
yarn generate
```

You can start the development server using the following commands:

**Server-Side:**

```bash
cd ./server_side/WebService

dotnet clean

dotnet run
```

**Client-Side:**

```bash
cd ./client_side

yarn install

yarn start
```

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

## Versioning in the Project

### Process Overview

When the solution is cleaned or rebuilt, a PowerShell script automatically updates all project versions. The version is defined in './versionconfig.json', with the following structure:

```json
{
    "version": {
        "major": 1,
        "minor": 2,
        "patch": 2
    }
}
```
