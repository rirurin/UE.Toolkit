using System.Collections;
using System.Runtime.InteropServices;
using UE.Toolkit.Core.Common;
using UE.Toolkit.Core.Types.Unreal.Factories.Interfaces;
using UE.Toolkit.Core.Types.Unreal.UE5_4_4;

// ReSharper disable InconsistentNaming

namespace UE.Toolkit.Core.Types.Unreal.Factories.UE5_4_4;

public class UnrealFactory : BaseUnrealFactory
{
    
    public override nint SizeOf<T>()
    {
        var TypeName = typeof(T).Name;
        unsafe
        {
            return TypeName switch
            {
                nameof(IUObject) => sizeof(UObjectBase),
                nameof(IUClass) => sizeof(UClass),
                nameof(IUScriptStruct) => sizeof(UScriptStruct),
                nameof(IUEnum) => sizeof(UEnum),
                nameof(IUUserDefinedEnum) => sizeof(UUserDefinedEnum),
                nameof(IFProperty) => sizeof(FProperty),
                nameof(IFByteProperty) => sizeof(FByteProperty),
                nameof(IFBoolProperty) => sizeof(FBoolProperty),
                nameof(IFEnumProperty) => sizeof(FEnumProperty),
                nameof(IFObjectProperty) => sizeof(FObjectProperty),
                nameof(IFSoftClassProperty) => sizeof(FSoftClassProperty),
                nameof(IFClassProperty) => sizeof(FClassProperty),
                nameof(IFStructProperty) => sizeof(FStructProperty),
                nameof(IFMapProperty) => sizeof(FMapProperty),
                nameof(IFInterfaceProperty) => sizeof(FInterfaceProperty),
                nameof(IFArrayProperty) => sizeof(FArrayProperty),
                nameof(IFSetProperty) => sizeof(FSetProperty),
                nameof(IFOptionalProperty) => sizeof(FOptionalProperty),
                _ => throw new NotSupportedException(TypeName)
            };
        }
    }
    
    public override IFProperty CreateFProperty(nint ptr) => new FProperty_UE5_4_4(ptr, this);
    public override IFByteProperty CreateFByteProperty(nint ptr) => new FByteProperty_UE5_4_4(ptr, this);
    public override IFBoolProperty CreateFBoolProperty(nint ptr) => new FBoolProperty_UE5_4_4(ptr, this);
    public override IFEnumProperty CreateFEnumProperty(nint ptr) => new FEnumProperty_UE5_4_4(ptr, this);
    public override IFObjectProperty CreateFObjectProperty(nint ptr) => new FObjectProperty_UE5_4_4(ptr, this);
    public override IFSoftClassProperty CreateFSoftClassProperty(nint ptr) => new FSoftClassProperty_UE5_4_4(ptr, this);
    public override IFClassProperty CreateFClassProperty(nint ptr) => new FClassProperty_UE5_4_4(ptr, this);
    public override IFStructProperty CreateFStructProperty(nint ptr) => new FStructProperty_UE5_4_4(ptr, this);
    public override IFMapProperty CreateFMapProperty(nint ptr) => new FMapProperty_UE5_4_4(ptr, this);
    public override IFInterfaceProperty CreateFInterfaceProperty(nint ptr) => new FInterfaceProperty_UE5_4_4(ptr, this);
    public override IFArrayProperty CreateFArrayProperty(nint ptr) => new FArrayProperty_UE5_4_4(ptr, this);
    public override IFSetProperty CreateFSetProperty(nint ptr) => new FSetProperty_UE5_4_4(ptr, this);
    public override IFOptionalProperty CreateFOptionalProperty(nint ptr) => new FOptionalProperty_UE5_4_4(ptr, this);
    public override IUObjectArray CreateUObjectArray(nint ptr) => new UObjectArray_UE5_4_4(ptr, this);
    public override IUObject CreateUObject(nint ptr) => new UObject_UE5_4_4(ptr, this);
    public override IUClass CreateUClass(nint ptr) => new UClass_UE5_4_4(ptr, this);
    public override IUScriptStruct CreateUScriptStruct(nint ptr) => new UScriptStruct_UE5_4_4(ptr, this);
    public override IUEnum CreateUEnum(nint ptr) => new UEnum_UE5_4_4(ptr, this);
    public override IUField CreateUField(nint ptr) => new UField_UE5_4_4(ptr, this);
    public override IUStruct CreateUStruct(nint ptr) => new UStruct_UE5_4_4(ptr, this);
    public override IUUserDefinedEnum CreateUUserDefinedEnum(nint ptr) => new UUserDefinedEnum_UE5_4_4(ptr, this);
    public override IFFieldClass CreateFFieldClass(nint ptr) => new FFieldClass_UE5_4_4(ptr, this);
    public override IFField CreateFField(nint ptr) => new FField_UE5_4_4(ptr, this);
    public override IFStructParams CreateFStructParams(nint ptr) => new FStructParamsUE5_4_4(ptr, this);
    public override IFPropertyParams CreateFPropertyParams(nint ptr) => new FPropertyParamsUE5_4_4(ptr, this);
}

