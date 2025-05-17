using System.Runtime.InteropServices;

// ReSharper disable InconsistentNaming

namespace UE.Toolkit.Core.Types.Unreal;

[StructLayout(LayoutKind.Sequential)]
public struct FSoftObjectPtr
{
    public TPersistentObjectPtr<FSoftObjectPath> Super;
}

[StructLayout(LayoutKind.Sequential)]
public struct TSoftObjectPtr<TObject> where TObject : unmanaged
{
    public FSoftObjectPtr SoftObjectPtr;
}

/**
 * TSoftClassPtr is a templatized wrapper around FSoftObjectPtr that works like a TSubclassOf, it can be used in UProperties for blueprint subclasses
 */
public struct TSoftClassPtr<TObject> where TObject : unmanaged
{
    public FSoftObjectPtr SoftObjectPtr;
}