#!/usr/bin/env pwsh
$configuration = "Release"
$Env:Version = "1.0.1+vvvvvv"

Remove-Item -Recurse -Force artifacts -ErrorAction SilentlyContinue
Remove-Item -Recurse -Force tools -ErrorAction SilentlyContinue
Get-ChildItem .\ -include bin,obj -Recurse | foreach ($_) { remove-item $_.fullname -Force -Recurse }

Set-Location src

dotnet restore --locked-mode
dotnet tool restore
dotnet build --configuration $configuration --no-restore
dotnet test --logger "trx;LogFileName=../../../artifacts/test-results.trx"
dotnet publish --configuration $configuration --no-build
dotnet pack --configuration $configuration --no-build --output "../artifacts/nupkgs"

Set-Location ..

$dir = Get-ChildItem "artifacts/publish" | ?{$_.PSISContainer}

foreach ($d in $dir){
    dotnet dotnet-octo pack --id $d.Name --basePath $d.FullName --version $Env:Version --outFolder="artifacts/octo-nupkg"
}
