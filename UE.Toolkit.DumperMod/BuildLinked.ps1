# Set Working Directory
Split-Path $MyInvocation.MyCommand.Path | Push-Location
[Environment]::CurrentDirectory = $PWD

Remove-Item "$env:RELOADEDIIMODS/UE.Toolkit.DumperMod/*" -Force -Recurse
dotnet publish "./UE.Toolkit.DumperMod.csproj" -c Release -o "$env:RELOADEDIIMODS/UE.Toolkit.DumperMod" /p:OutputPath="./bin/Release" /p:ReloadedILLink="true"

# Restore Working Directory
Pop-Location