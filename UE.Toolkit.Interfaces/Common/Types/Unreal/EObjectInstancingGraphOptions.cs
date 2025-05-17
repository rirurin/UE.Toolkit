namespace UE.Toolkit.Interfaces.Common.Types.Unreal;

public enum EObjectInstancingGraphOptions
{
    None = 0x00,

    // if set, start with component instancing disabled
    DisableInstancing = 0x01,

    // if set, instance only subobject template values
    InstanceTemplatesOnly = 0x02,
};