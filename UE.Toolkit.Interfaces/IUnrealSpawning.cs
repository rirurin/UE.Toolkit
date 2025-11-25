using UE.Toolkit.Core.Types.Unreal.Factories.Interfaces;

namespace UE.Toolkit.Interfaces;

public interface IUnrealSpawning
{
    IUObject? SpawnObject<TObject>(string Name, IUObject? Owner) where TObject : unmanaged;
    
    IUObject? SpawnObject(string Name, IUClass Class, IUObject? Owner);
    
    /*

    IUObject? SpawnActor<TObject>(string Name) where TObject : unmanaged;

    IUObject? SpawnActor(string Name, IUClass Class);
    
    IUObject? SpawnActor<TObject>(string Name, IUObject World) where TObject : unmanaged;

    IUObject? SpawnActor(string Name, IUClass Class, IUObject World);
    
    */
}