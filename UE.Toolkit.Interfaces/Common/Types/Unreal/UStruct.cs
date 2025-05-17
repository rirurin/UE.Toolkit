using System.Runtime.InteropServices;

namespace UE.Toolkit.Interfaces.Common.Types.Unreal;

[StructLayout(LayoutKind.Sequential, Size = 0xB0, Pack = 8)]
public unsafe struct UStruct
{
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
                if (ToolkitUtils.GetUObjectName(tempStruct) == type) return true;
            }
        }

        return false;
    }
}