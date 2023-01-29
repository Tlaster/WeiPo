using System.Collections.Immutable;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using ReactiveUI;
using WeiPoX.Core.DeclarativeUI.Widgets;
using WeiPoX.Core.Routing.Parser;

namespace WeiPoX.Core.Routing;

internal class RouteStackManager : ILifecycleObserver
{
    private readonly ReplaySubject<BackstackEntry?> _currentRoute = new();
    private readonly RouteParser _routeParser = new();
    private readonly Stack<BackstackEntry> _routeStack = new();
    private readonly StateHolder _stateHolderParent;

    public RouteStackManager(StateHolder stateHolderParent, Lifecycle lifecycleParent)
    {
        _stateHolderParent = stateHolderParent;
        lifecycleParent.AddObserver(this);
    }

    public IObservable<BackstackEntry> CurrentRoute => _currentRoute.WhereNotNull().AsObservable();

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

        var backstackEntry = new BackstackEntry(
            Guid.NewGuid().ToString(),
            matchResult.Route,
            _stateHolderParent,
            matchResult.PathMap,
            new QueryString(query)
        );
        _routeStack.Push(backstackEntry);
        _currentRoute.OnNext(backstackEntry);
    }

    public void Pop()
    {
        var result = _routeStack.Pop();
        _currentRoute.OnNext(_routeStack.Peek());
        result.Destroy();
    }

    public BackstackEntry Peek()
    {
        return _routeStack.Peek();
    }

    public bool IsEmpty()
    {
        return _routeStack.Count == 0;
    }

    public void OnStateChanged(Lifecycle.State state)
    {
        switch (state)
        {
            case Lifecycle.State.Initialized:
                break;
            case Lifecycle.State.Active:
                _routeStack.LastOrDefault()?.Active();
                break;
            case Lifecycle.State.InActive:
                _routeStack.LastOrDefault()?.InActive();
                break;
            case Lifecycle.State.Destroyed:
                foreach (var backstackEntry in _routeStack)
                {
                    backstackEntry.Destroy();
                }
                _routeStack.Clear();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }
    }
}

public record Route(string Path, Func<BackstackEntry, Widget> Content);