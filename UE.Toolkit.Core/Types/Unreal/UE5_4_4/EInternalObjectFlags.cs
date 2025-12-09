// ReSharper disable InconsistentNaming
// ReSharper disable GrammarMistakeInComment

namespace UE.Toolkit.Core.Types.Unreal.UE5_4_4;

/**
 * Internal flags stored in the global object table, used by the garbage collector.
*/
[Flags]
public enum EInternalObjectFlags : uint
{
	None = 0,
    ReachableInCluster = 0x800000, /// External reference to object in cluster exists
	ClusterRoot = 0x1000000, /// Root of a cluster
	Native = 0x2000000, /// Native (UClass only). 
	Async = 0x4000000, /// Object exists only on a different thread than the game thread.
	AsyncLoading = 0x8000000, /// Object is being asynchronously loaded.
	Unreachable = 0x10000000, /// Object is not reachable on the object graph.
	PendingKill = 0x20000000, /// Objects that are pending destruction (invalid for gameplay but valid objects)
	RootSet = 0x40000000, /// Object will not be garbage collected, even if unreferenced.
	PendingConstruction = 0x80000000, /// Object didn't have its class constructor called yet (only the UObjectBase one to initialize its most basic members)
}