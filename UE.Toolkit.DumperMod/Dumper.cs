using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using UE.Toolkit.Core.Common;
using UE.Toolkit.Core.Types.Unreal;
using UE.Toolkit.Interfaces;

// ReSharper disable InconsistentNaming

namespace UE.Toolkit.DumperMod;

public unsafe class Dumper
{
    private record PropertyDefinition(string Name, int Size, int Offset, Func<string> PropTypeName) : ICsharpText
    {
        public string GetCsharpText()
        {
            return $"[FieldOffset(0x{Offset:X})] public {PropTypeName()} {SanitizeName(Name)}; // Size: 0x{Size:X}";
        }
    }

    private record UStructDefinition(string InternalName, string DisplayName, int Size, int Alignment, PropertyDefinition[] Properties, string? SuperInternalName) : ICsharpText
    {
        public string Name => SanitizeName(DisplayName);

        public string GetCsharpText()
        {
            var sb = new StringBuilder();

            if (Mod.Config.Mode == DumpFileMode.FilePerType)
            {
                AddHeader(sb);
            }
            
            sb.AppendLine($"[StructLayout(LayoutKind.Explicit, Pack = {Alignment}, Size = 0x{Size:X})]");
            sb.AppendLine($"public unsafe struct {SanitizeName(DisplayName)}\n{{");
            if (SuperInternalName != null)
            {
                if (_UStructDefinitions.TryGetValue(SuperInternalName, out var super))
                {
                    sb.AppendLine($"    [FieldOffset(0x0)] public {SanitizeName(super.DisplayName)} Super; // Size: 0x{super.Size:X}");
                }
                else
                {
                    Log.Warning($"Failed to get super: {SuperInternalName}");
                }
            }
            
            foreach (var prop in Properties)
            {
                sb.AppendLine($"    {prop.GetCsharpText()}");
            }
            
            sb.AppendLine("}");
            return sb.ToString();
        }
    }

    private record UEnumDefinition(string Name, string UnderlyingType, Dictionary<string, long> Entries) : ICsharpText
    {
        public string GetCsharpText()
        {
            var sb = new StringBuilder();
            
            if (Mod.Config.Mode == DumpFileMode.FilePerType)
            {
                AddHeader(sb);
            }
            
            sb.AppendLine($"public enum {SanitizeName(Name)} : {UnderlyingType}\n{{");
            foreach (var entry in Entries)
            {
                sb.AppendLine($"    {SanitizeEntryName(entry.Key)} = {entry.Value},");
            }

            sb.AppendLine("}");
            return sb.ToString();
        }

        private static string SanitizeEntryName(string name) => SanitizeName(name.Split("::").Last());
    }

    private delegate FText* UEnum_GetDisplayNameTextByIndex(UUserDefinedEnum* userEnum, FText* outName, int index);

    private readonly SHFunction<UEnum_GetDisplayNameTextByIndex> _GetDisplayNameTextByIndex =
        new("48 89 5C 24 ?? 55 56 57 48 83 EC 30 48 8B FA");
    
    private static readonly Dictionary<string, UStructDefinition> _UStructDefinitions = [];
    private readonly Dictionary<string, UEnumDefinition> _UEnumDefinitions = [];
    private readonly string _dumpDir;
    private readonly IUnrealObjects _uobjs;

    public Dumper(IUnrealObjects uobjs, string dumpDir)
    {
        _uobjs = uobjs;
        _dumpDir = dumpDir;
        
        if (Directory.Exists(dumpDir)) Directory.Delete(dumpDir, true);
        Directory.CreateDirectory(dumpDir);
    }

