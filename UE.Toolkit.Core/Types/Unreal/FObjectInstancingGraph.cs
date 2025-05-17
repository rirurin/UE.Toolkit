using System.Runtime.InteropServices;

namespace UE.Toolkit.Core.Types.Unreal;

[StructLayout(LayoutKind.Sequential)]
public unsafe struct FObjectInstancingGraph
{
    public UObjectBase* SourceRoot;
    public UObjectBase* DestinationRoot;
    public EObjectInstancingGraphOptions InstancingOptions;
    public bool bCreatingArchetype;
    public bool bLoadingObject;
    public TMap<nint, nint> SourceToDestinationMap; // TMap<UObject*, UObject*>
    public TSet<nint> SubobjectInstantiationExclusionList; // TSet<FProperty>
}