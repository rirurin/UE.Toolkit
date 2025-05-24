namespace UE.Toolkit.Reloaded.ObjectWriters.Writers;

public interface IFieldWriter
{
    void Reset();
    
    void SetField(string value);

    void SetField<TValue>(TValue value)
        where TValue : unmanaged;
}