    public void DumpObjects()
    {
        Log.Information("Dumping objects...");

        var sw = new Stopwatch();
        
        sw.Start();
        RegisterObjects();

        StringBuilder? sb = null;
        if (Mod.Config.Mode == DumpFileMode.SingleFile)
        {
            sb = new();
            AddHeader(sb);
        }

        var numDumped = 0;
        foreach (var item in _UStructDefinitions)
        {
            if (Mod.Config.Mode == DumpFileMode.FilePerType)
            {
                var outputFile = Path.Join(_dumpDir, $"{item.Value.DisplayName}.cs");
                File.WriteAllText(outputFile, item.Value.GetCsharpText());
            }
            else
            {
                sb?.AppendLine(item.Value.GetCsharpText());
            }
            
            numDumped++;
        }
        
        foreach (var item in _UEnumDefinitions)
        {
            if (Mod.Config.Mode == DumpFileMode.FilePerType)
            {
                var outputFile = Path.Join(_dumpDir, $"{item.Key}");
                File.WriteAllText(outputFile, item.Value.GetCsharpText());
            }
            else
            {
                sb?.AppendLine(item.Value.GetCsharpText());
            }
            
            numDumped++;
        }

        sw.Stop();
        if (Mod.Config.Mode == DumpFileMode.SingleFile)
        {
            var singleFileOutput = string.IsNullOrEmpty(Mod.Config.SingleFileOutputName)
                ? Path.Join(_dumpDir, "Types.cs")
                : Path.Join(_dumpDir, $"{Mod.Config.SingleFileOutputName.Replace(".cs", string.Empty)}.cs");
            
            File.WriteAllText(singleFileOutput, sb!.ToString());
            Log.Information($"{numDumped} objects dumped in {sw.ElapsedMilliseconds}ms.\nOutput File: {singleFileOutput}");
        }
        else
        {
            Log.Information($"{numDumped} objects dumped in {sw.ElapsedMilliseconds}ms.\nOutput Folder: {_dumpDir}");
        }
    }

    private void RegisterObjects()
    {
        for (int i = 0; i < _uobjs.GUObjectArray->ObjObjects.NumElements; i++)
        {
            var objItem = _uobjs.GUObjectArray->ObjObjects.GetItem(i);
            var obj = objItem->Object;
            if (obj == null) continue;

            var moduleName = GetModuleNameForPackage(obj->GetOutermost());
            var fileBaseName = GetHeaderNameForObject(obj);

            if (obj->IsChildOf<UClass>())
            {
                var uclass = (UClass*)obj;
                if (uclass->IsChildOf<UInterface>())
                {
                    // TODO:
                    var interfaceName = ToolkitUtils.GetPrivateName(uclass);
                    _UStructDefinitions[interfaceName] = new(interfaceName, interfaceName, 0, 0, [], null);
                    Log.Debug($"Interface: {interfaceName}");
                }
                else
                {
                    GenerateObjectDefinition(uclass);
                }
            }
            else if (obj->IsChildOf<UScriptStruct>())
            {
                GenerateStructDefinition((UScriptStruct*)obj);
            }
            else if (obj->IsChildOf<UEnum>())
            {
                GenerateEnumDefinition((UEnum*)obj);
            }
        }
    }

    private static string GetModuleNameForPackage(UObjectBase* package)
    {
        if (package->OuterPrivate != null)
        {
            throw new("Encountered a package with an outer object set");
        }

        var packageName = package->NamePrivate.ToString();
        if (!packageName.StartsWith("/Script/"))
        {
            return string.Empty;
        }

        return packageName["/Script/".Length..];
    }
    
    private static string GetHeaderNameForObject(UObjectBase* obj)
    {
        string? headerName = null;
        UObjectBase* finalObj;
        
        if (obj->IsA<UClass>() || obj->IsA<UScriptStruct>())
        {
            headerName = obj->NamePrivate.ToString();
        }
        else if (obj->IsA<UEnum>())
        {
            headerName = obj->NamePrivate.ToString();
        }
        else
        {
            // TODO: UFunction stuff;
        }
        
        // TODO: Other stuff?
        return headerName;
    }

    private static void AddHeader(StringBuilder sb)
    {
        sb.AppendLine("/* Generated with UE Toolkit: Dumper (1.1.0)     */");
        sb.AppendLine("/* GitHub: https://github.com/RyoTune/UE.Toolkit */");
        sb.AppendLine("/* Author: RyoTune                               */");
        sb.AppendLine("/* Special thanks to UE4SS team and Rirurin      */");
        sb.AppendLine("/* whose code was used for reference.            */");
        sb.AppendLine();
        
        sb.AppendLine("using System.Runtime.InteropServices;");
            
        if (!string.IsNullOrEmpty(Mod.Config.FileUsings))
        {
            var usings = Mod.Config.FileUsings.Split(';',
                StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Replace("using ", string.Empty));
            foreach (var use in usings) sb.AppendLine($"using {use};");
        }

        sb.AppendLine();
        if (!string.IsNullOrEmpty(Mod.Config.FileNamespace))
        {
            sb.AppendLine($"namespace {Mod.Config.FileNamespace.TrimEnd(';').Replace("namespace ", string.Empty)};");
            sb.AppendLine();
        }
    }

