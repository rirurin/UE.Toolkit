# UE Toolkit
General modding toolkit for Unreal Engine games using Reloaded II.

__Supported__
- UE 5.4.4 (Clair Obscur)
- UE 4.27.2 (P3R)

## Features
- `UObject` and `UDataTable` logging.
- Simplified `UObject` and `UDataTable` editing at runtime.
- Access to `FMemory` functions (currently just `Malloc` and `Free`).
- Mod for dumping game types to C# types.
- Methods for creating `FString` and `FText`.
- Edit object data (numbers and text) with just XML text files. 

### Installation
#### Setup Reloaded and Add Your Game
I recommend the P3R guides, since they're the newest and cover GamePass. __You only really need to add your game to Reloaded, you can ignore anything past that.__

[__Beginner's Guide to Modding Persona 3 Reload__](https://gamebanana.com/tuts/17156)

[__Beginner's Guide to Modding P3R (Xbox / Game Pass)__](https://gamebanana.com/tuts/17171)

#### Installing UE Toolkit
1. Go to [Releases](https://github.com/RyoTune/UE.Toolkit/releases) and download the latest version of `UE.Toolkit.Reloaded.7z`
2. Drag and drop the `7z` file into Reloaded to install.

#### Installing the UE Toolkit Extension Mod
Install the extension mod for your game to be able to use all features (Object XML Editing).
- __Clair Obscur__ - https://github.com/RyoTune/E33.UEToolkit/releases
- __Persona 3 Reload__ - https://github.com/RyoTune/P3R.UEToolkit/releases

## Editing Objects with XML
In supported games, you're able to edit any object with just a simple text file (XML). No Unreal Engine, no file unpacking/repacking/cooking, and no hex 🤮 editing. Just _Notepad++_ and a dream ✨! ~~Or Notepad if you hate yourself...~~

On top of that, your edits __only__ edit what you're editing! Or put non-stupidly, __mod merging__ is built in 🎉! Mods only conflict if they edit the exact same thing, like the price of the same item for example.

#### Install UE Toolkit

### Requirements
- UE Toolkit (Main Mod)
- The UE Toolkit extension mod for your game.
- Enable "View File Extensions" on Windows

### Creating a Mod (Reloaded)
This will be a bit abridged since I expect most people using this mod are experienced with Reloaded and my other mods.

#### Steps
1. Create a Reloaded mod with a __Mod Dependency__ on the extension mod. You can add the main mod too if you like, but it's not required.
2. In your mod's folder, create the following folders: `MOD_FOLDER/ue-toolkit/objects`
3. Inside the `objects` folder, you will place any __Object XML__ files you make. They can be in sub-folders for organization.

### Creating a Mod (Unreal's `~mods` Folder)
Not recommended or really supported, but some people don't like change and/or hate Reloaded. Reloaded is great though 💖!

*Only supported on Clair Obscur's extension mod.*
#### Steps
1. In the game's `~mods` folder, create the following folders: `../~mods/ue-toolkit/objects`
2. Inside the `objects` folder, create a folder for *your* specific mod: `../~mods/ue-toolkit/objects/My Super Cool Mod`
3. Inside your mod's folder, you will place any __Object XML__ files you make. They can be in sub-folders for organization. 

### Creating an Object XML
Now for actually editing an object. You only need two (2) pieces of information to get started: the object's __Name__ and its __Class__ (and __RowStruct__ for _DataTables_).

You can easily get both using [__FModel__](https://github.com/4sval/FModel/releases).

#### Steps
1. Inside your chosen folder from the previous steps, create a new __text file__.
2. Name the file the same as the object's __Name__.
3. Change the file extension from `.txt` to `.obj.xml`. Your file's name should be similar to: `DT_jRPG_CharacterDefinitions.obj.xml`
4. Open your file in a text editor.

### Writing an Object XML
Generally, your XML will match the "shape" of the object, starting with its __Class__. I highly recommend looking at the object in __FModel__ or going to the extension mod's source if you can read code.

The best I can give is a general example, since it'll look very different depending on what you're editing.

#### The Root Element
The first element in your XML, aka the __Root Element__, will be the object's __Class__. In this example, taken from _Clair Obscur_'s `DL_Goblu_00Entry.uasset`, the __Class__ is a `DataLayerAsset`.

```xml
<DataLayerAsset>

</DataLayerAsset>
```

#### Editing Properties
A `DataLayerAsset` has two properties: `DataLayerType` and `DebugColor`.  We'll first edit `DataLayerType`.

```xml
<DataLayerAsset>
	<DataLayerType value="1"/> 
	OR
	<DataLayerType>1</DataLayerType>
</DataLayerAsset>
```

##### Steps
1. Add a new element with the same name as the property, in this case `DataLayerType`.
2. Add a `value` attribute to the element, with the value you want to set it to.
3. OR between the open and closing tags.

#### Editing Properties of Properties
We started with `DataLayerType` since it's value is directly in `DataLayerAsset`, but what if a property has its own properties? For example, `DebugColor` has RGB properties. 

Well, it's not too different than what we already have with the __Root Element__.

```xml
<DataLayerAsset>
	<DataLayerType value="1"/>
	<DebugColor>
		<R value="255"/>
		<G value="255"/>
		<B value="255"/>
	</DebugColor>
</DataLayerAsset>
```
##### Steps
1. Same as before, add a new element with the name of the property __but__ with open and closing tags.
2. Inside this element, add new elements with the names of the properties (like `DataLayerType` earlier.)

If a sub-property has _its_ own property, then just repeat the same process as needed.

#### Editing Items in a List (Arrays)
For editing lists, arrays, or DataTables a bit later, the process a slightly different since we need to set _which_ item we want to edit. For that, we use a special `Item` element.

```xml
<ArmorItemListTable>
	<Data>
		<Item id="2">
		    <EquipID value="100"/>
		</Item>
	</Data>
</ArmorItemListTable>
```

In this example, from _Persona 3 Reload_'s `DatArmorItemListTable` (not a DataTable), `ArmorItemListTable` is the __Class__, which has a `Data` property that's a list of of armor items.

First, we add an `Item` element with an `id` attribute. The ID is which item in the list we want to edit. The first item would have an ID of `1`, the second item `2`, third `3`, and so on.

Inside the `Item` element, we're back to what's already been covered. In this example, items have an `EquipID` property and we're setting the `EquipID` of the __2nd__ item to `100`.

#### Editing Items in a DataTable
Finally,  _DataTables_! There's only two notable differences: the `row-struct` attribute and `id`s are now row names. This example is from _Clair Obscur_'s `DT_jRPG_CharacterDefinitions.uasset`.

```xml
<DataTable row-struct="S_jRPG_CharacterDefinition">
  <Item id="Lune">
      <CharacterDisplayName value="TEST1"/>
  </Item>
  <Item id="Maelle">
      <CharacterDisplayName value="TEST2"/>
  </Item>
  <Item id="Sciel">
      <CharacterDisplayName value="TEST3"/>
  </Item>
</DataTable>
```

A DataTable's __Class__ is... `DataTable`! Who could've guessed? As such, the __Root Element__ in our XML has to be `DataTable`. 

But, we also need to specify the __Class (Struct)__ for the __Items (Rows)__ in the DataTable so we can edit them. You do that by adding a `row-struct` attribute with the DataTable's __RowStruct__ name, `S_jRPG_CharacterDefinition` in this example.

For our `Item` element's ID, instead of a number we use the __Row's name__ that we want to edit. Inside the `Item` element, it's no different than lists from earlier.

That covers everything, congrats on finishing this 🎉🎉🎉!

#### Misc. Stuff (Enums, Type Hints)
##### Enums
For enum properties (stuff like `EDataLayerType::Runtime`, where it starts with an `E`), you can use name values if you know them. You can find them in the extension mod's source or in __FModel__.

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
