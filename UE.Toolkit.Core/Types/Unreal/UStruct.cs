using System.Runtime.InteropServices;
using UE.Toolkit.Core.Common;

namespace UE.Toolkit.Core.Types.Unreal;

[StructLayout(LayoutKind.Sequential, Size = 0xB0, Pack = 8)]
public unsafe struct UStruct
{
    private static readonly string[] BaseTypes = ["UClass", "UInterface", "AActor", "UScriptStruct"];
        
    public UField Super;
    private fixed byte Unknown[0x10]; // Z_Construct_UClass_UStruct_Statics?
    public UStruct* SuperStruct;
    
    /** Pointer to start of linked list of child fields */
    public UField* Children;
    
    /** Pointer to start of linked list of child fields */
    public FField* ChildProperties;
    
    public int PropertiesSize;
    public int MinAlignment;
    public TArray<byte> Script;
    
    /** In memory only: Linked list of properties from most-derived to base */
    public FProperty* PropertyLink;
    
    public FProperty* RefLink;
    public FProperty* DestructorLink;
    public FProperty* PostConstructLink;

    public readonly bool IsA(string type) => IsChildOf(type);

    public readonly bool IsChildOf(string type)
    {
        fixed (UStruct* self = &this)
        {
            for (var tempStruct = self; tempStruct != null; tempStruct = tempStruct->SuperStruct)
            {
                // ToolkitUtils.GetUObjectName calls IsChildOf to resolve name prefixes
                // and IsChildOf calls GetUObjectName to compare types, leading to a stack overflow.
                // Fix: Check types used in GetUObjectName by their private name.
                if (BaseTypes.Any(x => x == type))
                {
                    if (((UObjectBase*)tempStruct)->NamePrivate.ToString() == type[1..]) return true;
                }
                else
                {
                    if (ToolkitUtils.GetNativeName(tempStruct) == type) return true;
                }
            }
        }

        return false;
    }

    public readonly bool IsChildOf<T>() => IsChildOf(typeof(T).Name);

    public readonly bool IsA<T>() => IsChildOf(typeof(T).Name);
}