    private void GenerateObjectDefinition(UClass* uclass)
    {
        var className = ToolkitUtils.GetPrivateName(uclass);
        var classNativeName = ToolkitUtils.GetNativeName(uclass);
        var size = uclass->Super.PropertiesSize;
        var alignment = uclass->Super.MinAlignment;
        
        // TODO: Flag stuff?

        var super = uclass->GetSuperClass();
        var superName = super != null ? ToolkitUtils.GetPrivateName(super) : null;
        //var superNativeName = super != null ? GetNativeClassName(super) : "UObjectBaseUtility";
        
        // TODO: Add super header data.
        
        // TODO: Generate delegates.

        var properties = ResolveProperties(uclass->Super.PropertyLink);

        // TODO: Generate functions.

        _UStructDefinitions[className] =
            new(className, classNativeName, size, alignment, properties, superName);
        
        Log.Debug($"UObject: {classNativeName}");
    }

    private void GenerateStructDefinition(UScriptStruct* scriptStruct)
    {
        var structName = ToolkitUtils.GetPrivateName(scriptStruct);
        var structNativeName = ToolkitUtils.GetNativeName(scriptStruct);
        var size = scriptStruct->Super.PropertiesSize;
        var alignment = scriptStruct->Super.MinAlignment;
        var super = scriptStruct->Super.SuperStruct;
        var superName = super != null ? ToolkitUtils.GetPrivateName(super) : null;
        //var superNativeName = super != null ? GetNativeStructName((UScriptStruct*)super) : null;
        
        var props = ResolveProperties(scriptStruct->Super.PropertyLink);

        _UStructDefinitions[structName] = new(structName, structNativeName, size, alignment, props, superName);
        Log.Debug($"UScriptStruct: {structNativeName}");
    }
    
    private void GenerateEnumDefinition(UEnum* uenum, string? knownType = null)
    {
        var name = ToolkitUtils.GetPrivateName(uenum);
        if (_UEnumDefinitions.ContainsKey(name) && knownType == null) return;
        
        var entries = new Dictionary<string, long>();
        
        var dispNames = new Dictionary<int, string>();
        if (((UObjectBase*)uenum)->IsChildOf<UUserDefinedEnum>())
        {
            var userEnum = (UUserDefinedEnum*)uenum;
            for (int i = 0; i < uenum->Names.ArrayNum; i++)
            {
                var dispNameFText =
                    _GetDisplayNameTextByIndex.Wrapper(userEnum, (FText*)Marshal.AllocHGlobal(sizeof(FText)), i);
                var dispName = _uobjs.FTextToString(dispNameFText);
                dispNames[i] = $"{name}::{dispName}";
            }
        }

        long bigEntryValue = 0;
        string bigEntryName = string.Empty;
        for (int i = 0; i < uenum->Names.ArrayNum; i++)
        {
            var entry = &uenum->Names.AllocatorInstance[i];

            if (!dispNames.TryGetValue(i, out var entryName))
            {
                entryName = entry->Key.ToString();
            }
            
            var entryValue = entry->Value;
            entries[entryName] = entryValue;
            
            if (bigEntryValue < entryValue)
            {
                bigEntryValue = entryValue;
                bigEntryName = entryName;
            }
        }

        var entryConstant = $"{name}::{name}_MAX";
        if (entries.TryGetValue(entryConstant, out var entryConstantValue) && entryConstantValue == bigEntryValue)
        {
            entries.Remove(entryConstant);
        }

        if (bigEntryValue == 256 && bigEntryName.EndsWith("_MAX")) entries.Remove(bigEntryName);

        var underlyingType = knownType ?? bigEntryValue switch
        {
            <= byte.MaxValue => "byte",
            <= short.MaxValue => "short",
            <= int.MaxValue => "int",
            <= long.MaxValue => "long",
        };

        bool hasNegativeValue = entries.Any(x => x.Value < 0);
        if (underlyingType == "byte" && hasNegativeValue) underlyingType = "sbyte";
        if (underlyingType != "byte" && !hasNegativeValue && !underlyingType.StartsWith('u')) underlyingType = $"u{underlyingType}";
        
        _UEnumDefinitions[name] = new(name, underlyingType, entries);
    }