public unsafe class FOptionalProperty_UE5_4_4(nint ptr, IUnrealFactory factory)
    : FProperty_UE5_4_4(ptr, factory), IFOptionalProperty
{
    public IFProperty ValueProperty => _factory.CreateFProperty((nint)((FOptionalProperty*)Ptr)->ValueProperty);
}

public unsafe class FSetProperty_UE5_4_4(nint ptr, IUnrealFactory factory)
    : FProperty_UE5_4_4(ptr, factory), IFSetProperty
{
    public IFProperty ElementProp => _factory.CreateFProperty((nint)((FSetProperty*)Ptr)->ElementProp);
}

public unsafe class FArrayProperty_UE5_4_4(nint ptr, IUnrealFactory factory)
    : FProperty_UE5_4_4(ptr, factory), IFArrayProperty
{
    public IFProperty Inner => _factory.CreateFProperty((nint)((FArrayProperty*)Ptr)->Inner);
}

public unsafe class FInterfaceProperty_UE5_4_4(nint ptr, IUnrealFactory factory)
    : FProperty_UE5_4_4(ptr, factory), IFInterfaceProperty
{
    public IUClass InterfaceClass => _factory.CreateUClass((nint)((FInterfaceProperty*)Ptr)->InterfaceClass);
}

public unsafe class FMapProperty_UE5_4_4(nint ptr, IUnrealFactory factory)
    : FProperty_UE5_4_4(ptr, factory), IFMapProperty
{
    public IFProperty KeyProp => _factory.CreateFProperty((nint)((FMapProperty*)Ptr)->KeyProp);
    public IFProperty ValueProp => _factory.CreateFProperty((nint)((FMapProperty*)Ptr)->ValueProp);
}

public unsafe class FStructProperty_UE5_4_4(nint ptr, IUnrealFactory factory)
    : FProperty_UE5_4_4(ptr, factory), IFStructProperty
{
    public IUScriptStruct Struct => _factory.CreateUScriptStruct((nint)((FStructProperty*)Ptr)->Struct);
}

public unsafe class FClassProperty_UE5_4_4(nint ptr, IUnrealFactory factory)
    : FProperty_UE5_4_4(ptr, factory), IFClassProperty
{
    public IUClass PropertyClass => _factory.CreateUClass((nint)((FClassProperty*)Ptr)->Super.PropertyClass);
    public IUClass MetaClass => _factory.CreateUClass((nint)((FClassProperty*)Ptr)->MetaClass);
}

public unsafe class FSoftClassProperty_UE5_4_4(nint ptr, IUnrealFactory factory)
    : FProperty_UE5_4_4(ptr, factory), IFSoftClassProperty
{
    public IUClass MetaClass => _factory.CreateUClass((nint)((FSoftClassProperty*)Ptr)->MetaClass);
}

public unsafe class FObjectProperty_UE5_4_4(nint ptr, IUnrealFactory factory)
    : FProperty_UE5_4_4(ptr, factory), IFObjectProperty
{
    public IUClass PropertyClass => _factory.CreateUClass((nint)((FObjectProperty*)Ptr)->PropertyClass);
}

