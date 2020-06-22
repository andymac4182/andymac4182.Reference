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

dotnet tool restore
dotnet dotnet-gitversion /nofetch

$gitVersion = (dotnet dotnet-gitversion /nofetch) | Out-String | ConvertFrom-Json -AsHashtable

if($IsBuildEnv)
{
    $CommitSha = $gitVersion["ShortSha"]
}

$Version = "$($gitVersion["SemVer"]).$BuildId+$CommitSha"
$Env:version = $Version

Write-Output "Build version: $Version"
Write-Output "##vso[build.updatebuildnumber]$Version"

$DockerVersion = $Version -replace "\+","-"

docker build -f Dockerfile.build -t "app-build:$DockerVersion" .
docker run --name buildoutput -d "app-build:$DockerVersion"
docker cp buildoutput:app/artifacts ./artifacts
docker stop buildoutput
docker rm buildoutput

$dockerFiles = Get-ChildItem ./src/* -Include Dockerfile -Recurse -Depth 2
foreach ($dockerFile in $dockerFiles)
{
    $folder = (Get-Item $dockerFile).Directory
    $applicationName = $folder.Name.ToLower()
    Write-Host "Running docker build for $applicationName. The Dockerfile is located at $dockerFile"
    docker build --build-arg version --file $dockerFile -t "$($applicationName):$DockerVersion" .
}
