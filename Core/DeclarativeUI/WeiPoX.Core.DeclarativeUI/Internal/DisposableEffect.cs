namespace WeiPoX.Core.DeclarativeUI.Internal;

internal record DisposableEffect : IDisposable
{
    private readonly Action _dispose;

    public DisposableEffect(Action dispose)
    {
        _dispose = dispose;
    }

    public void Dispose()
    {
        _dispose();
    }
}