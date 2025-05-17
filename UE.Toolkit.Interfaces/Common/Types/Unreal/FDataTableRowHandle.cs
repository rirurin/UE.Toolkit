using System.Runtime.InteropServices;

namespace UE.Toolkit.Interfaces.Common.Types.Unreal;

[StructLayout(LayoutKind.Sequential)]
public unsafe struct FDataTableRowHandle
{
    public UDataTable<UObjectBase>* DataTable;
    public FName RowName;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe struct FDataTableRowHandle<TRow>
 where TRow : unmanaged
{
    public UDataTable<TRow>* DataTable;
    public FName RowName;
}