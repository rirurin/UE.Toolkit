using Reloaded.Hooks.ReloadedII.Interfaces;
using System.Collections;
using UE.Toolkit.Core.Common;
using UE.Toolkit.Core.Types.Interfaces;
using UE.Toolkit.Core.Types.Unreal.Factories.Interfaces;
using UE.Toolkit.Core.Types.Unreal.Object;
using UE.Toolkit.Core.Types.Unreal.UE4_27_2;
using EClassFlags = UE.Toolkit.Core.Types.Unreal.UE5_4_4.EClassFlags;
using EObjectFlags = UE.Toolkit.Core.Types.Unreal.UE5_4_4.EObjectFlags;
using EPropertyFlags = UE.Toolkit.Core.Types.Unreal.UE5_4_4.EPropertyFlags;
using EStructFlags = UE.Toolkit.Core.Types.Unreal.UE5_4_4.EStructFlags;
using FFieldObjectUnion = UE.Toolkit.Core.Types.Unreal.UE5_4_4.FFieldObjectUnion;
using FName = UE.Toolkit.Core.Types.Unreal.UE5_4_4.FName;
using FText = UE.Toolkit.Core.Types.Unreal.UE5_4_4.FText;
using FUObjectArray_Pack4 = UE.Toolkit.Core.Types.Unreal.UE5_4_4.FUObjectArray_Pack4;

// ReSharper disable InconsistentNaming

namespace UE.Toolkit.Core.Types.Unreal.Factories.UE4_27_2;

public class UnrealFactory : BaseUnrealFactory
{

    public override IFProperty CreateFProperty(nint ptr) => new FPropertyUE4_27_2(ptr, this);

    public override IFByteProperty CreateFByteProperty(nint ptr) => new FBytePropertyUE4_27_2(ptr, this);

    public override IFEnumProperty CreateFEnumProperty(nint ptr) => new FEnumPropertyUE4_27_2(ptr, this);

    public override IFObjectProperty CreateFObjectProperty(nint ptr) => new FObjectPropertyUE4_27_2(ptr, this);

    public override IFSoftClassProperty CreateFSoftClassProperty(nint ptr) => new FSoftClassPropertyUE4_27_2(ptr, this);

    public override IFClassProperty CreateFClassProperty(nint ptr) => new FClassPropertyUE4_27_2(ptr, this);

    public override IFStructProperty CreateFStructProperty(nint ptr) => new FStructPropertyUE4_27_2(ptr, this);

    public override IFMapProperty CreateFMapProperty(nint ptr) => new FMapPropertyUE4_27_2(ptr, this);

    public override IFInterfaceProperty CreateFInterfaceProperty(nint ptr) => new FInterfacePropertyUE4_27_2(ptr, this);

    public override IFArrayProperty CreateFArrayProperty(nint ptr) => new FArrayPropertyUE4_27_2(ptr, this);

    public override IFSetProperty CreateFSetProperty(nint ptr) => new FSetPropertyUE4_27_2(ptr, this);

    public override IFOptionalProperty CreateFOptionalProperty(nint ptr) => throw new NotSupportedException();
    
    public override IUObjectArray CreateUObjectArray(nint ptr) => new UObjectArrayUE4_27_2(ptr, this);

    public override IUObject CreateUObject(nint ptr) => new UObjectUE4_27_2(ptr, this);

    public override IUClass CreateUClass(nint ptr) => new UClassUE4_27_2(ptr, this);

    public override IUScriptStruct CreateUScriptStruct(nint ptr) => new UScriptStructUE4_27_2(ptr, this);

    public override IUEnum CreateUEnum(nint ptr) => new UEnumUE4_27_2(ptr, this);

    public override IUField CreateUField(nint ptr) => new UFieldUE4_27_2(ptr, this);

    public override IUStruct CreateUStruct(nint ptr) => new UStructUE4_27_2(ptr, this);

    public override IUUserDefinedEnum CreateUUserDefinedEnum(nint ptr) => new UUserDefinedEnumUE4_27_2(ptr, this);

    public override IFFieldClass CreateFFieldClass(nint ptr) => new FFieldClassUE4_27_2(ptr, this);

    public override IFField CreateFField(nint ptr) => new FFieldUE4_27_2(ptr, this);


    public UnrealFactory(IUnrealMemoryInternal allocator, IReloadedHooks hooks) : base(allocator, hooks) { }
}

public unsafe class FSetPropertyUE4_27_2(nint ptr, IUnrealFactory factory)
    : FPropertyUE4_27_2(ptr, factory), IFSetProperty
{
    public IFProperty ElementProp => _factory.CreateFProperty((nint)((FSetProperty*)Ptr)->elem_prop);
}

