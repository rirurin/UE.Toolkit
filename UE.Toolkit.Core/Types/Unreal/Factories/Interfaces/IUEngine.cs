namespace UE.Toolkit.Core.Types.Unreal.Factories.Interfaces;

public interface IUEngine : IUObject
{
    public IEnumerable<IFWorldContext> GetWorldList();
}