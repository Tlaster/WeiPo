using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace WeiPoX.Core.Routing;

internal class RouteStackManager
{
    private readonly ReplaySubject<IRoute?> _currentRoute = new();
    private readonly Stack<IRoute> _routeStack = new();
    public IObservable<IRoute?> CurrentRoute => _currentRoute.AsObservable();

    public void Push(IRoute route)
    {
        _routeStack.Push(route);
        _currentRoute.OnNext(route);
    }

    public IRoute Pop()
    {
        var result = _routeStack.Pop();
        _currentRoute.OnNext(_routeStack.Peek());
        return result;
    }

    public IRoute Peek()
    {
        return _routeStack.Peek();
    }

    public bool IsEmpty()
    {
        return _routeStack.Count == 0;
    }
}

internal interface IRoute
{
}

internal interface ITypeRoute : IRoute
{
    Type Type { get; }
}