public unsafe class FArrayPropertyUE4_27_2(nint ptr, IUnrealFactory factory)
    : FPropertyUE4_27_2(ptr, factory), IFArrayProperty
{
    public IFProperty Inner => _factory.CreateFProperty((nint)((FArrayProperty*)Ptr)->inner);
}

public unsafe class FInterfacePropertyUE4_27_2(nint ptr, IUnrealFactory factory)
    : FPropertyUE4_27_2(ptr, factory), IFInterfaceProperty
{
    public IUClass InterfaceClass => _factory.CreateUClass((nint)((FObjectProperty*)Ptr)->prop_class);
}

public unsafe class FMapPropertyUE4_27_2(nint ptr, IUnrealFactory factory)
    : FPropertyUE4_27_2(ptr, factory), IFMapProperty
{
    public IFProperty KeyProp => _factory.CreateFProperty((nint)((FMapProperty*)Ptr)->key_prop);
    public IFProperty ValueProp => _factory.CreateFProperty((nint)((FMapProperty*)Ptr)->value_prop);
}

public unsafe class FStructPropertyUE4_27_2(nint ptr, IUnrealFactory factory)
    : FPropertyUE4_27_2(ptr, factory), IFStructProperty
{
    public IUScriptStruct Struct => _factory.CreateUScriptStruct((nint)((FStructProperty*)Ptr)->struct_data);
}

public unsafe class FClassPropertyUE4_27_2(nint ptr, IUnrealFactory factory)
    : FPropertyUE4_27_2(ptr, factory), IFClassProperty
{
    public IUClass PropertyClass => _factory.CreateUClass((nint)((FClassProperty*)Ptr)->_super.prop_class);
    public IUClass? MetaClass => _factory.CreateUClass((nint)((FClassProperty*)Ptr)->meta);
}

public unsafe class FSoftClassPropertyUE4_27_2(nint ptr, IUnrealFactory factory)
    : FPropertyUE4_27_2(ptr, factory), IFSoftClassProperty
{
    public IUClass MetaClass => _factory.CreateUClass((nint)((FClassProperty*)Ptr)->meta);
}

public unsafe class FObjectPropertyUE4_27_2(nint ptr, IUnrealFactory factory)
    : FPropertyUE4_27_2(ptr, factory), IFObjectProperty
{
    public IUClass PropertyClass => _factory.CreateUClass((nint)((FObjectProperty*)Ptr)->prop_class);
}

public unsafe class FEnumPropertyUE4_27_2(nint ptr, IUnrealFactory factory)
    : FPropertyUE4_27_2(ptr, factory), IFEnumProperty
{
    private readonly FEnumProperty* _self = (FEnumProperty*)ptr;

    public IUEnum Enum => _factory.CreateUEnum((nint)_self->enum_data);
    public IFProperty UnderlyingProp => _factory.CreateFProperty((nint)_self->underlying_type);
}

public unsafe class FFieldClassUE4_27_2(nint ptr, IUnrealFactory factory)
    : IFFieldClass
{
    private readonly FFieldClass* _self = (FFieldClass*)ptr;

    public string Name => _self->name.ToString();
    public ulong Id => throw new NotImplementedException();
    public ulong CastFlags => throw new NotImplementedException();
    public EClassFlags ClassFlags => throw new NotImplementedException();
    public IFFieldClass SuperClass => factory.CreateFFieldClass((nint)_self->super);
    public IFField DefaultObject => factory.CreateFField((nint)_self->default_obj);
    public nint FieldConstructor => _self->ctor;
}

public unsafe class FFieldUE4_27_2(nint ptr, IUnrealFactory factory)
    : IFField
{
    private readonly FField* _self = (FField*)ptr;
    protected readonly IUnrealFactory _factory = factory;

    public nint Ptr => ptr;
    public nint VTable => _self->_vtable;
    public IFFieldClass ClassPrivate => _factory.CreateFFieldClass((nint)_self->class_private);
    public FFieldObjectUnion Owner => throw new NotSupportedException();
    public IFField? Next => _self->next != null ? _factory.CreateFField((nint)_self->next) : null;
    public string NamePrivate => _self->name_private.ToString();
    public EObjectFlags FlagsPrivate => _self->flags_private;
}

