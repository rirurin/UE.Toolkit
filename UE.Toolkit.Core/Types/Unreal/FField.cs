using System.Runtime.InteropServices;

namespace UE.Toolkit.Core.Types.Unreal;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
public unsafe struct FField
{
    public nint VTable;
    public FFieldClass* ClassPrivate;
    public FFieldObjectUnion Owner; // May be garbage? The following fields are correctly placed,
                                    // but within a UserDefinedStruct this field is random data (not null).
    public FField* Next;
    public FName NamePrivate;
    public EObjectFlags FlagsPrivate;
}