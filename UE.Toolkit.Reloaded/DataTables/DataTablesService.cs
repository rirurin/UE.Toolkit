// ReSharper disable InconsistentNaming

using UE.Toolkit.Interfaces;
using UE.Toolkit.Interfaces.Common.Types.Unreal;
using UE.Toolkit.Interfaces.Common.Types.Wrappers;

namespace UE.Toolkit.Reloaded.DataTables;

public unsafe class DataTablesService : IDataTables
{
    private delegate void HandleDataTableChanged(UDataTable<UObjectBase>* self, FName changedRowName);
    private readonly SHFunction<HandleDataTableChanged>? _HandleDataTableChanged;
    
    private Action<UDataTableWrapper<UObjectBase>>? _onDataTableChanged;
    
    public DataTablesService()
    {
        _HandleDataTableChanged = new(HandleDataTableChangedImpl, "40 55 53 57 48 8D 6C 24 ?? 48 81 EC C0 00 00 00 8B 41");
    }

    public void OnDataTableChanged<TRow>(string name, Action<UDataTableWrapper<TRow>> callback)
        where TRow : unmanaged
    {
        _onDataTableChanged += table =>
        {
            if (table.Name == name) callback(new((UDataTable<TRow>*)table.Instance));
        };
    }
    
    private void HandleDataTableChangedImpl(UDataTable<UObjectBase>* self, FName changedRowName)
    {
        _HandleDataTableChanged!.Hook!.OriginalFunction(self, changedRowName);
        if (self->BaseObj.NamePrivate.ToString() == "None") return;

        var table = new UDataTableWrapper<UObjectBase>(self);
        
        if (Mod.Config.LogTablesEnabled)
        {
            Log.Information($"{nameof(HandleDataTableChanged)} || Table: {table.Name} || Struct: {table.RowStructName}");
            for (var i = 0; i < table.Count; i++)
            {
                var row = table[i];
                switch (i)
                {
                    case < 5:
                        Log.Information($"\t{row.Name}");
                        break;
                    case 5:
                        Log.Information($"\t...with {table.Count - i} more.");
                        break;
                }
            }
        }
        
        _onDataTableChanged?.Invoke(table);
    }
}