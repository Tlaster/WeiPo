using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Runtime.CompilerServices;
using WeiPoX.Core.DeclarativeUI.Widgets;

namespace WeiPoX.Core.Routing;

public class BackDispatcher
{
    private readonly List<IBackHandler> _handlers = new();
    private readonly BehaviorSubject<int> _canGoBack = new(0);
    
    public void OnBackPressed()
    {
        _handlers.LastOrDefault(h => h.IsEnabled)?.OnBackPressed();
    }

    public IObservable<bool> CanGoBack => _canGoBack.Select(_ => _handlers.Any(it => it.IsEnabled));
    
    internal void OnBackStackChanged()
    {
        _canGoBack.OnNext(_canGoBack.Value + 1);
    }

    public void Add(IBackHandler handler)
    {
        _handlers.Add(handler);
    }
    
    public void Remove(IBackHandler handler)
    {
        _handlers.Remove(handler);
    }
    
}

public interface IBackHandler
{
    bool IsEnabled { get; }
    void OnBackPressed();
}

public record BackHandler(bool IsEnabled, Action OnBackPressed) : IBackHandler
{
    void IBackHandler.OnBackPressed()
    {
        OnBackPressed();
    }
}

public static class BackHandlerExtension
{
    public static void UseBackHandler(this StatefulWidget widget, bool enabled, Action handler)
    {
        var backDispatcher = widget.UseContext<BackDispatcher>();
        widget.UseEffect(() =>
        {
            var backHandler = new BackHandler(enabled, handler);
            backDispatcher.Add(backHandler);
            return () => backDispatcher.Remove(backHandler);
        }, enabled);
    }
}