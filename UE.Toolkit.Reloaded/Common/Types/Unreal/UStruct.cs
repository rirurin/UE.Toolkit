using System.Runtime.InteropServices;

namespace UE.Toolkit.Reloaded.Common.Types.Unreal;

[StructLayout(LayoutKind.Sequential, Size = 0xB0, Pack = 8)]
public unsafe struct UStruct
{
    public UField Super;
    private fixed byte Unknown[0x10];
    public UStruct* SuperStruct;
    public UField* Children;
    public FField* ChildProperties;
    public int PropertiesSize;
    public int MinAlignment;
    public TArray<byte> Script;
    public FProperty* PropertyLink; 
    public FProperty* RefLink;
    public FProperty* DestructorLink;
    public FProperty* PostConstructLink;
}