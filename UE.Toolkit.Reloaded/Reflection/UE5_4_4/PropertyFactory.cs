using System.Runtime.InteropServices;
using UE.Toolkit.Core.Types.Interfaces;
using UE.Toolkit.Core.Types.Unreal.Factories;
using UE.Toolkit.Core.Types.Unreal.Factories.Interfaces;
using UE.Toolkit.Core.Types.Unreal.UE5_4_4;
using UE.Toolkit.Interfaces;

namespace UE.Toolkit.Reloaded.Reflection.UE5_4_4;

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
            _ => EPropertyFlags.CPF_NativeAccessSpecifierPrivate,
        };
        return Flags;
    }

    protected override unsafe void LinkToPropertyList(IFProperty Property, IUClass Reflect)
    {
        var pProperty = (FProperty*)Property.Ptr;
        pProperty->PropertyLinkNext = null;
        pProperty->NextRef = null;
        pProperty->DestructorLinkNext = null;
        pProperty->PostConstructLinkNext = null;
        
        var pClass = (UClass*)Reflect.Ptr;
        if (((UStruct*)pClass)->PropertyLink == null)
        {
            ((UStruct*)pClass)->PropertyLink = pProperty;
        }
        else
        {
            var LastProp = Reflect.PropertyLink.Last();
            var LastFProp = (FProperty*)LastProp.Ptr;
            LastFProp->PropertyLinkNext = pProperty;
        }       
    }
    
    protected override unsafe void SetPropertySuperFields(IFField Field, string Name, IUClass ClassReflection, 
        FieldClassGlobal PropertyClass)
    {
        var pField = (FField*)Field.Ptr;
        pField->VTable = PropertyClass.Vtable;
        pField->ClassPrivate = (FFieldClass*)PropertyClass.Params.Ptr;
        pField->Owner.Object = (UObjectBase*)ClassReflection.Ptr; // UClass*
        pField->Next = null;
        pField->NamePrivate = new FName(Name);
        pField->FlagsPrivate = EObjectFlags.RF_Public | EObjectFlags.RF_MarkAsNative | EObjectFlags.RF_Transient;
    }
    
    private unsafe void SetPropertyFieldDefaults(FProperty* pProperty, int Offset)
    {
        pProperty->RepIndex = 0;
        pProperty->BlueprintReplicationCondition = 0;
        pProperty->Offset_Internal = Offset;
    }
    
    private unsafe void SetPropertyFieldsInner<T>(IFProperty Property, int Offset, 
        PropertyVisibility Visibility, PropertyBuilderFlags PropertyFlags)
    {
        var pProperty = (FProperty*)Property.Ptr;
        pProperty->ArrayDim = 1;
        pProperty->ElementSize = Marshal.SizeOf<T>();
        pProperty->PropertyFlags = CreatePropertyFlags(Visibility, PropertyFlags);
        SetPropertyFieldDefaults(pProperty, Offset);
    }
    
    protected override unsafe void SetCopyPropertyFields<T>(IFProperty Property, int Offset, 
        PropertyVisibility Visibility)
    {
        var PropertyFlags = PropertyBuilderFlags.NoCtor | PropertyBuilderFlags.Copy | PropertyBuilderFlags.NoDtor;
        SetPropertyFieldsInner<T>(Property, Offset, Visibility, PropertyFlags);
    }

    protected override void SetStringPropertyFields<T>(IFProperty Property, int Offset,
        PropertyVisibility Visibility)
        => SetPropertyFieldsInner<T>(Property, Offset, Visibility, PropertyBuilderFlags.NoCtor);
    
    protected override void SetTextPropertyFields<T>(IFProperty Property, int Offset,
        PropertyVisibility Visibility)
        => SetPropertyFieldsInner<T>(Property, Offset, Visibility, PropertyBuilderFlags.None);
    
    protected override unsafe void SetBoolPropertyFields(IFBoolProperty Property, BooleanMask Mask) 
    {
        var pBoolProperty = (FBoolProperty*)Property.Ptr;
        pBoolProperty->FieldSize = (byte)Marshal.SizeOf<byte>();
        pBoolProperty->ByteOffset = 0;
        pBoolProperty->ByteMask = Mask.ByteMask;
        pBoolProperty->FieldMask = Mask.FieldMask;       
    }
    
    public override bool CreateI8<TOwner>(out IFProperty? NewProperty, string Name, int Offset, PropertyVisibility Visibility) 
        => CreateCopyPropertyInner<TOwner, byte, FProperty>(out NewProperty, Name, Offset, "Int8Property", Visibility);
    
    public override bool CreateI16<TOwner>(out IFProperty? NewProperty, string Name, int Offset, PropertyVisibility Visibility) 
        => CreateCopyPropertyInner<TOwner, short, FProperty>(out NewProperty, Name, Offset, "Int16Property", Visibility);
    
    public override bool CreateI32<TOwner>(out IFProperty? NewProperty, string Name, int Offset, PropertyVisibility Visibility) 
        => CreateCopyPropertyInner<TOwner, int, FProperty>(out NewProperty, Name, Offset, "IntProperty", Visibility);
    
    public override bool CreateI64<TOwner>(out IFProperty? NewProperty, string Name, int Offset, PropertyVisibility Visibility) 
        => CreateCopyPropertyInner<TOwner, long, FProperty>(out NewProperty, Name, Offset, "Int64Property", Visibility);
    
    public override bool CreateU8<TOwner>(out IFProperty? NewProperty, string Name, int Offset, PropertyVisibility Visibility) 
        => CreateCopyPropertyInner<TOwner, byte, FProperty>(out NewProperty, Name, Offset, "UInt8Property", Visibility);
    
    public override bool CreateU16<TOwner>(out IFProperty? NewProperty, string Name, int Offset, PropertyVisibility Visibility) 
        => CreateCopyPropertyInner<TOwner, short, FProperty>(out NewProperty, Name, Offset, "UInt16Property", Visibility);
    
    public override bool CreateU32<TOwner>(out IFProperty? NewProperty, string Name, int Offset, PropertyVisibility Visibility) 
        => CreateCopyPropertyInner<TOwner, int, FProperty>(out NewProperty, Name, Offset, "UInt32Property", Visibility);
    
    public override bool CreateU64<TOwner>(out IFProperty? NewProperty, string Name, int Offset, PropertyVisibility Visibility) 
        => CreateCopyPropertyInner<TOwner, long, FProperty>(out NewProperty, Name, Offset, "UInt64Property", Visibility);
    
    public override bool CreateF32<TOwner>(out IFProperty? NewProperty, string Name, int Offset, PropertyVisibility Visibility) 
        => CreateCopyPropertyInner<TOwner, float, FProperty>(out NewProperty, Name, Offset, "FFloatProperty", Visibility);
    
    public override bool CreateF64<TOwner>(out IFProperty? NewProperty, string Name, int Offset, PropertyVisibility Visibility) 
        => CreateCopyPropertyInner<TOwner, double, FProperty>(out NewProperty, Name, Offset, "FDoubleProperty", Visibility);

    public override bool CreateStruct<TOwner, TField>(out IFStructProperty? NewProperty, string Name, int Offset,
        PropertyVisibility Visibility)
    {
        NewProperty = null;
        if (!TryGetClassAndProperty<TOwner>("StructProperty", out var ClassReflection, out var PropertyClass)
            || !Classes.GetScriptStructInfoFromType<TField>(out var ScriptStruct))
            return false;
        var Alloc = Memory.Malloc(Marshal.SizeOf<FStructProperty>(), FIELD_ALIGNMENT);
        NewProperty = Factory.CreateFStructProperty(Alloc);
        SetPropertySuperFields(Factory.CreateFField(Alloc), Name, ClassReflection!, PropertyClass!);
        unsafe
        {
            var pProperty = (FProperty*)Alloc;
            pProperty->ArrayDim = 1;
            pProperty->ElementSize = ScriptStruct!.PropertiesSize; // FExampleStruct mExampleField; 
            pProperty->PropertyFlags = CreatePropertyFlags(Visibility, PropertyBuilderFlags.None);
            SetPropertyFieldDefaults(pProperty, Offset);
        }
        LinkToPropertyList(NewProperty, ClassReflection!);
        unsafe { ((FStructProperty*)NewProperty.Ptr)->Struct = (UScriptStruct*)ScriptStruct.Ptr; }
        return true;
    }

    public override bool CreateObject<TOwner, TField>(out IFObjectProperty? NewProperty, string Name, int Offset,
        PropertyVisibility Visibility)
    {
        NewProperty = null;
        if (!TryGetClassAndProperty<TOwner>("ObjectProperty", out var ClassReflection, out var PropertyClass)
            || !Classes.GetClassInfoFromClass<TField>(out var FieldClass))
            return false;
        var Alloc = Memory.Malloc(Marshal.SizeOf<FObjectProperty>(), FIELD_ALIGNMENT);
        NewProperty = Factory.CreateFObjectProperty(Alloc);
        SetPropertySuperFields(Factory.CreateFField(Alloc), Name, ClassReflection!, PropertyClass!);
        unsafe
        {
            var pProperty = (FProperty*)Alloc;
            pProperty->ArrayDim = 1;
            pProperty->ElementSize = Marshal.SizeOf<nint>();
            // For ObjectProperty:  UExampleClass* pExampleObject;
            // For ClassProperty:  TSubclassOf<class UExampleClass> pExampleClass;
            pProperty->PropertyFlags = CreatePropertyFlags(Visibility, PropertyBuilderFlags.None);
            SetPropertyFieldDefaults(pProperty, Offset);
        }
        LinkToPropertyList(NewProperty, ClassReflection!);
        unsafe { ((FObjectProperty*)NewProperty.Ptr)->PropertyClass = (UClass*)FieldClass!.Ptr; }
        return true;
    }

    public override bool CreateName<TOwner>(out IFProperty? NewProperty, string Name, int Offset, PropertyVisibility Visibility) 
        => CreateCopyPropertyInner<TOwner, FName, FProperty>(out NewProperty, Name, Offset, "NameProperty", Visibility);
    
    public override bool CreateString<TOwner>(out IFProperty? NewProperty, string Name, int Offset, PropertyVisibility Visibility) 
        => CreateStringPropertyInner<TOwner, FName, FProperty>(out NewProperty, Name, Offset, "StringProperty", Visibility);
    
    public override bool CreateText<TOwner>(out IFProperty? NewProperty, string Name, int Offset, PropertyVisibility Visibility) 
        => CreateTextPropertyInner<TOwner, FName, FProperty>(out NewProperty, Name, Offset, "TextProperty", Visibility);

    public override bool CreateArray<TOwner>(out IFArrayProperty? NewProperty, string Name, int Offset, PropertyVisibility Visibility,
        IFProperty Inner)
    {
        NewProperty = null;
        if (!TryGetClassAndProperty<TOwner>("ArrayProperty", out var ClassReflection, out var PropertyClass))
            return false;
        var Alloc = Memory.Malloc(Marshal.SizeOf<FArrayProperty>(), FIELD_ALIGNMENT);
        NewProperty = Factory.CreateFArrayProperty(Alloc);
        SetPropertySuperFields(Factory.CreateFField(Alloc), Name, ClassReflection!, PropertyClass!);
        unsafe
        {
            var pProperty = (FProperty*)Alloc;
            pProperty->ArrayDim = 1;
            pProperty->ElementSize= 0x10; // sizeof(TArray<T>)
            pProperty->PropertyFlags = CreatePropertyFlags(Visibility, PropertyBuilderFlags.NoCtor);
            SetPropertyFieldDefaults(pProperty, Offset);
        }
        LinkToPropertyList(NewProperty, ClassReflection!);
        unsafe { ((FArrayProperty*)Alloc)->Inner = (FProperty*)Inner.Ptr; }
        return true;
    }
}