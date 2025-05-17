// ReSharper disable InconsistentNaming

using System.Runtime.InteropServices;

namespace UE.Toolkit.Interfaces.Common.Types.Unreal;

[StructLayout(LayoutKind.Sequential)]
public struct TPair<TKeyType, TValueType>
    where TKeyType : unmanaged
    where TValueType : unmanaged
{
    public TKeyType Key;
    public TValueType Value;
}