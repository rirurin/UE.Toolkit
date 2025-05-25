using System.Runtime.InteropServices;

// ReSharper disable InconsistentNaming

namespace UE.Toolkit.Core.Types.Unreal.UE5_4_4;

[StructLayout(LayoutKind.Sequential)]
public struct TOptional<TOptionalType>
{
    public TOptionalType Value;
}