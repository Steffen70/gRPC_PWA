param (
    [string]$ProtoFilesPath,
    [string]$OutputPath
)

$directoryPath = (Resolve-Path -Relative $PSScriptRoot)

if(-not $ProtoFilesPath) {
    $ProtoFilesPath = Resolve-Path (Join-Path $directoryPath "../protos")
}

if(-not $OutputPath) {
    $OutputPath = Join-Path $directoryPath "../client_side/src/generated"
}

if (-not (Test-Path -Path $OutputPath -PathType Container)) {
    New-Item -ItemType Directory -Path $OutputPath
}

$OutputPath = Resolve-Path $OutputPath

$protoFiles = Get-ChildItem -Path $ProtoFilesPath -Filter *.proto

foreach ($protoFile in $protoFiles) {
    $protocCommand = "protoc --proto_path=$ProtoFilesPath --js_out=import_style=commonjs:$OutputPath --grpc-web_out=import_style=commonjs,mode=grpcwebtext:$OutputPath $($protoFile.FullName)"

    Invoke-Expression $protocCommand
}

Write-Host "gRPC-Web client code generation completed."
