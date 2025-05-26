using System.Runtime.InteropServices;
using UE.Toolkit.Core.Common;

namespace UE.Toolkit.Core.Types.Unreal.UE5_4_4;

[StructLayout(LayoutKind.Sequential, Size = 0x28)]
public unsafe struct UObjectBase
{
    private static readonly Dictionary<string, EClassCastFlags> CastFlagsMap
        = Enum.GetValues<EClassCastFlags>().ToDictionary(x => x.ToString(), x => x);
    
    private static readonly Dictionary<FName, HashSet<string>> StructChainMappings = [];
    
    public nint VTable;
    public EObjectFlags ObjectFlags;
    public int InternalIndex;
    public UClass* ClassPrivate;
    public FName NamePrivate;
    public UObjectBase* OuterPrivate;

    public readonly UObjectBase* GetOuterPrivate() => OuterPrivate;

    public readonly UObjectBase* GetOutermost()
    {
        fixed (UObjectBase* obj = &this)
        {
            var currObj = obj;
            while (currObj->OuterPrivate != null) currObj = currObj->OuterPrivate;
            return currObj;
        }
    }
    
    public bool IsChildOf(string type)
    {
        var ofClassName = type;
        var hasPrefix = (ofClassName[0] == 'U' || ofClassName[0] == 'A' || ofClassName[0] == 'F') && char.IsUpper(ofClassName[1]); 
        if (hasPrefix) ofClassName = ofClassName[1..];

        if (ofClassName == ToolkitUtils.GetPrivateName((nint)ClassPrivate)) return true;

        // TODO: Optimize (again) by caching the result between types.
        var hasCastFlag = CastFlagsMap.TryGetValue(type, out var ofClassFlag);
        
        // If self is a UClass, first check own cast flags and
        // struct chain.
        if (ClassPrivate->ClassCastFlags.HasFlag(EClassCastFlags.UClass))
        {
            fixed (UObjectBase* self = &this)
            {
                var selfClass = (UClass*)self;
                if (hasCastFlag)
                {
                    if ((selfClass->ClassCastFlags & ofClassFlag) == ofClassFlag) return true;
                }
                
                // Populate struct chain data.
                // TODO: Possibly optimize it using NamePrivate to Struct* mappings but idc rn!
                InitStructChain((UStruct*)selfClass);
                if (StructChainMappings[NamePrivate].Contains(ofClassName)) return true;
            }
        }

        // Check object's private class cast flags.
        return hasCastFlag && (ClassPrivate->ClassCastFlags & ofClassFlag) == ofClassFlag;
    }

    public bool IsChildOf<T>() => IsChildOf(typeof(T).Name);

    private static void InitStructChain(UStruct* baseStruct)
    {
        if (StructChainMappings.ContainsKey(baseStruct->Super.Super.NamePrivate)) return;

        var names = new HashSet<string>();
        for (var tempStruct = baseStruct; tempStruct != null; tempStruct = tempStruct->SuperStruct)
        {
            names.Add(tempStruct->Super.Super.NamePrivate.ToString());
        }

        StructChainMappings[baseStruct->Super.Super.NamePrivate] = names;
    }
}