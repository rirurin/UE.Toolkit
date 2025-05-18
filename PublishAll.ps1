# Set Working Directory
Split-Path $MyInvocation.MyCommand.Path | Push-Location
[Environment]::CurrentDirectory = $PWD

./Publish.ps1 -ProjectPath "UE.Toolkit.Reloaded/UE.Toolkit.Reloaded.csproj" `
              -PackageName "UE.Toolkit.Reloaded" `
              -PublishOutputDir "Publish/ToUpload/Toolkit" `

./Publish.ps1 -ProjectPath "UE.Toolkit.DumperMod/UE.Toolkit.DumperMod.csproj" `
              -PackageName "UE.Toolkit.DumperMod" `
              -PublishOutputDir "Publish/ToUpload/Dumper" `

Pop-Location