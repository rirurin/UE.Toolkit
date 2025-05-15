# Set Working Directory
Split-Path $MyInvocation.MyCommand.Path | Push-Location
[Environment]::CurrentDirectory = $PWD

Remove-Item "$env:RELOADEDIIMODS/UE.Toolkit.TestMod/*" -Force -Recurse
dotnet publish "./UE.Toolkit.TestMod.csproj" -c Release -o "$env:RELOADEDIIMODS/UE.Toolkit.TestMod" /p:OutputPath="./bin/Release" /p:ReloadedILLink="true"

# Restore Working Directory
Pop-Location