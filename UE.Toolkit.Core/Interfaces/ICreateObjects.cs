using UE.Toolkit.Core.Types.Unreal;

namespace UE.Toolkit.Core.Interfaces;

public unsafe interface ICreateObjects
{
    FText* CreateFText(string str);
    
    FString* CreateFString(string str);
}