public unsafe class FPropertyUE4_27_2(nint ptr, IUnrealFactory factory)
    : FFieldUE4_27_2(ptr, factory), IFProperty
{
    private readonly FProperty* _self = (FProperty*)ptr;

    public int ArrayDim => _self->array_dim;
    public int ElementSize => _self->element_size;
    public EPropertyFlags PropertyFlags => _self->property_flags;
    public ushort RepIndex => _self->rep_index;
    public byte BlueprintReplicationCondition => _self->blueprint_rep_cond;
    public int Offset_Internal => _self->offset_internal;
    public IEnumerable<IFProperty> PropertyLinkNext
        => new IFPropertyEnumerable(_factory.CreateFProperty((nint)_self->prop_link_next), PropertyType.PropertyLink,
            _factory);
    public IEnumerable<IFProperty> NextRef
        => new IFPropertyEnumerable(_factory.CreateFProperty((nint)_self->next_ref), PropertyType.NextRef,
            _factory);
    public IEnumerable<IFProperty> DestructorLinkNext
        => new IFPropertyEnumerable(_factory.CreateFProperty((nint)_self->dtor_link_next), PropertyType.DestructorLink,
            _factory);
    public IEnumerable<IFProperty> PostConstructLinkNext
        => new IFPropertyEnumerable(_factory.CreateFProperty((nint)_self->post_ct_link_next), PropertyType.PostConstructLink,
            _factory);
    public string RepNotifyFunc => _self->rep_notify_func.ToString();
}

public unsafe class IFPropertyEnumerable(IFProperty initial, PropertyType propType, IUnrealFactory factory)
    : IEnumerator<IFProperty>, IEnumerable<IFProperty>
{
    private FProperty* _current = (FProperty*)initial.Ptr;
    private bool _isInitial = true;
    
    public bool MoveNext()
    {
        if (_isInitial)
        {
            _isInitial = false;
            return _current != null;
        }
        
        if (_current == null) return false;
        switch (propType)
        {
            case PropertyType.PropertyLink:
                _current = _current->prop_link_next;
                break;
            case PropertyType.NextRef:
                _current = _current->next_ref;
                break;
            case PropertyType.DestructorLink:
                _current = _current->dtor_link_next;
                break;
            case PropertyType.PostConstructLink:
                _current = _current->post_ct_link_next;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(propType), propType, null);
        }

        return _current != null;
    }

    public void Reset() => _current = (FProperty*)initial.Ptr;

    public IFProperty Current => factory.CreateFProperty((nint)_current);

    object? IEnumerator.Current => Current;

    public void Dispose() { }

    public IEnumerator<IFProperty> GetEnumerator() => this;

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

public unsafe class FBytePropertyUE4_27_2(nint ptr, IUnrealFactory factory)
    : FPropertyUE4_27_2(ptr, factory), IFByteProperty
{
    public IUEnum? Enum => ((FByteProperty*)Ptr)->enum_data != null ? _factory.CreateUEnum((nint)((FByteProperty*)Ptr)->enum_data) : null;
}

public unsafe class UUserDefinedEnumUE4_27_2(nint ptr, IUnrealFactory factory)
    : UEnumUE4_27_2(ptr, factory), IUUserDefinedEnum
{
    public UE.Toolkit.Core.Types.Unreal.UE5_4_4.TMap<FName, FText> DisplayNameMap => ((UUserDefinedEnum*)ptr)->DisplayNameMap;
}

public unsafe class UEnumUE4_27_2(nint ptr, IUnrealFactory factory)
    : UFieldUE4_27_2(ptr, factory), IUEnum
{
    private readonly UEnum* _self = (UEnum*)ptr;
    public string CppType => _self->cpp_type.ToString();
    public UE.Toolkit.Core.Types.Unreal.UE5_4_4.TArray<UE.Toolkit.Core.Types.Unreal.UE5_4_4.TPair<FName, long>> Names => _self->entries;
}

public unsafe class UScriptStructUE4_27_2(nint ptr, IUnrealFactory factory)
    : UStructUE4_27_2(ptr, factory), IUScriptStruct
{
    private readonly UScriptStruct* _self = (UScriptStruct*)ptr;
    public EStructFlags StructFlags => (EStructFlags)_self->flags;
    public bool bPrepareCppStructOpsCompleted => _self->b_prepare_cpp_struct_ops_completed;
    public nint CppStructOps => _self->cpp_struct_ops;
}