    private PropertyDefinition[] ResolveProperties(FProperty* propLink)
    {
        var props = new List<PropertyDefinition>();
        for (; propLink != null; propLink = propLink->PropertyLinkNext)
        {
            var newProp = ResolveProperty(propLink);
            var numSameName = props.Count(x => x.Name.StartsWith(newProp.Name)); // Compare against sanitized name, since that's what props are using.
            
            // Handle multiple properties with the same name.
            if (numSameName == 0)
            {
                props.Add(newProp);
            }
            else
            {
                props.Add(newProp with { Name = $"{newProp.Name}_{numSameName + 1}" });
            }
        }

        return props.ToArray();
    }

    private PropertyDefinition ResolveProperty(FProperty* prop)
    {
        var (name, size, offset, _) = GetBaseInfo(prop);
        if (name == "bool" || name == "float") name += '_';
        
        var resProp = new PropertyDefinition(SanitizeName(name), size, offset, GetPropertyTypeNameLazy(prop));
        return resProp;
    }

    /// <summary>
    /// Gets the property type name, such as <c>byte</c> for <c>ByteProperty</c>.
    /// This is done lazily since we only have the internal names in <see cref="FProperty"/>
    /// when we want their C++/C# equivalent, which isn't known until object registrations finish.
    /// </summary>
    /// <param name="prop"></param>
    /// <returns>C++/C# property type name.</returns>
    private Func<string> GetPropertyTypeNameLazy(FProperty* prop)
    {
        var className = prop->Super.ClassPrivate->Name.ToString();
        switch (className)
        {
            case "BoolProperty":
                return () => "bool";
            case "ByteProperty":
                var byteProp = (FByteProperty*)prop;
                if (byteProp->Enum != null)
                {
                    var byteEnumName = ToolkitUtils.GetPrivateName(byteProp->Enum);
                    GenerateEnumDefinition(byteProp->Enum, "byte");
                    return () => byteEnumName;
                }
                
                return () => "byte";
            case "Int8Property":
                return () => "byte";
            case "Int16Property":
                return () => "short";
            case "UInt16Property":
                return () => "ushort";
            case "IntProperty":
                return () => "int";
            case "UInt32Property":
                return () => "uint";
            case "Int64Property":
                return () => "long";
            case "UInt64Property":
                return () => "ulong";
            case "FloatProperty":
                return () => "float";
            case "DoubleProperty":
                return () => "double";
            case "NameProperty":
                return () => "FName";
            case "StrProperty":
                return () => "FString";
            case "TextProperty":
                return () => "FText";
            case "DataTableRowHandle":
                return () => "FDataTableRowHandle";
            case "DelegateProperty":
                return () => "FScriptDelegate";
            case "MulticastInlineDelegateProperty":
            case "MulticastSparseDelegateProperty":
                return () => "FMulticastScriptDelegate";
            case "WeakObjectProperty":
                return () => "FWeakObjectPtr";
            case "ObjectProperty":
                var objPropType = GetObjectName((nint)((FObjectProperty*)prop)->PropertyClass);
                return () => SanitizeName(_UStructDefinitions.TryGetValue(objPropType, out var knownStruct) ? $"{knownStruct.DisplayName}*" : $"{objPropType}*");
            case "SoftObjectProperty":
                var softObjPropType = GetObjectName((nint)((FObjectProperty*)prop)->PropertyClass);
                return () => SanitizeName(_UStructDefinitions.TryGetValue(softObjPropType, out var knownStruct) ? $"TSoftObjectPtr<{knownStruct.DisplayName}>" : $"TSoftObjectPtr<{softObjPropType}>");
            case "SoftClassProperty":
                var softClassPropType = GetObjectName((nint)((FSoftClassProperty*)prop)->MetaClass);
                return () => SanitizeName(_UStructDefinitions.TryGetValue(softClassPropType, out var knownStruct) ? $"TSoftClassPtr<{knownStruct.DisplayName}>" : $"TSoftClassPtr<{softClassPropType}>");
            case "StructProperty":
                var structPropType = GetObjectName((nint)((FStructProperty*)prop)->Struct);
                return () => SanitizeName(_UStructDefinitions.TryGetValue(structPropType, out var knownStruct) ? knownStruct.DisplayName : structPropType);
            case "ClassProperty":
                var classPropClass = ((FClassProperty*)prop)->MetaClass;
                var classPropType = classPropClass != null ? ToolkitUtils.GetPrivateName(classPropClass) : "UClass*";
                return () => SanitizeName(_UStructDefinitions.TryGetValue(classPropType, out var knownStruct) ? $"{knownStruct.DisplayName}*" : classPropType);
            case "EnumProperty":
                var enumProp = (FEnumProperty*)prop;
                var enumPropName = ToolkitUtils.GetPrivateName(enumProp->Enum);
                GenerateEnumDefinition(enumProp->Enum, GetPropertyTypeName(enumProp->UnderlyingProp));
                return () => SanitizeName(enumPropName);
            case "MapProperty":
                var mapProp = (FMapProperty*)prop;
                var mapPropKeyType = GetPropertyTypeName(mapProp->KeyProp);
                var mapPropValueType = GetPropertyTypeName(mapProp->ValueProp);
                return () =>
                {
                    var isKeyPtr = mapPropKeyType.EndsWith('*') || mapPropKeyType.Contains('<'); // Use nint for pointers and generic types.
                    var isValuePtr = mapPropValueType.EndsWith('*') || mapPropValueType.Contains('<');
                    
                    _UStructDefinitions.TryGetValue(mapPropKeyType.TrimEnd('*'), out var knownKeyStruct);
                    _UStructDefinitions.TryGetValue(mapPropValueType.TrimEnd('*'), out var knownValueStruct);
                    
                    var keyType = isKeyPtr ? "nint" : SanitizeName(knownKeyStruct?.DisplayName ?? mapPropKeyType);
                    var valueType = isValuePtr ? "nint" : SanitizeName(knownValueStruct?.DisplayName ?? mapPropValueType);

                    if (isKeyPtr || isValuePtr)
                    {
                        return $"TMap<{keyType}, {valueType}> /* TMap<{mapPropKeyType}, {mapPropValueType}> */";
                    }
                    
                    return $"TMap<{keyType}, {valueType}>";
                };
            case "InterfaceProperty":
                var intPropType = GetObjectName((nint)((FInterfaceProperty*)prop)->InterfaceClass);
                return () => SanitizeName(_UStructDefinitions.TryGetValue(intPropType, out var knownStruct) ? $"TScriptInterface<{knownStruct.DisplayName}>" : $"TScriptInterface<{intPropType}>");
            case "ArrayProperty":
                var arrayPropType = GetPropertyTypeName(((FArrayProperty*)prop)->Inner);
                return () =>
                {
                    var isPtrType = arrayPropType.EndsWith('*') || arrayPropType.Contains('<'); // Use nint for pointers and generic types.
                    _UStructDefinitions.TryGetValue(arrayPropType.TrimEnd('*'), out var knownStruct);
                    return isPtrType ?
                        $"TArray<nint> /* TArray<{knownStruct?.DisplayName ?? arrayPropType}> */" :
                        $"TArray<{SanitizeName(knownStruct?.DisplayName ?? arrayPropType)}>";
                };
            case "SetProperty":
                var setPropType = GetPropertyTypeName(((FSetProperty*)prop)->ElementProp);
                return () =>
                {
                    var isPtrType = setPropType.EndsWith('*') || setPropType.Contains('<'); // Use nint for pointers and generic types.;
                    _UStructDefinitions.TryGetValue(setPropType.TrimEnd('*'), out var knownStruct);
                    return isPtrType
                        ? $"TSet<nint> /* TSet<{setPropType}> */"
                        : $"TSet<{SanitizeName(knownStruct?.DisplayName ?? setPropType)}>";
                };
            case "OptionalProperty":
                var optionalType = GetPropertyTypeName(((FOptionalProperty*)prop)->ValueProperty);
                return () =>
                {
                    if (_UStructDefinitions.TryGetValue(optionalType, out var knownOptType))
                        optionalType = knownOptType.DisplayName;
                    
                    return $"TOptional<{SanitizeName(optionalType)}>";
                };
            case "FieldPathProperty":
                return () => "FFieldPath";
            case "LazyObjectProperty":
                var lazyObjType = ToolkitUtils.GetPrivateName(((FLazyObjectProperty*)prop)->Super.PropertyClass);
                return () =>
                {
                    if (_UStructDefinitions.TryGetValue(lazyObjType, out var knownLazyType))
                        lazyObjType = knownLazyType.DisplayName;
                    
                    return $"TLazyObjectPtr<{SanitizeName(lazyObjType)}>";
                };
            default:
                Log.Warning($"Unknown Property: {className}");
                return () => className;
        }
    }

