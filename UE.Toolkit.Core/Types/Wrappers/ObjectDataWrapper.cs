using System.Runtime.InteropServices;
using UE.Toolkit.Core.Interfaces;
using UE.Toolkit.Core.Types.Unreal;

namespace UE.Toolkit.Core.Types.Wrappers;

public unsafe class ObjectDataWrapper<TObject> where TObject : unmanaged
{
    private readonly Dictionary<string, (Type Type, nint Offset)> _fields = [];
    private readonly TObject* _obj;
    private readonly ICreateObjects _objCreator;

    public ObjectDataWrapper(TObject* obj, ICreateObjects objCreator)
    {
        _obj = obj;
        _objCreator = objCreator;

        var type = typeof(TObject);
        foreach (var field in type.GetFields())
        {
            if (field.FieldType.IsGenericType) continue;
            
            var fieldName = field.Name;
            var fieldOffset = Marshal.OffsetOf<TObject>(fieldName);
            _fields[fieldName] = (field.FieldType, fieldOffset);
        }
    }

    public void SetField(string field, string str)
    {
        var strPtr = Marshal.StringToHGlobalUni(str);
        SetField(field, strPtr);
        Marshal.FreeHGlobal(strPtr);
    }

    public void SetField<TValue>(string field, TValue value)
        where TValue : unmanaged
    {
        if (_fields.TryGetValue(field, out var fieldInfo))
        {
            var fieldPtr = (nint)_obj + fieldInfo.Offset;
            switch (fieldInfo.Type.Name)
            {
                case "Int64":
                case "UInt64":
                    *(nint*)fieldPtr = *(nint*)&value;
                    break;
                case "Int32":
                case "UInt32":
                    *(int*)fieldPtr = *(int*)&value;
                    break;
                case "Int16":
                case "UInt16":
                    *(short*)fieldPtr = *(short*)&value;
                    break;
                case "Int8":
                case "UInt8":
                    *(byte*)fieldPtr = *(byte*)&value;
                    break;
                case "Single":
                    *(float*)fieldPtr = Convert.ToSingle(value);;
                    break;
                case "Double":
                    *(double*)fieldPtr = Convert.ToDouble(value);
                    break;
                case nameof(FText):
                    var ftext = _objCreator.CreateFText(Marshal.PtrToStringUni(*(nint*)&value)!);
                    *(FText*)fieldPtr = *ftext;
                    break;
                case nameof(FString):
                    var fstring = _objCreator.CreateFString(Marshal.PtrToStringUni(*(nint*)&value)!);
                    *(FString*)fieldPtr = *fstring;
                    break;
                default:
                    *(byte*)fieldPtr = *(byte*)&value;
                    break;
            }
            
            //Log.Information($"{nameof(ObjectDataWrapper<byte>)} || Set {field} to {value} || {fieldInfo.Item2} bytes to 0x{fieldPtr:X}");
        }
    }
}