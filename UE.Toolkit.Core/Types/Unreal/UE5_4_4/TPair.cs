// ReSharper disable InconsistentNaming

using System.Runtime.InteropServices;

namespace UE.Toolkit.Core.Types.Unreal.UE5_4_4;

[StructLayout(LayoutKind.Sequential)]
public struct TPair<TKeyType, TValueType>
    where TKeyType : unmanaged
    where TValueType : unmanaged
{
    public TKeyType Key;
    public TValueType Value;
}