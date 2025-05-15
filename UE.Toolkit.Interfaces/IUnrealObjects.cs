using UE.Toolkit.Interfaces.Common.Types.Unreal;

namespace UE.Toolkit.Interfaces;

public interface IUnrealObjects
{
    unsafe FText* CreateFText(string str);
    unsafe FString* CreateFString(string str);
}