public unsafe class FEnumProperty_UE5_4_4(nint ptr, IUnrealFactory factory)
    : FProperty_UE5_4_4(ptr, factory), IFEnumProperty
{
    private readonly FEnumProperty* _self = (FEnumProperty*)ptr;

    public IUEnum Enum => _factory.CreateUEnum((nint)_self->Enum);
    public IFProperty UnderlyingProp => _factory.CreateFProperty((nint)_self->UnderlyingProp);
}

public unsafe class FFieldClass_UE5_4_4(nint ptr, IUnrealFactory factory)
    : IFFieldClass
{
    private readonly FFieldClass* _self = (FFieldClass*)ptr;

    public nint Ptr => ptr;
    public string Name => _self->Name.ToString();
    public ulong Id => _self->Id;
    public ulong CastFlags => _self->CastFlags;
    public EClassFlags ClassFlags => _self->ClassFlags;
    public IFFieldClass SuperClass => factory.CreateFFieldClass((nint)_self->SuperClass);
    public IFField DefaultObject => factory.CreateFField((nint)_self->DefaultObject);
    public nint FieldConstructor => _self->FieldConstructor;
}

public unsafe class FField_UE5_4_4(nint ptr, IUnrealFactory factory)
    : IFField
{
    private readonly FField* _self = (FField*)ptr;
    protected readonly IUnrealFactory _factory = factory;

    public nint Ptr => ptr;
    public nint VTable => _self->VTable;
    public IFFieldClass ClassPrivate => _factory.CreateFFieldClass((nint)_self->ClassPrivate);
    public FFieldObjectUnion Owner => _self->Owner;
    public IFField? Next => _self->Next != null ? _factory.CreateFField((nint)_self->Next) : null;
    public string NamePrivate => _self->NamePrivate.ToString();
    public EObjectFlags FlagsPrivate => _self->FlagsPrivate;
}

public unsafe class FProperty_UE5_4_4(nint ptr, IUnrealFactory factory)
    : FField_UE5_4_4(ptr, factory), IFProperty
{
    private readonly FProperty* _self = (FProperty*)ptr;

    public int ArrayDim => _self->ArrayDim;
    public int ElementSize => _self->ElementSize;
    public EPropertyFlags PropertyFlags => _self->PropertyFlags;
    public ushort RepIndex => _self->RepIndex;
    public byte BlueprintReplicationCondition => _self->BlueprintReplicationCondition;
    public int Offset_Internal => _self->Offset_Internal;
    public IEnumerable<IFProperty> PropertyLinkNext
        => new IFPropertyEnumerable(_factory.CreateFProperty((nint)_self->PropertyLinkNext), PropertyType.PropertyLink,
            _factory);
    public IEnumerable<IFProperty> NextRef
        => new IFPropertyEnumerable(_factory.CreateFProperty((nint)_self->NextRef), PropertyType.NextRef,
            _factory);
    public IEnumerable<IFProperty> DestructorLinkNext
        => new IFPropertyEnumerable(_factory.CreateFProperty((nint)_self->DestructorLinkNext), PropertyType.DestructorLink,
            _factory);
    public IEnumerable<IFProperty> PostConstructLinkNext
        => new IFPropertyEnumerable(_factory.CreateFProperty((nint)_self->PostConstructLinkNext), PropertyType.PostConstructLink,
            _factory);
    public string RepNotifyFunc => _self->RepNotifyFunc.ToString();
}

