// ReSharper disable InconsistentNaming
namespace UE.Toolkit.Reloaded.Common.Types.Unreal;

public enum EObjectFlags : uint
{
    Public = 1 << 0x0,
    Standalone = 1 << 0x1,
    MarkAsNative = 1 << 0x2,
    Transactional = 1 << 0x3,
    ClassDefaultObject = 1 << 0x4,
    ArchetypeObject = 1 << 0x5,
    Transient = 1 << 0x6,
    MarkAsRootSet = 1 << 0x7,
    TagGarbageTemp = 1 << 0x8,
    NeedInitialization = 1 << 0x9,
    NeedLoad = 1 << 0xa,
    KeepForCooker = 1 << 0xb,
    NeedPostLoad = 1 << 0xc,
    NeedPostLoadSubobjects = 1 << 0xd,
    NewerVersionExists = 1 << 0xe,
    BeginDestroyed = 1 << 0xf,
    FinishDestroyed = 1 << 0x10,
    BeingRegenerated = 1 << 0x11,
    DefaultSubObject = 1 << 0x12,
    WasLoaded = 1 << 0x13,
    TextExportTransient = 1 << 0x14,
    LoadCompleted = 1 << 0x15,
    InheritableComponentTemplate = 1 << 0x16,
    DuplicateTransient = 1 << 0x17,
    StrongRefOnFrame = 1 << 0x18,
    NonPIEDuplicateTransient = 1 << 0x19,
    Dynamic = 1 << 0x1a,
    WillBeLoaded = 1 << 0x1b,
    HasExternalPackage = 1 << 0x1c,
    PendingKill = 1 << 0x1d,
    Garbage = 1 << 0x1e,
    AllocatedInSharedPage = (uint)1 << 0x1f
}