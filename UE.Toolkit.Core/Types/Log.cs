namespace UE.Toolkit.Core.Types;

public static class DebugLog
{
    static Action<string>? PrintCallback = null;
    static Action<string>? ErrorCallback = null;

    public static void SetPrintCallback(Action<string> Callback) => PrintCallback = Callback;
    public static void SetErrorCallback(Action<string> Callback) => ErrorCallback = Callback;
    public static void Print(string Text)
    {
        if (PrintCallback != null)
        {
            PrintCallback(Text);
        }
    }
    public static void Error(string Text)
    {
        if (ErrorCallback != null)
        {
            ErrorCallback(Text);
        }
    }
}
