namespace UE.Toolkit.Core.Types.Unreal.Factories.Interfaces;

public interface IFBoolProperty : IFProperty
{
    byte FieldSize { get; }
    byte ByteOffset { get; }
    byte ByteMask { get; }
    byte FieldMask { get; }
}