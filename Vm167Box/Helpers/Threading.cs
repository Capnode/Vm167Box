namespace Vm167Box.Helpers;

public class Threading
{
    public static void RunOnMainThread(Action action)
    {
        if (MainThread.IsMainThread)
        {
            action.Invoke();
        }
        else
        {
            MainThread.BeginInvokeOnMainThread(action);
        }
    }
}
