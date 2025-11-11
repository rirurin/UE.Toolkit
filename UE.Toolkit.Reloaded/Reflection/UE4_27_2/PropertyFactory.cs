using System.Runtime.InteropServices;
using UE.Toolkit.Core.Types.Interfaces;
using UE.Toolkit.Core.Types.Unreal.Factories;
using UE.Toolkit.Core.Types.Unreal.Factories.Interfaces;
using UE.Toolkit.Core.Types.Unreal.UE4_27_2;
using UE.Toolkit.Interfaces;
using FField = UE.Toolkit.Core.Types.Unreal.UE4_27_2.FField;
using FName = UE.Toolkit.Core.Types.Unreal.UE5_4_4.FName;
using EFindName = UE.Toolkit.Core.Types.Unreal.UE5_4_4.EFindName;
using EObjectFlags = UE.Toolkit.Core.Types.Unreal.UE5_4_4.EObjectFlags;
using EPropertyFlags = UE.Toolkit.Core.Types.Unreal.UE5_4_4.EPropertyFlags;
using FFieldClass = UE.Toolkit.Core.Types.Unreal.UE4_27_2.FFieldClass;
using FProperty = UE.Toolkit.Core.Types.Unreal.UE4_27_2.FProperty;
using UObjectBase = UE.Toolkit.Core.Types.Unreal.UE4_27_2.UObjectBase;

namespace UE.Toolkit.Reloaded.Reflection.UE4_27_2;

