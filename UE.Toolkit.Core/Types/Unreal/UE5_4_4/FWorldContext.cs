using System.Runtime.InteropServices;

namespace UE.Toolkit.Core.Types.Unreal.UE5_4_4;


public enum WorldType : byte
{
    /** An untyped world, in most cases this will be the vestigial worlds of streamed in sub-levels */
    None,

    /** The game world */
    Game,

    /** A world being edited in the editor */
    Editor,

    /** A Play In Editor world */
    PIE,

    /** A preview world for an editor tool */
    EditorPreview,

    /** A preview world for a game */
    GamePreview,

    /** A minimal RPC world for a game */
    GameRPC,

    /** An editor world that was loaded but not currently being edited in the level editor */
    Inactive
}

[StructLayout(LayoutKind.Explicit, Size = 0x2c8)]
public unsafe struct FWorldContext
{
    [FieldOffset(0x0)] public WorldType WorldType;
    [FieldOffset(0x2c0)] public UWorld* ThisCurrentWorld;
}