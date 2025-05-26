using System.Runtime.InteropServices;
using UE.Toolkit.Core.Types.Interfaces;
using UE.Toolkit.Core.Types.Unreal.UE5_4_4;

namespace UE.Toolkit.Reloaded.ObjectWriters.Writers;

public unsafe class TextFieldWriter(string fieldName, nint fieldPtr, Type fieldType, IObjectCreator objCreator) : IFieldWriter
{
    private byte[]? _ogData;
    
    public void Reset()
    {
        if (_ogData != null) Marshal.Copy(_ogData, 0, fieldPtr, _ogData.Length);
    }

    public void SetField(string value)
    {
        var strPtr = Marshal.StringToHGlobalUni(value);
        SetField(strPtr);
        Marshal.FreeHGlobal(strPtr);
    }

    public void SetField<TValue>(TValue value) where TValue : unmanaged
    {
        var strValue = Marshal.PtrToStringUni(*(nint*)&value)!;
        switch (fieldType.Name)
        {
            case nameof(FText):
                _ogData = new byte[sizeof(FText)];
                Marshal.Copy(fieldPtr, _ogData, 0, sizeof(FText));
                var ftext = objCreator.CreateFText(strValue);
                *(FText*)fieldPtr = *ftext;
                break;
            case nameof(FString):
                _ogData = new byte[sizeof(FString)];
                Marshal.Copy(fieldPtr, _ogData, 0, sizeof(FString));
                var fstring = objCreator.CreateFString(strValue);
                *(FString*)fieldPtr = *fstring;
                break;
            default:
                Log.Error($"{nameof(TextFieldWriter)} || Invalid type '{fieldType.Name}' for field '{fieldName}'.");
                return;
        }
        
        Log.Debug($"{nameof(TextFieldWriter)} || Field '{fieldName}' at 0x{fieldPtr:X} set to: {strValue}");
    }
}
