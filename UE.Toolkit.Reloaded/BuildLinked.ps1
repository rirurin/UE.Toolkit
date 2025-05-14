# Set Working Directory
Split-Path $MyInvocation.MyCommand.Path | Push-Location
[Environment]::CurrentDirectory = $PWD

Remove-Item "$env:RELOADEDIIMODS/UE.Toolkit.Reloaded/*" -Force -Recurse
dotnet publish "./UE.Toolkit.Reloaded.csproj" -c Release -o "$env:RELOADEDIIMODS/UE.Toolkit.Reloaded" /p:OutputPath="./bin/Release" /p:ReloadedILLink="true"

# Restore Working Directory
Pop-Location