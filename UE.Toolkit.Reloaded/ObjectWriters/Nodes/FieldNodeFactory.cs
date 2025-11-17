using System.Diagnostics.CodeAnalysis;
using UE.Toolkit.Core.Types.Interfaces;
using UE.Toolkit.Core.Types.Unreal.UE5_4_4;
using UE.Toolkit.Interfaces.ObjectWriters;

namespace UE.Toolkit.Reloaded.ObjectWriters.Nodes;

public class FieldNodeFactory(ITypeRegistry typeReg, IObjectCreator objCreator, IUnrealMemoryInternal memory)
{
    public ITypeRegistry TypeRegistry { get; } = typeReg;
    public IUnrealMemoryInternal Memory { get; } = memory;
    
    public bool TryCreate(string fieldName, nint fieldPtr, Type fieldType, [NotNullWhen(true)]out IFieldNode? node)
    {
        node = Create(fieldName, fieldPtr, fieldType);
        return node != null;
    }

    private IFieldNode? Create(string fieldName, nint fieldPtr, Type fieldType)
    {
        if (fieldType.IsPrimitive
            || fieldType.IsEnum
            || fieldType == typeof(string)
            || fieldType == typeof(FText)
            || fieldType == typeof(FString)
            || fieldType == typeof(FName))
        {
            return new PrimitiveFieldNode(fieldName, fieldPtr, fieldType, objCreator);
        }

        if (fieldType.Name.StartsWith("TArray"))
        {
            return new TArrayFieldNode(fieldName, fieldPtr, fieldType, this);
        }

        if (fieldType.Name.StartsWith("UDataTable"))
        {
            return new DataTableFieldNode(fieldName, fieldPtr, fieldType, this);
        }

        if (fieldType.IsValueType)
        {
            return new StructFieldNode(fieldName, fieldPtr, fieldType, this);
        }

        Log.Error($"{nameof(FieldNodeFactory)} || Unsupported field '{fieldName}' with type '{fieldType.Name}'.");
        return null;
    }
}