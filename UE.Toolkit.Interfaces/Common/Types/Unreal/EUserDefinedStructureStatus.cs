// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global
// ReSharper disable IdentifierTypo

namespace UE.Toolkit.Interfaces.Common.Types.Unreal;

public enum EUserDefinedStructureStatus : byte
{
    /** Struct is in an unknown state. */
    UDSS_UpToDate,
    /** Struct has been modified but not recompiled. */
    UDSS_Dirty,
    /** Struct tried but failed to be compiled. */
    UDSS_Error,
    /** Struct is a duplicate, the original one was changed. */
    UDSS_Duplicate,

    UDSS_MAX,
};