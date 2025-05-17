using System.Runtime.InteropServices;

namespace UE.Toolkit.Core.Types.Unreal;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
public unsafe struct FFieldPath
{	
    /** Untracked pointer to the resolved property */
    public FField* ResolvedField;
    
    /** The cached owner of this field. Even though implemented as a weak pointer, GC will keep a strong reference to it if exposed through UPROPERTY macro */
    public FWeakObjectPtr ResolvedOwner;
    
    /** Path to the FField object from the innermost FField to the outermost UObject (UPackage) */
    public TArray<FName> Path;
}