public class PropertyFactory(IUnrealFactory factory, IUnrealMemory memory, IUnrealClasses classes)
    : BasePropertyFactory(factory, memory, classes)
{

    protected override EPropertyFlags CreatePropertyFlags(PropertyVisibility Visibility, PropertyBuilderFlags InFlags)
    {
        var Flags = EPropertyFlags.CPF_Edit | EPropertyFlags.CPF_BlueprintVisible;
        if (InFlags.HasFlag(PropertyBuilderFlags.NoCtor)) Flags |= EPropertyFlags.CPF_ZeroConstructor;
        if (InFlags.HasFlag(PropertyBuilderFlags.Copy)) Flags |= EPropertyFlags.CPF_IsPlainOldData;
        if (InFlags.HasFlag(PropertyBuilderFlags.NoDtor)) Flags |= EPropertyFlags.CPF_NoDestructor;
        if (InFlags.HasFlag(PropertyBuilderFlags.Hash)) Flags |= EPropertyFlags.CPF_HasGetValueTypeHash;
        Flags |= Visibility switch
        {
            PropertyVisibility.Public => EPropertyFlags.CPF_NativeAccessSpecifierPublic,
            PropertyVisibility.Protected => EPropertyFlags.CPF_NativeAccessSpecifierProtected,
            PropertyVisibility.Private => EPropertyFlags.CPF_NativeAccessSpecifierPrivate,
        };
        return Flags;
    }

    protected override unsafe void LinkToPropertyList(IFProperty Property, IUClass Reflect)
    {
        var pProperty = (FProperty*)Property.Ptr;
        pProperty->prop_link_next = null;
        pProperty->next_ref = null;
        pProperty->dtor_link_next = null;
        pProperty->post_ct_link_next = null;
        
        var pClass = (UClass*)Reflect.Ptr;
        if (((UStruct*)pClass)->prop_link == null)
        {
            ((UStruct*)pClass)->prop_link = pProperty;
        }
        else
        {
            var LastProp = Reflect.PropertyLink.Last();
            var LastFProp = (FProperty*)LastProp.Ptr;
            LastFProp->prop_link_next = pProperty;
            ((FField*)LastFProp)->next = (FField*)pProperty;
            /*
            var FirstProp = Reflect.PropertyLink.First();
            var FirstFProp = (FProperty*)FirstProp.Ptr;
            var NextProp = FirstFProp->prop_link_next;
            if (NextProp != null)
                pProperty->prop_link_next = NextProp;
            FirstFProp->prop_link_next = pProperty;
            */
        }       
    }

    protected override unsafe void SetPropertySuperFields(IFField Field, string Name, IUClass ClassReflection, 
        FieldClassGlobal PropertyClass)
    {
        var pField = (FField*)Field.Ptr;
        pField->_vtable = PropertyClass.Vtable;
        pField->class_private = (FFieldClass*)PropertyClass.Params.Ptr;
        pField->owner.Object = (UObjectBase*)ClassReflection.Ptr; // UClass*
        pField->next = null;
        pField->name_private = new FName(Name);
        pField->flags_private = EObjectFlags.RF_Public | EObjectFlags.RF_MarkAsNative | EObjectFlags.RF_Transient;
    }

    protected override unsafe void SetCopyPropertyFields<T>(IFProperty Property, int Offset, PropertyVisibility Visibility)
    {
        var pProperty = (FProperty*)Property.Ptr;
        pProperty->array_dim = 1;
        pProperty->element_size = Marshal.SizeOf<T>();
        var PropertyFlags = PropertyBuilderFlags.NoCtor | PropertyBuilderFlags.Copy | PropertyBuilderFlags.NoDtor;
        pProperty->property_flags = CreatePropertyFlags(Visibility, PropertyFlags);
        pProperty->rep_index = 0;
        pProperty->blueprint_rep_cond = 0;
        pProperty->offset_internal = Offset;
        pProperty->rep_notify_func = new();
    }

    private unsafe bool CreateCopyPropertyInner<TOwner, TField>(out IFProperty? NewProperty,
        string Name, int Offset, string PropertyName, PropertyVisibility Visibility)
        where TOwner : unmanaged
        where TField : unmanaged
    {
        NewProperty = null;
        if (!TryGetClassAndProperty<TOwner>(PropertyName, out var ClassReflection, out var PropertyClass))
            return false;
        var NewFProperty = (FProperty*)Memory.Malloc(Marshal.SizeOf<FProperty>(), FIELD_ALIGNMENT);
        SetPropertySuperFields(Factory.CreateFField((nint)NewFProperty), Name, ClassReflection, PropertyClass);
        NewProperty = Factory.CreateFProperty((nint)NewFProperty);
        SetCopyPropertyFields<TField>(NewProperty, Offset, Visibility);
        LinkToPropertyList(NewProperty, ClassReflection);
        return true;       
    }
     
    public override bool CreateI8<TOwner>(out IFProperty? NewProperty, string Name, int Offset, PropertyVisibility Visibility) 
        => CreateCopyPropertyInner<TOwner, byte>(out NewProperty, Name, Offset, "Int8Property", Visibility);
    
    public override bool CreateI16<TOwner>(out IFProperty? NewProperty, string Name, int Offset, PropertyVisibility Visibility) 
        => CreateCopyPropertyInner<TOwner, short>(out NewProperty, Name, Offset, "Int16Property", Visibility);
    
    public override bool CreateI32<TOwner>(out IFProperty? NewProperty, string Name, int Offset, PropertyVisibility Visibility) 
        => CreateCopyPropertyInner<TOwner, int>(out NewProperty, Name, Offset, "IntProperty", Visibility);
    
    public override bool CreateI64<TOwner>(out IFProperty? NewProperty, string Name, int Offset, PropertyVisibility Visibility) 
        => CreateCopyPropertyInner<TOwner, long>(out NewProperty, Name, Offset, "Int64Property", Visibility);
    
    public override bool CreateU8<TOwner>(out IFProperty? NewProperty, string Name, int Offset, PropertyVisibility Visibility) 
        => CreateCopyPropertyInner<TOwner, byte>(out NewProperty, Name, Offset, "UInt8Property", Visibility);
    
    public override bool CreateU16<TOwner>(out IFProperty? NewProperty, string Name, int Offset, PropertyVisibility Visibility) 
        => CreateCopyPropertyInner<TOwner, short>(out NewProperty, Name, Offset, "UInt16Property", Visibility);
    
    public override bool CreateU32<TOwner>(out IFProperty? NewProperty, string Name, int Offset, PropertyVisibility Visibility) 
        => CreateCopyPropertyInner<TOwner, int>(out NewProperty, Name, Offset, "UInt32Property", Visibility);
    
    public override bool CreateU64<TOwner>(out IFProperty? NewProperty, string Name, int Offset, PropertyVisibility Visibility) 
        => CreateCopyPropertyInner<TOwner, long>(out NewProperty, Name, Offset, "UInt64Property", Visibility);
    
    public override bool CreateF32<TOwner>(out IFProperty? NewProperty, string Name, int Offset, PropertyVisibility Visibility) 
        => CreateCopyPropertyInner<TOwner, float>(out NewProperty, Name, Offset, "FFloatProperty", Visibility);
    
    public override bool CreateF64<TOwner>(out IFProperty? NewProperty, string Name, int Offset, PropertyVisibility Visibility) 
        => CreateCopyPropertyInner<TOwner, double>(out NewProperty, Name, Offset, "FDoubleProperty", Visibility);
}