# UE Toolkit
General modding toolkit for Unreal Engine games using Reloaded II.

Supported: UE5/Clair Obscur

## Features
- `UObject` and `UDataTable` logging.
- Simplified `UObject` and `UDataTable` editing at runtime.
- Access to `FMemory` functions (currently just `Malloc` and `Free`).
- Mod for dumping game types to C# types.
- Methods for creating `FString` and `FText`.

## UE Toolkit: Dumper
1. Launch game and get to the main menu or later.
2. In Reloaded, right-click `UE Toolkit: Dumper` and select `Configure`.
3. In the config window, fill in any settings you want, then click `Save` to start dumping objects.
4. Generated files will be located in the mod folder: right-click `UE Toolkit: Dumper` and select `Open Folder`.

Some Unreal types may not be generated and need to be supplied. `UE.Toolkit.Core` includes any types
that were missing in my testing. Add it to your project using **NuGet** and add `UE.Toolkit.Core.Types.Unreal;`
in the `File Usings` config before dumping.

## Special Thanks
- UE4SS team, for object dumping reference.
- Rirurin, for object dumping reference.