using System.Collections;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using UE.Toolkit.Core.Types;
using UE.Toolkit.Core.Types.Unreal.Factories;
using UE.Toolkit.Core.Types.Unreal.Factories.Interfaces;
using UE.Toolkit.Core.Types.Unreal.Object;
using UE.Toolkit.Core.Types.Unreal.UE5_4_4;

namespace UE.Toolkit.Core.Common;

public static unsafe class ToolkitUtils
{
    private static readonly Dictionary<FName, string> PrivateToNativeNameMap = [];

    private static readonly Dictionary<FName, ClassMetadata> ClassMetadata = [];

    public static nint GetGlobalAddress(nint address) => *(int*)address + address + 4;
    
    public static string GetPrivateName(nint objPtr) => ((UObjectBase*)objPtr)->NamePrivate.ToString();

    /// <summary>
    /// Gets the <c>UObject</c>'s (expected) native name.<br/>
    /// This is different from its private name, which has no type prefix (the <c>U</c> in <c>UObject</c> for example),
    /// typically...<br/>
    /// VERY SLOW!!!
    /// </summary>
    /// <param name="objPtr">UObject instance</param>
    /// <returns><c>UObject</c> native name.</returns>
    public static string GetNativeName(nint objPtr)
    {
        var uobj = (UObjectBase*)objPtr;
        if (PrivateToNativeNameMap.TryGetValue(uobj->NamePrivate, out var nameNative)) return nameNative;
        
        var namePrivate = uobj->NamePrivate.ToString();
        nameNative = namePrivate;
        
        if (uobj->IsChildOf<UClass>())
        {
            nameNative = $"U{namePrivate}";
        }
        if (uobj->IsChildOf<AActor>())
        {
            nameNative = $"A{namePrivate}";
        }
        if (uobj->IsChildOf<UScriptStruct>())
        {
            // Already has the F struct prefix, UserDefinedStructs usually I think.
            var hasStructPrefix = namePrivate[0] == 'F' && char.IsUpper(namePrivate[1]);
            if (!hasStructPrefix)
            {
                nameNative = $"F{namePrivate}";
            }
        }

        PrivateToNativeNameMap[uobj->NamePrivate] = nameNative;
        return nameNative;
    }
    
    public static string GetNativeName(IUObject uobj)
    {
        if (PrivateToNativeNameMap.TryGetValue(uobj.NamePrivate, out var nameNative)) return nameNative;
        
        var namePrivate = uobj.NamePrivate.ToString();
        nameNative = namePrivate;
        
        if (uobj.IsChildOf<UClass>())
        {
            nameNative = $"U{namePrivate}";
        }
        if (uobj.IsChildOf<AActor>())
        {
            nameNative = $"A{namePrivate}";
        }
        if (uobj.IsChildOf<UScriptStruct>())
        {
            // Already has the F struct prefix, UserDefinedStructs usually I think.
            var hasStructPrefix = namePrivate[0] == 'F' && char.IsUpper(namePrivate[1]);
            if (!hasStructPrefix)
            {
                nameNative = $"F{namePrivate}";
            }
        }

        PrivateToNativeNameMap[uobj.NamePrivate] = nameNative;
        return nameNative;
    }
    
    public unsafe static TReturnType ProcessEvent<TReturnType>(IUObject self, IUnrealFactory factory, string funcName, params ProcessEventParameterBase[] param) 
        where TReturnType : unmanaged
    {
        var FuncName = new FName(funcName);
        var Name = self.ClassPrivate.NamePrivate;
        ClassMetadata? Metadata = null;

        SetMetadataForClass(self.ClassPrivate, factory);
        //return default;
        var func = ClassMetadata[Name][FuncName];
        unsafe
        {
            func.CollectParameters(out var fParams, param);
            var processEvent = Marshal.GetDelegateForFunctionPointer<UObject_ProcessEvent>(*(nint*)(self.VTable + 0x220));
            processEvent((UObjectBase*)self.Ptr, (UFunction*)func.Function.Ptr, func.ParamAlloc);
            func.CopyReferenceValues(fParams);
            return (TReturnType*)func.ReturnValue.Ptr != null ? *(TReturnType*)(func.ParamAlloc + func.ReturnValue.Offset_Internal) : default;
        }
    }

    internal static void SetMetadataForClass(IUClass Class, IUnrealFactory Factory)
    {
        if (!ClassMetadata.TryGetValue(Class.NamePrivate, out _))
        {
            ClassMetadata[Class.NamePrivate] = new ClassMetadata(Class, Factory);
            if (Class.GetSuperClass() != null)
            {
                SetMetadataForClass(Class.GetSuperClass()!, Factory);
            }
        }
    }

    internal static bool TryGetClassMetadata(IUClass Class, [MaybeNullWhen(false)] out ClassMetadata Result)
        => ClassMetadata.TryGetValue(Class.NamePrivate, out Result);
}

public class ClassMetadata
{

    /// <summary>
    /// Pointer to Unreal's reflection data for the target class
    /// </summary>
    private IUClass Class;

