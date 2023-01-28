using System.Collections.Immutable;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using ReactiveUI;
using WeiPoX.Core.DeclarativeUI.Widgets;
using WeiPoX.Core.Routing.Parser;

namespace WeiPoX.Core.Routing;

internal class RouteStackManager
{
    private readonly ReplaySubject<BackstackEntry?> _currentRoute = new();
    private readonly Stack<BackstackEntry> _routeStack = new();
    public IObservable<BackstackEntry> CurrentRoute => _currentRoute.WhereNotNull().AsObservable();

    private readonly RouteParser _routeParser = new();

    internal void Init(string initialRoute, ImmutableList<Route> routes)
    {
        foreach (var route in routes)
        {
            foreach (var expandOptionalVariable in RouteParser.ExpandOptionalVariables(route.Path))
            {
                _routeParser.Insert(expandOptionalVariable, route);
            }
        }
        Push(initialRoute);
    }
    
    public void Push(string path)
    {
        var query = path.Split('?').LastOrDefault() ?? string.Empty;
        var route = path.Split('?').FirstOrDefault() ?? throw new Exception("Route not found");
        var matchResult = _routeParser.Find(route);
        if (matchResult is null)
        {
            throw new Exception("Route not found");
        }
        var backstackEntry = new BackstackEntry(matchResult.Route, matchResult.PathMap, new QueryString(query));
        _routeStack.Push(backstackEntry);
        _currentRoute.OnNext(backstackEntry);
    }

    public BackstackEntry Pop()
    {
        var result = _routeStack.Pop();
        _currentRoute.OnNext(_routeStack.Peek());
        return result;
    }

    public BackstackEntry Peek()
    {
        return _routeStack.Peek();
    }

    public bool IsEmpty()
    {
        return _routeStack.Count == 0;
    }
}

public record Route(string Path, Func<BackstackEntry, Widget> Content);