using System.Collections.Immutable;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using WeiPoX.Core.DeclarativeUI.Widgets;
using WeiPoX.Core.Lifecycle;

namespace WeiPoX.Core.DeclarativeUI.Foundation;

public class BackDispatcher
{
    private readonly BehaviorSubject<ImmutableList<IBackHandler>> _handlers = new(ImmutableList<IBackHandler>.Empty);

    public BackDispatcher()
    {
        CanGoBack = _handlers.Select(list => list.Any(handler => handler.IsEnabled));
    }

    public IObservable<bool> CanGoBack { get; }

    public bool OnBackPressed()
    {
        foreach (var handler in _handlers.Value.Where(handler => handler.IsEnabled))
        {
            handler.OnBackPressed();
            return true;
        }

        return false;
    }

    public void Add(IBackHandler handler)
    {
        _handlers.OnNext(_handlers.Value.Add(handler));
    }

    public void Remove(IBackHandler handler)
    {
        _handlers.OnNext(_handlers.Value.Remove(handler));
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
        var lifecycleHolder = widget.UseContext<LifecycleHolder>();
        widget.UseEffect(() =>
        {
            var backHandler = new BackHandler(enabled, handler);
            backDispatcher.Add(backHandler);
            return () => backDispatcher.Remove(backHandler);
        }, enabled, lifecycleHolder);
    }
}