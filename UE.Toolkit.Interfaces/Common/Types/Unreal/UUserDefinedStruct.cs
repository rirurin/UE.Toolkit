// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global
// ReSharper disable IdentifierTypo

using System.Runtime.InteropServices;

namespace UE.Toolkit.Interfaces.Common.Types.Unreal;

[StructLayout(LayoutKind.Sequential, Size = 0x108, Pack = 4)]
public struct UUserDefinedStruct
{
    public UScriptStruct Super;
    public EUserDefinedStructureStatus Status; // Size is 1 byte but dumped headers say GUID is located 4 bytes away, packing?
    public Guid Guid;
    
    /* Default instance of this struct with default values filled in, used to initialize structure */
    //FUserStructOnScopeIgnoreDefaults DefaultStructInstance;

    /* Bool to indicate we want to initialize a version of this struct without defaults, this is set while allocating the DefaultStructInstance itself */
    //bool bIgnoreStructDefaults;
}