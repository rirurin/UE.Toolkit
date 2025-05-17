using System.Runtime.InteropServices;

namespace UE.Toolkit.Core.Types.Unreal;

[StructLayout(LayoutKind.Sequential, Size = 0x28)]
public unsafe struct UObjectBase
{
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

    public readonly string GetName() => NamePrivate.ToString();

    public readonly bool IsA(string type) => ClassPrivate->Super.IsChildOf(type);
    
    public readonly bool IsChildOf(string type) => ClassPrivate->Super.IsChildOf(type);
}