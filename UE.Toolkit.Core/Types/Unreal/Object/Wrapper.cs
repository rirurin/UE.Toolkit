using System;
using System.Runtime.InteropServices;
using UE.Toolkit.Core.Common;
using UE.Toolkit.Core.Types.Unreal.UE5_4_4;

namespace UE.Toolkit.Core.Types.Unreal.Object;

/// <summary>
/// Simple <see cref="UObjectBase"/> wrapper containing the name and instance.
/// </summary>
public unsafe class ToolkitUObject<TObject>(TObject* self) where TObject : unmanaged
{
    /// <summary>
    /// Object instance.
    /// </summary>
    public TObject* Self { get; } = self;

    /// <summary>
    /// Object name.
    /// </summary>
    public string Name { get; } = ToolkitUtils.GetNativeName((nint)self);

    /*
    /// <summary>
    /// Invoke a function that's accessible from blueprints by the provided name.
    /// </summary>
    /// <typeparam name="TReturnType">What the function is expected to return</typeparam>
    /// <param name="obj">The instance of an object to call the function from</param>
    /// <param name="funcName">The name of the function to call</param>
    /// <param name="param">Input parameters to pass into the method</param>
    /// <returns></returns>
    public unsafe TReturnType ProcessEvent<TReturnType>(string funcName, params ProcessEventParameterBase[] param) where TReturnType : unmanaged
        => ToolkitUtils.ProcessEvent<TReturnType, TObject>(Self, funcName, param);
    */
}

internal unsafe delegate void UObject_ProcessEvent(UObjectBase* self, UFunction* targetFunc, nint paramData);

/// <summary>
/// Simple <see cref="UFunction"/> wrapper
/// </summary>
public unsafe class ToolkitFunction
{
    private static readonly Dictionary<FName, FunctionMetadata> FunctionMetadata = [];
    /// <summary>
    /// Object instance
    /// </summary>
    public UFunction* Self { get; }

    public ToolkitFunction(UFunction* self)
    {
        Self = self;
    }

    /*
    public FProperty* GetReturnValue()
    {

    }
    */

}