    /// <summary>
    /// Managed list of functions exposed to blueprints for the target class
    /// </summary>
    private Dictionary<FName, FunctionMetadata> Functions = [];

    private unsafe void AddFunctionsFromClass(IUnrealFactory Factory)
    {
        
        foreach (var Func in Class.GetFunctionMap())
        {

            var NewFunc = Factory.CreateUStruct((nint)(*Func.Value.Value).Value);
            //DebugLog.Print($"Add function {NewFunc.NamePrivate} in {Class.GetNativeName()} (0x{NewFunc.Ptr:x})");
            Functions.Add(Func.Key, new(NewFunc, Factory));
        }
    }

    public unsafe ClassMetadata(IUClass _class, IUnrealFactory Factory)
    {
        Class = _class;
        AddFunctionsFromClass(Factory);
    }
    public FunctionMetadata this[FName name]
    {
        get {
            if (TryGetFunction(name, out var value))
            {
                return value;
            } else
            {
                throw new KeyNotFoundException();
            }
        }
    }

    public bool TryGetFunction(FName name, [MaybeNullWhen(false)] out FunctionMetadata result)
    {
        if (Functions.TryGetValue(name, out result))
        {
            return true;
        }
        else if (Class.GetSuperClass() != null && ToolkitUtils.TryGetClassMetadata(Class.GetSuperClass()!, out var parent))
        {
            return parent.TryGetFunction(name, out result);
        } else
        {
            return false;
        }
    }
}

public class FunctionMetadata : IDisposable
{
    private bool disposedValue;

    /// <summary>
    /// Pointer to Unreal's function reflection data.
    /// </summary>
    public IUStruct Function { get; }

    /// <summary>
    /// The return value for the function. If the function returns void, then this
    /// will be null
    /// </summary>
    public IFProperty ReturnValue { get; }

    /// <summary>
    /// List of all parameters required for the function
    /// </summary>
    public Dictionary<string, IFProperty> Parameters { get; }
    
    private nint ParameterAllocationSize;

    private IUnrealFactory Factory;

    /// <summary>
    /// Memory used to store parameters when invoking method
    /// </summary>
    private ConcurrentDictionary<Thread, nint> ParameterAllocation { get; } = [];

    private unsafe (Dictionary<string, IFProperty>, IFProperty, int) GetParameters()
    {
        IFProperty ReturnValue = Factory.CreateFProperty(nint.Zero);
        Dictionary<string, IFProperty> Parameters = new();
        var LastFieldOffset = 0;
        var LastFieldSize = 0;
        if (Function.ChildProperties != null)
        {
            foreach (var Field in Function.ChildProperties)
            {
                var Prop = Factory.CreateFProperty(Field.Ptr);
                //DebugLog.Print($"{Prop.NamePrivate}: 0x{Prop.Offset_Internal:x} (size 0x{Prop.ElementSize:x})");
                // Collect parameters
                if (Prop.PropertyFlags.HasFlag(EPropertyFlags.CPF_ReturnParm))
                {
                    ReturnValue = Prop;
                }
                else
                {
                    Parameters.Add(Prop.NamePrivate, Prop);
                }
                // Get allocation size maximum
                var ofs = Prop.Offset_Internal;
                if (ofs > LastFieldOffset)
                {
                    LastFieldOffset = ofs;
                    LastFieldSize = Prop.ElementSize;
                }
            }
        }
        return (Parameters, ReturnValue, LastFieldOffset + LastFieldSize);
    }

    public unsafe FunctionMetadata(IUStruct _Function, IUnrealFactory _Factory)
    {
        Function = _Function;
        Factory = _Factory;
        (Parameters, ReturnValue, ParameterAllocationSize) = GetParameters();
    }

    public nint ParamAlloc
    {
        get {
            unsafe
            {
                return ParameterAllocation.GetOrAdd(Thread.CurrentThread, (nint)NativeMemory.Alloc((nuint)ParameterAllocationSize));
            }
        }
    }

    
    internal void CollectParameters(out Dictionary<ProcessEventParameterBase, IFProperty> outNativeProps, params ProcessEventParameterBase[] funcParams)
    {
        outNativeProps = new();
        var alloc = ParamAlloc;
        foreach (var param in funcParams)
        {
            var prop = Parameters[new(param.Name)];
            unsafe
            {
                outNativeProps.Add(param, prop);
                param.AddToAllocation(alloc + prop.Offset_Internal);
            }
        }
    }

    internal unsafe void CopyReferenceValues(Dictionary<ProcessEventParameterBase, IFProperty> nativeProps)
    {
        var alloc = ParamAlloc;
        foreach (var nativeProp in nativeProps)
        {
            nativeProp.Key.RetrieveValue(alloc + nativeProp.Value.Offset_Internal);
        }
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                // TODO: dispose managed state (managed objects)
            }
            unsafe
            {
                foreach (var tsAlloc in ParameterAllocation)
                {
                    ParameterAllocation.Remove(tsAlloc.Key, out nint Alloc);
                    NativeMemory.Free((void*)Alloc);
                }
            }
            disposedValue = true;
        }
    }
    ~FunctionMetadata()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: false);
    }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}