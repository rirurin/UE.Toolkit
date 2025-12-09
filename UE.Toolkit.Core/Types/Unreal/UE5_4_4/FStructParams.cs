using System.Runtime.InteropServices;

namespace UE.Toolkit.Core.Types.Unreal.UE5_4_4;

[StructLayout(LayoutKind.Sequential)]
public unsafe struct FStructParams
{
    public nint OuterFunc; // UObject* (*OuterFunc)(); - ZConstruct for UPackage
    public nint SuperFunc; // UScriptStruct* (*OuterFunc)();
    public nint StructOpsFunc; // UScriptStruct::ICppStructOps* (*StructOpsFunc)();
    public nint NameUTF8;
    public FPropertyParamsBase** Properties;
    public ushort NumProperties;
    public ushort SizeOf;
    public byte AlignOf;
    public EObjectFlags ObjectFlags;
    public EStructFlags StructFlags;

    public FPropertyParamsBase* GetProperty(int Index)
    {
        if (Index < 0 || Index >= NumProperties) return null;
        return Properties[Index];
    }
}