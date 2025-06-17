using UE.Toolkit.Core.Types.Unreal.UE5_4_4;
using UE.Toolkit.Core.Types.Wrappers;
using UE.Toolkit.Interfaces;

// ReSharper disable InconsistentNaming

namespace UE.Toolkit.Reloaded.DataTables;

public unsafe class DataTablesService : IDataTables
{
    private delegate void UDataTable_HandleDataTableChanged(UDataTable<UObjectBase>* self, FName changedRowName);
    private readonly SHFunction<UDataTable_HandleDataTableChanged>? _HandleDataTableChanged;
    
    private Action<UDataTableWrapper<UObjectBase>>? _onDataTableChanged;
    
    public DataTablesService()
    {
        _HandleDataTableChanged = new(HandleDataTableChangedImpl);
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
            Log.Information($"{nameof(UDataTable_HandleDataTableChanged)} || Table: {table.Name} || Struct: {table.RowStructName}");
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