public unsafe class UStructUE4_27_2(nint ptr, IUnrealFactory factory)
    : UFieldUE4_27_2(ptr, factory), IUStruct
{
    private readonly UStruct* _self = (UStruct*)ptr;

    public IUStruct? SuperStruct
        => _self->super_struct != null ? _factory.CreateUStruct((nint)_self->super_struct) : null;
    public IEnumerable<IUField> Children
        => new IUFieldEnumerable(_factory.CreateUField((nint)_self->children));
    public IEnumerable<IFField>? ChildProperties
        => _self->child_properties != null ? new IFFieldEnumerable(_factory.CreateFField((nint)_self->child_properties)) : null;
    public int PropertiesSize => _self->properties_size;
    public int MinAlignment => _self->min_alignment;
    public UE.Toolkit.Core.Types.Unreal.UE5_4_4.TArray<byte> Script { get; } = new();

    public IEnumerable<IFProperty> PropertyLink
        => new IFPropertyEnumerable(_factory.CreateFProperty((nint)_self->prop_link), PropertyType.PropertyLink,
            _factory);
    
    public IEnumerable<IFProperty> RefLink
        => new IFPropertyEnumerable(_factory.CreateFProperty((nint)_self->ref_link), PropertyType.NextRef,
            _factory);
    public IEnumerable<IFProperty> DestructorLink
        => new IFPropertyEnumerable(_factory.CreateFProperty((nint)_self->dtor_link), PropertyType.DestructorLink,
            _factory);
    public IEnumerable<IFProperty> PostConstructLink
        => new IFPropertyEnumerable(_factory.CreateFProperty((nint)_self->post_ct_link), PropertyType.PostConstructLink,
            _factory);
}

public class IFFieldEnumerable(IFField initial)
    : IEnumerator<IFField>, IEnumerable<IFField>
{
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

    public void Reset() => _current = initial;

    public IFField Current => _current!;

    object? IEnumerator.Current => Current;

    public void Dispose() { }

    public IEnumerator<IFField> GetEnumerator() => this;

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

public class IUFieldEnumerable(IUField initial)
    : IEnumerator<IUField>, IEnumerable<IUField>
{
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

    public void Reset() => _current = initial;

    public IUField Current => _current!;

    object? IEnumerator.Current => Current;

    public void Dispose() { }
    
    public IEnumerator<IUField> GetEnumerator() => this;

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

public unsafe class UFieldUE4_27_2(nint ptr, IUnrealFactory factory)
    : UObjectUE4_27_2(ptr, factory), IUField
{
    private readonly UField* _self = (UField*)ptr;

    public IUField? Next
        => _self->next != null ? _factory.CreateUField((nint)_self->next) : null;
}

public unsafe class UObjectUE4_27_2(nint ptr, IUnrealFactory factory) : IUObject
{
    private readonly UObjectBase* _self = (UObjectBase*)ptr;
    protected readonly IUnrealFactory _factory = factory;

    public nint Ptr => (nint)_self;

    public nint VTable => _self->_vtable;

    public EObjectFlags ObjectFlags => _self->ObjectFlags;

    public int InternalIndex => (int)_self->InternalIndex;

    public IUClass ClassPrivate => _factory.CreateUClass((nint)_self->ClassPrivate);

    public FName NamePrivate => _self->NamePrivate;

    public IUObject? OuterPrivate => _self->OuterPrivate != null ? _factory.CreateUObject((nint)_self->OuterPrivate) : null;

    public bool IsChildOf(string type) => _self->IsChildOf(type);

    public IUObject GetOutermost() => _factory.CreateUObject((nint)_self->GetOutermost());

    public string GetNativeName() => ToolkitUtils.GetNativeName(this);

    public TReturnType ProcessEvent<TReturnType>(string funcName, params ProcessEventParameterBase[] param) where TReturnType : unmanaged
        => ToolkitUtils.ProcessEvent<TReturnType>(this, factory, funcName, param);
}

public unsafe class UClassUE4_27_2(nint ptr, IUnrealFactory factory)
    : UStructUE4_27_2(ptr, factory), IUClass
{
    private readonly UClass* _self = (UClass*)ptr;

    public Unreal.UE5_4_4.TMapDictionary<FName, Ptr<Unreal.UE5_4_4.UFunction>> GetFunctionMap()
        => new((Unreal.UE5_4_4.TMap<FName, Ptr<Unreal.UE5_4_4.UFunction>>*)(&_self->func_map), _factory.GetAllocator());

    public IUClass? GetSuperClass()
        => _self->_super.super_struct != null ? _factory.CreateUClass((nint)_self->_super.super_struct) : null;
}

public unsafe class UObjectArrayUE4_27_2(nint ptr, IUnrealFactory factory) : IUObjectArray
{
    private readonly FUObjectArray_Pack4* _self = (FUObjectArray_Pack4*)ptr;

    public int ObjLastNonGCIndex => _self->ObjLastNonGCIndex;

    public IUObject? IndexToObject(int idx)
    {
        var objItem = _self->ObjObjects.GetItem(idx);
        if (objItem == null || objItem->Object == null) return null;
        
        return factory.CreateUObject((nint)objItem->Object);
    }

    public int NumElements => _self->ObjObjects.NumElements;
}
