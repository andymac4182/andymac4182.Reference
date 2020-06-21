#!/usr/bin/env pwsh
$configuration = "Release"
$BuildId = "0"
$IsBuildEnv = $false
$CommitSha = 'LOCAL'
$Env:DOTNET_CLI_TELEMETRY_OPTOUT = 'true'

if (Test-Path Env:BUILD_BUILDID)
{
    $BuildId = $Env:BUILD_BUILDID
    $IsBuildEnv = $true
}

if (Test-Path Env:GITHUB_RUN_NUMBER)
{
    $BuildId = $Env:GITHUB_RUN_NUMBER
    $IsBuildEnv = $true
}

Remove-Item -Recurse -Force artifacts -ErrorAction SilentlyContinue
Remove-Item -Recurse -Force tools -ErrorAction SilentlyContinue
Get-ChildItem .\ -include bin,obj -Recurse | ForEach-Object ($_) { Remove-Item $_.fullname -Force -Recurse }

Set-Location src

dotnet tool restore
dotnet dotnet-gitversion /nofetch

$gitVersion = (dotnet dotnet-gitversion /nofetch) | Out-String | ConvertFrom-Json -AsHashtable

if($IsBuildEnv)
{
    $CommitSha = $gitVersion["ShortSha"]
}

$Version = "$($gitVersion["SemVer"]).$BuildId+$CommitSha"
$Env:Version = $Version

Write-Output "Build version: $Version"
Write-Output "##vso[build.updatebuildnumber]$Version"

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

if (Test-Path "artifacts/publish")
{
    $dir = Get-ChildItem "artifacts/publish" | ?{$_.PSISContainer}

    foreach ($d in $dir){
        dotnet dotnet-octo pack --id $d.Name --basePath $d.FullName --version $Env:Version --outFolder="artifacts/octo-nupkg"

        if($IsBuildEnv)
        {
            #dotnet dotnet-octo push --package "artifacts/octo-nupkg/$( $d.Name ).$( $Env:Version ).nupkg"
        }
    }
}
