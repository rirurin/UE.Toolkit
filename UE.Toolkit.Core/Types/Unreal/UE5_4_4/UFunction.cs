// ReSharper disable InconsistentNaming
// ReSharper disable IdentifierTypo

namespace UE.Toolkit.Core.Types.Unreal.UE5_4_4;

/// <summary>
/// Reflection data for a replicated or Kismet callable function.
/// </summary>
public unsafe struct UFunction
{
    public UStruct Super;
    public EFunctionFlags FunctionFlags;
    public byte NumParms;
    public ushort ParmsSize;
    public ushort ReturnValueOffset;
    public ushort RPCId;
    public ushort RPCResponseId;
    public FProperty* FirstPropertyToInit;

    public UFunction* EventGraphFunction; //#if UE_BLUEPRINT_EVENTGRAPH_FASTCALLS
    public int EventGraphCallOffset;	  //#if UE_BLUEPRINT_EVENTGRAPH_FASTCALLS

    //private UFunction** SingletonPtr;   //#if WITH_LIVE_CODING
    public void* Func;
}