    /// <summary>
    /// Gets the property type name, such as <c>byte</c> for <c>ByteProperty</c>.
    /// </summary>
    /// <param name="prop"></param>
    /// <returns></returns>
    private static string GetPropertyTypeName(FProperty* prop)
    {
        var className = prop->Super.ClassPrivate->Name.ToString();
        switch (className)
        {
            case "BoolProperty":
                return "bool";
            case "ByteProperty" or "Int8Property":
                return "byte";
            case "Int16Property":
                return "short";
            case "UInt16Property":
                return "ushort";
            case "IntProperty":
                return "int";
            case "UInt32Property":
                return "uint";
            case "Int64Property":
                return "long";
            case "UInt64Property":
                return "ulong";
            case "FloatProperty":
                return "float";
            case "DoubleProperty":
                return "double";
            case "NameProperty":
                return "FName";
            case "StrProperty":
                return "FString";
            case "TextProperty":
                return "FText";
            case "DataTableRowHandle":
                return "FDataTableRowHandle";
            case "ObjectProperty":
                return $"{GetObjectName((nint)((FObjectProperty*)prop)->PropertyClass)}*";
            case "SoftObjectProperty":
                return $"TSoftObjectPtr<{GetObjectName((nint)((FObjectProperty*)prop)->PropertyClass)}>";
            case "SoftClassProperty":
                return $"TSoftClassPtr<{GetObjectName((nint)((FSoftClassProperty*)prop)->MetaClass)}>";
            case "StructProperty":
                return GetObjectName((nint)((FStructProperty*)prop)->Struct);
            case "ClassProperty":
                return GetObjectName((nint)((FClassProperty*)prop)->MetaClass);
            case "EnumProperty":
                return GetObjectName((nint)((FEnumProperty*)prop)->Enum);
            case "MapProperty":
                var mapProp = (FMapProperty*)prop;
                var mapPropKeyType = GetPropertyTypeName(mapProp->KeyProp);
                var mapPropValueType = GetPropertyTypeName(mapProp->ValueProp);
                return $"TMap<{mapPropKeyType}, {mapPropValueType}>";
            case "InterfaceProperty":
                return $"TScriptInterface<{GetObjectName((nint)((FInterfaceProperty*)prop)->InterfaceClass)}>";
            case "ArrayProperty":
                return $"TArray<{GetPropertyTypeName(((FArrayProperty*)prop)->Inner)}>";
            case "SetProperty":
                return $"TSet<{GetPropertyTypeName(((FSetProperty*)prop)->ElementProp)}>";
            case "DelegateProperty":
                return "FScriptDelegate";
            case "MulticastInlineDelegateProperty":
            case "MulticastSparseDelegateProperty":
                return "FMulticastScriptDelegate";
            case "WeakObjectProperty":
                return "FWeakObjectPtr";
            case "FieldPathProperty":
                return "FFieldPath";
            default:
                Log.Warning($"Unknown Property: {className}");
                return className;
        }
    }
    
