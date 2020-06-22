#!/usr/bin/env pwsh
$configuration = "Release"
$Env:DOTNET_CLI_TELEMETRY_OPTOUT = 'true'

if (!(Test-Path Env:version))
{
    $Env:version = "0.0.0.0+LOCAL"
}

Remove-Item -Recurse -Force artifacts -ErrorAction SilentlyContinue
Remove-Item -Recurse -Force tools -ErrorAction SilentlyContinue
Get-ChildItem .\ -include bin,obj -Recurse | ForEach-Object ($_) { Remove-Item $_.fullname -Force -Recurse }

Set-Location src

dotnet tool restore

$numOfCsprojFiles = Get-ChildItem -Include *.sln -Recurse | Measure-Object

if ($numOfCsprojFiles.Count -gt 0)
{
    dotnet restore --locked-mode
    dotnet build --configuration $configuration --no-restore
    dotnet test --logger "trx;LogFileName=../../../artifacts/test-results.trx"
    dotnet publish --configuration $configuration --no-build
    dotnet pack --configuration $configuration --no-build --output "../artifacts/nupkgs"
}

$packageJsonFiles = Get-ChildItem -Include package.json -Recurse -Depth 2

foreach ($packageJsonFile in $packageJsonFiles)
{
    $packageJsonDir = Split-Path $packageJsonFile
    Write-Host "Found npm project at $packageJsonDir"
    Set-Location $packageJsonDir
    npm install
    npm run build
    Set-Location ..
}

Set-Location ..
