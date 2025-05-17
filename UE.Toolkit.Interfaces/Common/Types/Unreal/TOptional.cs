using System.Runtime.InteropServices;
// ReSharper disable InconsistentNaming

namespace UE.Toolkit.Interfaces.Common.Types.Unreal;

[StructLayout(LayoutKind.Sequential)]
public struct TOptional<TOptionalType>
{
    public TOptionalType Value;
}