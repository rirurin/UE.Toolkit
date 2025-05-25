using System.Runtime.InteropServices;
using UE.Toolkit.Core.Common;

// ReSharper disable InconsistentNaming

namespace UE.Toolkit.Core.Types.Unreal.UE5_4_4;

[StructLayout(LayoutKind.Sequential, Size = 0xB0, Pack = 8)]
public unsafe struct UStruct
{
    private static readonly Dictionary<string, Ptr<UStruct>> KnownStructs = [];

    /// <summary>
    /// INTERNAL USE ONLY, DO NOT TOUCH!!!
    /// </summary>
    public static UStruct_IsChildOf? UStruct_IsChildOf;
    
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

    public readonly bool IsChildOf(string uclass)
    {
        // Kinda hacky but probably accurate and doesn't require us
        // knowing AOT every object's native name when not using native IsChildOf.
        var hasPrefix = (uclass[0] == 'U' || uclass[0] == 'A' || uclass[0] == 'F') && char.IsUpper(uclass[1]); 
        if (hasPrefix) uclass = uclass[1..];
        
        fixed (UStruct* self = &this)
        {
            // Use native UStruct::IsChildOf for speed if we have
            // a reference to the class we're testing for.
            if (KnownStructs.TryGetValue(uclass, out var structPtr) && UStruct_IsChildOf != null)
            {
                return UStruct_IsChildOf(self, structPtr.Value) == 1;
            }
            
            // Manually search every parent struct for a match.
            for (var tempStruct = self; tempStruct != null; tempStruct = tempStruct->SuperStruct)
            {
                var privateName = ToolkitUtils.GetPrivateName((nint)tempStruct);
                KnownStructs.TryAdd(privateName, new(tempStruct));
                
                if (privateName == uclass) return true;
            }
        }

        return false;
    }
}

public unsafe delegate byte UStruct_IsChildOf(UStruct* self, UStruct* other);
