using System.Collections.Immutable;
using WeiPoX.Core.DeclarativeUI.Widgets;
using WeiPoX.Core.Lifecycle;

namespace WeiPoX.Core.Routing;

public class Navigator
{
    private bool _isInitialized;
    private readonly RouteStackManager _routeStackManager = new();

    public IObservable<BackstackEntry> CurrentRoute => _routeStackManager.CurrentRoute;
    public IObservable<bool> CanGoBack => _routeStackManager.CanGoBack;

    internal void Init(string initialRoute, ImmutableList<Route> routes, StateHolder stateHolder, LifecycleHolder lifecycleHolder)
    {
        if (_isInitialized)
        {
            return;
        }

        _isInitialized = true;
        _routeStackManager.Init(initialRoute, routes, stateHolder, lifecycleHolder);
    }

    public void Push(string route)
    {
        _routeStackManager.Push(route);
    }

    public void Pop()
    {
        _routeStackManager.Pop();
    }
}

public static class NavigatorExtensions
{
    public static Navigator UseNavigator(this StatefulWidget widget)
    {
        var holder = widget.UseContext<StateHolder>();
        var navigator = holder.GetOrElse("navigator", () => new Navigator());
        return navigator;
    }
}