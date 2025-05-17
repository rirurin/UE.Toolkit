using System.Runtime.InteropServices;

namespace UE.Toolkit.Core.Types.Unreal;

[StructLayout(LayoutKind.Sequential, Size = 0xB0)]
public unsafe struct UDataTable<TRow>
    where TRow : unmanaged
{
    public UObjectBase BaseObj;
    public UScriptStruct* RowStruct;
    public TMap<FName, TRow> RowMap;
}