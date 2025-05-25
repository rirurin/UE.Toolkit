using System.Runtime.InteropServices;

namespace UE.Toolkit.Core.Types.Unreal.UE5_4_4;

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
    
    public readonly bool IsChildOf(string type) => ClassPrivate->IsChildOf(type);

    public readonly bool IsChildOf<T>() => IsChildOf(typeof(T).Name);

    public readonly bool IsA(string type) => IsChildOf(type);

    public readonly bool IsA<T>() => IsChildOf(typeof(T).Name);
}