using System.Diagnostics.CodeAnalysis;
using UE.Toolkit.Interfaces.ObjectWriters;

namespace UE.Toolkit.Reloaded.ObjectWriters;

public class TypeRegistry : ITypeRegistry
{
    private readonly Dictionary<string, ITypeProvider> _providers = [];
    
    public void RegisterProvider(ITypeProvider provider)
    {
        if (_providers.TryAdd(provider.Id, provider))
        {
            Log.Information($"{nameof(ObjectWriterService)} || Type provider registered: {provider.Id}");
        }
        else
        {
            Log.Warning($"{nameof(ObjectWriterService)} || Failed to register type provider with duplicate ID: {provider.Id}");
        }
    }

    public bool TryGetType(string typeName, string? typeHint, string? providerId, [NotNullWhen(true)] out Type? type)
    {
        const string typeObj = WriterConstants.HintAttrObject;
        const string typeActor = WriterConstants.HintAttrActor;
        const string typeStruct = WriterConstants.HintAttrStruct;

        // Try resolving with type hint, if provided.
        if (!string.IsNullOrEmpty(typeHint))
        {
            switch (typeHint)
            {
                case typeStruct when TryGetType($"F{typeName}", providerId, out type):
                case typeActor when TryGetType($"A{typeName}", providerId, out type):
                case typeObj when TryGetType($"U{typeName}", providerId, out type):
                    return true;
            }
        }
        
        // Try resolving by name directly.
        if (TryGetType(typeName, providerId, out type)) return true;
        
        // Fallback to trying every prefix...
        if (TryGetType($"F{typeName}", providerId, out type)) return true;
        if (TryGetType($"A{typeName}", providerId, out type)) return true;
        if (TryGetType($"U{typeName}", providerId, out type)) return true;

        return false;
    }

    public bool TryGetType(string typeName, string? providerId, [NotNullWhen(true)] out Type? type)
    {
        type = null;
        if (providerId == null)
        {
            type = _providers.Values.FirstOrDefault(x => x.CanProvide(typeName))?.GetType(typeName);
        }
        else if (_providers.TryGetValue(providerId, out var provider) && provider.CanProvide(typeName))
        {
            type = provider.GetType(typeName);
        }

        return type != null;
    }
}
