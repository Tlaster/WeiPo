using System.Collections.Immutable;
using WeiPoX.Core.DeclarativeUI.Widgets;
using WeiPoX.Core.Lifecycle;

namespace WeiPoX.Core.Routing;

public class Navigator
{
    private readonly RouteStackManager _routeStackManager = new();

    public IObservable<BackstackEntry> CurrentRoute => _routeStackManager.CurrentRoute;
    public IObservable<bool> CanGoBack => _routeStackManager.CanGoBack;

    internal void Init(string initialRoute, ImmutableList<Route> routes, StateHolder stateHolder, LifecycleHolder lifecycleHolder)
    {
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
        return widget.UseMemo(() => new Navigator(), true);
    }
}