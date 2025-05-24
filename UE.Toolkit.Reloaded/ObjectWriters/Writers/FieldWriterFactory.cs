using UE.Toolkit.Core.Types.Interfaces;
using UE.Toolkit.Core.Types.Unreal;

namespace UE.Toolkit.Reloaded.ObjectWriters.Writers;

public static class FieldWriterFactory
{
    public static IFieldWriter? Create(string fieldName, nint fieldPtr, Type fieldType, IObjectCreator objCreator)
    {
        if (fieldType.IsPrimitive || fieldType.Name == nameof(String))
            return new PrimitiveFieldWriter(fieldName, fieldPtr, fieldType);

        if (fieldType == typeof(FText) || fieldType == typeof(FString))
            return new TextFieldWriter(fieldName, fieldPtr, fieldType, objCreator);
        
        Log.Error($"{nameof(FieldWriterFactory)} || No writer found for field '{fieldName}' of type '{fieldType.Name}'.");
        return null;
    }

    private class DummyFieldWriter : IFieldWriter
    {
        public void Reset()
        {
        }

        public void SetField(string value)
        {
        }

        public void SetField<TValue>(TValue value) where TValue : unmanaged
        {
        }
    }
}