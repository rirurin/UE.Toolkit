using System.Runtime.InteropServices;

namespace UE.Toolkit.Core.Types.Unreal.UE5_4_4;

/**
 * Describes a pointer to a function bound to an Object.
 */
[StructLayout(LayoutKind.Sequential)]
public unsafe struct FDelegateProperty
{
    public FProperty Super;
    public UFunction* SignatureFunction;
}

/**
 * Describes a list of functions bound to an Object.
 */
[StructLayout(LayoutKind.Sequential)]
public unsafe struct FMulticastDelegateProperty
{
    public FProperty Super;
    
    /** Points to the source delegate function (the function declared with the delegate keyword) used in the declaration of this delegate property. */
    public UFunction* SignatureFunction;
}

[StructLayout(LayoutKind.Sequential)]
public struct FMulticastSparseDelegateProperty
{
    public FMulticastDelegateProperty Super;
}

[StructLayout(LayoutKind.Sequential)]
public struct FScriptDelegate // TScriptDelegate
{
    /** The object bound to this delegate, or nullptr if no object is bound */
    public FWeakObjectPtr Object;
    
    /** Name of the function to call on the bound object */
    public FName FunctionName;
}

[StructLayout(LayoutKind.Sequential)]
public struct FMulticastScriptDelegate // TMulticastScriptDelegate
{
    public TArray<FScriptDelegate> InvocationList;
}

//
// Describes a reference variable to another object which may be nil, and may turn nil at any point
//
[StructLayout(LayoutKind.Sequential)]
public struct FWeakObjectProperty
{
    public FObjectProperty Super;
}