using System.Text;
using UE.Toolkit.Core.Types.Unreal;

namespace UE.Toolkit.Core.Common;

public static unsafe class ToolkitUtils
{
    private static readonly Dictionary<FName, string> PrivateToNativeNameMap = [];

    public static nint GetGlobalAddress(nint address) => *(int*)address + address + 4;

    public static string GetPrivateName<T>(T* objPtr) where T : unmanaged
        => ((UObjectBase*)objPtr)->NamePrivate.ToString();

    /// <summary>
    /// Gets the <c>UObject</c>'s (expected) native name.<br/>
    /// This is different from its private name, which has no type prefix (the <c>U</c> in <c>UObject</c> for example),
    /// typically...
    /// </summary>
    /// <param name="objPtr">UObject instance</param>
    /// <returns><c>UObject</c> native name.</returns>
    public static string GetNativeName<T>(T* objPtr) where T : unmanaged
    {
        var uobj = (UObjectBase*)objPtr;
        if (PrivateToNativeNameMap.TryGetValue(uobj->NamePrivate, out var nameNative)) return nameNative;
        
        var namePrivate = uobj->NamePrivate.ToString();
        nameNative = namePrivate;
        
        if (uobj->IsChildOf<UClass>())
        {
            var uclass = (UClass*)objPtr;
            var classPrefix = 'U';
            
            if (uclass->IsChildOf<AActor>())
            {
                classPrefix = 'A';
            }

            nameNative = $"{classPrefix}{namePrivate}";
        }
        else if (uobj->IsChildOf<UScriptStruct>())
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
}