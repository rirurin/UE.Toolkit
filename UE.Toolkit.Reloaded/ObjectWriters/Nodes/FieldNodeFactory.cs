using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
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
        
        Log.Debug($"{nameof(FieldNodeFactory)} || Create Node '{fieldName}' with type '{fieldType.Name}'.");
        
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
        
        if (fieldType.Name.StartsWith("TMap"))
        {
            var keyType = fieldType.GetGenericArguments()[0];
            if (keyType == typeof(int))
            {
                return new TMapIntFieldNode(fieldName, fieldPtr, fieldType, this);
            }
            Log.Warning($"{nameof(FieldNodeFactory)} || TODO: Field '{fieldName}' with type '{fieldType.Name}'.");
            foreach (var Generic in fieldType.GenericTypeArguments)
            {
                Log.Warning($"{nameof(FieldNodeFactory)} || TMap Argument: {Generic.Name}");
            }
            return null;
        }

        if (fieldType.IsValueType)
        {
            return new StructFieldNode(fieldName, fieldPtr, fieldType, this);
        }

        Log.Error($"{nameof(FieldNodeFactory)} || Unsupported field '{fieldName}' with type '{fieldType.Name}'.");
        return null;
    }
}