// ReSharper disable InconsistentNaming

using UE.Toolkit.Reloaded.Common.Types.DataTables;
using UE.Toolkit.Reloaded.Common.Types.Unreal;

namespace UE.Toolkit.Reloaded.DataTables;

public unsafe class DataTablesService
{
    private delegate void HandleDataTableChanged(UDataTable<UObjectBase>* self, FName changedRowName);
    private readonly SHFunction<HandleDataTableChanged>? _HandleDataTableChanged;
    
    public DataTablesService()
    {
        _HandleDataTableChanged = new(HandleDataTableChangedImpl, "40 55 53 57 48 8D 6C 24 ?? 48 81 EC C0 00 00 00 8B 41");
    }
    
    private void HandleDataTableChangedImpl(UDataTable<UObjectBase>* self, FName changedRowName)
    {
        _HandleDataTableChanged!.Hook!.OriginalFunction(self, changedRowName);

        var table = new DataTable<UObjectBase>(self);
        
        Log.Information($"{nameof(HandleDataTableChanged)} || Table: {table.Name}");
        for (int i = 0; i < table.Count; i++)
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
}