public unsafe class FProperty_4_27_2(nint ptr, IUnrealFactory factory)
    : FField_UE5_4_4(ptr, factory), IFProperty
{
    private readonly FProperty* _self = (FProperty*)ptr;

    public int ArrayDim => _self->ArrayDim;
    public int ElementSize => _self->ElementSize;
    public EPropertyFlags PropertyFlags => _self->PropertyFlags;
    public ushort RepIndex => _self->RepIndex;
    public byte BlueprintReplicationCondition => _self->BlueprintReplicationCondition;
    public int Offset_Internal => _self->Offset_Internal;
    public IEnumerable<IFProperty> PropertyLinkNext
        => new IFPropertyEnumerable(_factory.CreateFProperty((nint)_self->PropertyLinkNext), PropertyType.PropertyLink,
            _factory);
    public IEnumerable<IFProperty> NextRef
        => new IFPropertyEnumerable(_factory.CreateFProperty((nint)_self->NextRef), PropertyType.NextRef,
            _factory);
    public IEnumerable<IFProperty> DestructorLinkNext
        => new IFPropertyEnumerable(_factory.CreateFProperty((nint)_self->DestructorLinkNext), PropertyType.DestructorLink,
            _factory);
    public IEnumerable<IFProperty> PostConstructLinkNext
        => new IFPropertyEnumerable(_factory.CreateFProperty((nint)_self->PostConstructLinkNext), PropertyType.PostConstructLink,
            _factory);
    public string RepNotifyFunc => _self->RepNotifyFunc.ToString();
}

public unsafe class IFPropertyEnumerable(IFProperty initial, PropertyType propType, IUnrealFactory factory)
    : IEnumerator<IFProperty>, IEnumerable<IFProperty>
{
    private FProperty* _current = (FProperty*)initial.Ptr;
    private bool isInitial = true;
    
    public bool MoveNext()
    {
        if (isInitial)
        {
            isInitial = false;
            return _current != null;
        }
        
        if (_current == null) return false;
        switch (propType)
        {
            case PropertyType.PropertyLink:
                _current = _current->PropertyLinkNext;
                break;
            case PropertyType.NextRef:
                _current = _current->NextRef;
                break;
            case PropertyType.DestructorLink:
                _current = _current->DestructorLinkNext;
                break;
            case PropertyType.PostConstructLink:
                _current = _current->PostConstructLinkNext;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(propType), propType, null);
        }

        return _current != null;
    }

    public void Reset() => _current = (FProperty*)initial.Ptr;

    public IFProperty Current => factory.CreateFProperty((nint)_current);

    object IEnumerator.Current => Current;

    public void Dispose() { }

    public IEnumerator<IFProperty> GetEnumerator() => this;

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

public unsafe class FBoolProperty_UE5_4_4(nint ptr, IUnrealFactory factory)
    : FProperty_UE5_4_4(ptr, factory), IFBoolProperty
{
    public byte FieldSize => ((FBoolProperty*)ptr)->FieldSize;
    public byte ByteOffset => ((FBoolProperty*)ptr)->ByteOffset;
    public byte ByteMask => ((FBoolProperty*)ptr)->ByteMask;
    public byte FieldMask => ((FBoolProperty*)ptr)->FieldMask;
}

public unsafe class FByteProperty_UE5_4_4(nint ptr, IUnrealFactory factory)
    : FProperty_UE5_4_4(ptr, factory), IFByteProperty
{
    public IUEnum? Enum => ((FByteProperty*)Ptr)->Enum != null ? _factory.CreateUEnum((nint)((FByteProperty*)Ptr)->Enum) : null;
}

public unsafe class UUserDefinedEnum_UE5_4_4(nint ptr, IUnrealFactory factory)
    : UEnum_UE5_4_4(ptr, factory), IUUserDefinedEnum
{
    public TMap<FName, FText> DisplayNameMap => ((UUserDefinedEnum*)Ptr)->DisplayNameMap;
}

public unsafe class UEnum_UE5_4_4(nint ptr, IUnrealFactory factory)
    : UField_UE5_4_4(ptr, factory), IUEnum
{
    private readonly UEnum* _self = (UEnum*)ptr;
    public string CppType => _self->CppType.ToString();
    public TArray<TPair<FName, long>> Names => _self->Names;
}

public unsafe class UScriptStruct_UE5_4_4(nint ptr, IUnrealFactory factory)
    : UStruct_UE5_4_4(ptr, factory), IUScriptStruct
{
    private readonly UScriptStruct* _self = (UScriptStruct*)ptr;
    public EStructFlags StructFlags => _self->StructFlags;
    public bool bPrepareCppStructOpsCompleted => _self->bPrepareCppStructOpsCompleted;
    public nint CppStructOps => _self->CppStructOps;
}

