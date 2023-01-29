using System.Collections.Immutable;

namespace WeiPoX.Core.Routing;

public class Navigator
{
    private readonly RouteStackManager _routeStackManager;

    public Navigator(StateHolder holder, Lifecycle lifecycle)
    {
        _routeStackManager = new RouteStackManager(holder, lifecycle);
    }

    internal IObservable<BackstackEntry> CurrentRoute => _routeStackManager.CurrentRoute;

    internal void Init(string initialRoute, ImmutableList<Route> routes)
    {
        _routeStackManager.Init(initialRoute, routes);
    }

    public void Push(string route)
    {
        _routeStackManager.Push(route);
    }

    public void Pop()
    {
        _routeStackManager.Pop();
    }

    public bool CanPop()
    {
        return !_routeStackManager.IsEmpty();
    }
}