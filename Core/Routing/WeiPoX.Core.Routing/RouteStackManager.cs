using System.Reactive.Linq;
using System.Reactive.Subjects;
using WeiPoX.Core.DeclarativeUI.Widgets;

namespace WeiPoX.Core.Routing;

internal class RouteStackManager
{
    private readonly ReplaySubject<Route?> _currentRoute = new();
    private readonly Stack<Route> _routeStack = new();
    public IObservable<Route?> CurrentRoute => _currentRoute.AsObservable();

    public void Push(Route route)
    {
        _routeStack.Push(route);
        _currentRoute.OnNext(route);
    }

    public Route Pop()
    {
        var result = _routeStack.Pop();
        _currentRoute.OnNext(_routeStack.Peek());
        return result;
    }

    public Route Peek()
    {
        return _routeStack.Peek();
    }

    public bool IsEmpty()
    {
        return _routeStack.Count == 0;
    }
}

public record Route(string Path, Func<BackstackEntry, Widget> Content);