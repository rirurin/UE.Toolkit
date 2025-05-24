namespace UE.Toolkit.Reloaded.ObjectWriters;

public static class WriterConstants
{
    public const string ItemTag = "Item";
    public const string ItemIdAttr = "id";
    
    public const string HintAttr = "hint";
    public const string HintAttrObject = "object";
    public const string HintAttrActor = "actor";
    public const string HintAttrStruct = "struct";
    
    public const string RowStructAttr = "row-struct";
    public const string RowStructHintAttr = $"row-struct-{HintAttr}";
    public const string RowStructProviderAttr = "row-struct-provider";
}