public unsafe class UStruct_UE5_4_4(nint ptr, IUnrealFactory factory)
    : UField_UE5_4_4(ptr, factory), IUStruct
{
    private readonly UStruct* _self = (UStruct*)ptr;

    public IUStruct? SuperStruct
        => _self->SuperStruct != null ? _factory.CreateUStruct((nint)_self->SuperStruct) : null;
    public IEnumerable<IUField> Children
        => new IUFieldEnumerable(_factory.CreateUField((nint)_self->Children));
    public IEnumerable<IFField> ChildProperties =>
        new IFFieldEnumerable(_factory.CreateFField((nint)_self->ChildProperties));
    public int PropertiesSize => _self->PropertiesSize;
    public int MinAlignment => _self->MinAlignment;
    public TArray<byte> Script { get; } = new();

    public IEnumerable<IFProperty> PropertyLink
        => new IFPropertyEnumerable(_factory.CreateFProperty((nint)_self->PropertyLink), PropertyType.PropertyLink,
            _factory);
    
    public IEnumerable<IFProperty> RefLink
        => new IFPropertyEnumerable(_factory.CreateFProperty((nint)_self->RefLink), PropertyType.NextRef,
            _factory);
    public IEnumerable<IFProperty> DestructorLink
        => new IFPropertyEnumerable(_factory.CreateFProperty((nint)_self->DestructorLink), PropertyType.DestructorLink,
            _factory);
    public IEnumerable<IFProperty> PostConstructLink
        => new IFPropertyEnumerable(_factory.CreateFProperty((nint)_self->PostConstructLink), PropertyType.PostConstructLink,
            _factory);
}