    /// <summary>
    /// Gets the object name from a given object pointer.
    /// Mostly just a shortcut to base <see cref="UObjectBase"/> name property.
    /// </summary>
    /// <param name="objPtr">Object pointer.</param>
    /// <returns>The object name.</returns>
    private static string GetObjectName(nint objPtr) => ((UObjectBase*)objPtr)->NamePrivate.ToString();

    private (string Name, int Size, int Offset, string ClassName) GetBaseInfo(FProperty* prop)
    {
        var name = SanitizeName(prop->Super.NamePrivate.ToString());
        var size = prop->ElementSize;
        var offset = prop->Offset_Internal;
        var className = prop->Super.ClassPrivate->Name.ToString();
        return (name, size, offset, className);
    }

    private static string SanitizeName(string name)
    {
        var parts = name.Split('_');
        if (parts.Last().Length == 32 && parts.Length > 2)
        {
            name = string.Join(string.Empty, parts[..^2]);
        }

        name = name.Replace(' ', '_');
        name = name.Replace('-', '_');
        name = name.Replace('/', '_');
        name = name.Replace("?", string.Empty);
        name = name.Replace('(', '_');
        name = name.Replace(')', '_');
        
        if (char.IsDigit(name[0]))
        {
            name = '_' + name;
        }

        return name;
    }

    private interface ICsharpText
    {
        string Name { get; }
    
        string GetCsharpText();
    }
}