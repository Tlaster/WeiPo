namespace WeiPoX.Core.DeclarativeUI.Widgets;

internal record Memo<T>(T Value, object[] Dependencies) : IDisposable
{
    public void Dispose()
    {
        if (Value is IDisposable disposable)
        {
            disposable.Dispose();
        }
    }
}