public class IFFieldEnumerable(IFField initial)
    : IEnumerator<IFField>, IEnumerable<IFField>
{
    private readonly IFField _initial = initial;
    private IFField? _current = initial;
    private bool isInitial = true;

    public bool MoveNext()
    {
        if (isInitial)
        {
            isInitial = false;
            return _current != null;
        }
        
        _current = _current?.Next;
        return _current != null;
    }

    public void Reset() => _current = _initial;

    public IFField Current => _current!;

    object IEnumerator.Current => Current;

    public void Dispose() { }

    public IEnumerator<IFField> GetEnumerator() => this;

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

public class IUFieldEnumerable(IUField initial)
    : IEnumerator<IUField>, IEnumerable<IUField>
{
    private readonly IUField _initial = initial;
    private IUField? _current = initial;
    private bool isInitial = true;

    public bool MoveNext()
    {
        if (isInitial)
        {
            isInitial = false;
            return _current != null;
        }
        
        _current = _current?.Next;
        return _current != null;
    }

    public void Reset() => _current = _initial;

    public IUField Current => _current!;

    object IEnumerator.Current => Current;

    public void Dispose() { }
    
    public IEnumerator<IUField> GetEnumerator() => this;

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

public unsafe class UField_UE5_4_4(nint ptr, IUnrealFactory factory)
    : UObject_UE5_4_4(ptr, factory), IUField
{
    private readonly UField* _self = (UField*)ptr;

    public IUField? Next
        => _self->Next != null ? _factory.CreateUField((nint)_self->Next) : null;
}

public unsafe class UObject_UE5_4_4(nint ptr, IUnrealFactory factory) : IUObject
{
    private readonly UObjectBase* _self = (UObjectBase*)ptr;
    protected readonly IUnrealFactory _factory = factory;

    public nint Ptr => (nint)_self;

    public nint VTable => _self->VTable;

    public EObjectFlags ObjectFlags => _self->ObjectFlags;

    public int InternalIndex => _self->InternalIndex;

    public IUClass ClassPrivate => _factory.CreateUClass((nint)_self->ClassPrivate);

    public FName NamePrivate => _self->NamePrivate;

    public IUObject? OuterPrivate => _self->OuterPrivate != null ? _factory.CreateUObject((nint)_self->OuterPrivate) : null;

    public bool IsChildOf(string type) => _self->IsChildOf(type);

    public IUObject GetOutermost() => _factory.CreateUObject((nint)_self->GetOutermost());

    public string GetNativeName() => ToolkitUtils.GetNativeName(this);
}

public unsafe class UClass_UE5_4_4(nint ptr, IUnrealFactory factory)
    : UStruct_UE5_4_4(ptr, factory), IUClass
{
    private readonly UClass* _self = (UClass*)ptr;

    public IUClass? GetSuperClass()
        => _self->GetSuperClass() != null ? _factory.CreateUClass((nint)_self->GetSuperClass()) : null;
}

public unsafe class UObjectArray_UE5_4_4(nint ptr, IUnrealFactory factory) : IUObjectArray
{
    private readonly FUObjectArray* _self = (FUObjectArray*)ptr;

    public int ObjLastNonGCIndex => _self->ObjLastNonGCIndex;
    public int NumElements => _self->ObjObjects.NumElements;

    public IUObject? IndexToObject(int idx)
    {
        var objItem = _self->ObjObjects.GetItem(idx);
        if (objItem == null || objItem->Object == null) return null;
        
        return factory.CreateUObject((nint)objItem->Object);
    }

    public void AddToRootSet(int idx)
    {
        var objItem = _self->ObjObjects.GetItem(idx);
        if (objItem == null || objItem->Object == null) return;
        objItem->Flags |= EInternalObjectFlags.RootSet;
        objItem->Object->ObjectFlags |= EObjectFlags.RF_MarkAsRootSet;
    }

    public void RemoveFromRootSet(int idx)
    {
        var objItem = _self->ObjObjects.GetItem(idx);
        if (objItem == null || objItem->Object == null) return;
        objItem->Flags &= ~EInternalObjectFlags.RootSet;
        objItem->Object->ObjectFlags &= ~EObjectFlags.RF_MarkAsRootSet;
    }
}

public class FPropertyParamEnumerator(IFStructParams owner) 
    : IEnumerator<IFPropertyParams>, IEnumerable<IFPropertyParams>
{
    private int CurrentIndex = -1;
    
    #region impl IEnumerator
    
    public bool MoveNext() => ++CurrentIndex < owner.PropertyCount;

    public void Reset() => CurrentIndex = -1;

    public IFPropertyParams Current => owner.GetProperty(CurrentIndex);

    object? IEnumerator.Current => Current;

    public void Dispose() {}
    
    #endregion
    
    #region impl IEnumerable
    
    public IEnumerator<IFPropertyParams> GetEnumerator() => this;
    
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    
    #endregion
}

public unsafe class FStructParamsUE5_4_4(nint ptr, IUnrealFactory factory) : IFStructParams
{
    private readonly FStructParams* _self = (FStructParams*)ptr;
    protected readonly IUnrealFactory _factory = factory;

    public nint Ptr => (nint)_self;

    public nint OuterFunc => _self->OuterFunc;
    public nint SuperFunc => _self->SuperFunc;
    public nint StructOpsFunc => _self->StructOpsFunc;
    public string Name => Marshal.PtrToStringUTF8(_self->NameUTF8)!;
    public ulong Size => _self->SizeOf;
    public ulong Alignment => _self->AlignOf;
    public EObjectFlags ObjectFlags => _self->ObjectFlags;
    public int StructFlags => _self->StructFlags;
    public int PropertyCount => _self->NumProperties;
    public IFPropertyParams? GetProperty(int Index)
    {
        var Result = ((FStructParams*)Ptr)->GetProperty(Index);
        return Result != null ? _factory.CreateFPropertyParams((nint)Result) : null;
    }
    
    public IEnumerable<IFPropertyParams> Properties => new FPropertyParamEnumerator(this);
}

public unsafe class FPropertyParamsUE5_4_4(nint ptr, IUnrealFactory factory) : IFPropertyParams
{
    private readonly FPropertyParamsBase* _self = (FPropertyParamsBase*)ptr;
    protected readonly IUnrealFactory _factory = factory;   
    
    public nint Ptr => (nint)_self;
    public string Name => Marshal.PtrToStringUTF8(_self->NameUTF8)!;
    public EPropertyFlags PropertyFlags => _self->PropertyFlags;
    public EPropertyGenFlags GenFlags => _self->PropertyGenFlags;
    public EObjectFlags ObjectFlags => _self->ObjectFlags;
}