using System.Collections.Immutable;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using ReactiveUI;
using WeiPoX.Core.DeclarativeUI.Widgets;
using WeiPoX.Core.Lifecycle;
using WeiPoX.Core.Routing.Parser;

namespace WeiPoX.Core.Routing;

internal class RouteStackManager : ILifecycleObserver
{
    private readonly ReplaySubject<BackstackEntry?> _currentRoute = new();
    private readonly RouteParser _routeParser = new();
    private readonly Stack<BackstackEntry> _routeStack = new();
    private StateHolder? _stateHolderParent;
    private LifecycleHolder? _lifecycleHolderParent;

    public RouteStackManager()
    {
        CurrentRoute = _currentRoute.WhereNotNull().AsObservable();
    }

    public IObservable<BackstackEntry> CurrentRoute { get; }

    internal void Init(string initialRoute, ImmutableList<Route> routes, StateHolder stateHolder, LifecycleHolder lifecycleHolder)
    {
        _lifecycleHolderParent = lifecycleHolder;
        _lifecycleHolderParent.AddObserver(this);
        _stateHolderParent = stateHolder;
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
        if (_stateHolderParent is null)
        {
            return;
        }
        var query = path.Split('?').LastOrDefault() ?? string.Empty;
        var route = path.Split('?').FirstOrDefault() ?? throw new Exception("Route not found");
        var matchResult = _routeParser.Find(route);
        if (matchResult is null)
        {
            throw new Exception("Route not found");
        }

        var backstackEntry = new BackstackEntry(
            _routeStack.Count,
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

    public void OnStateChanged(LifecycleState state)
    {
        switch (state)
        {
            case LifecycleState.Initialized:
                break;
            case LifecycleState.Active:
                _routeStack.LastOrDefault()?.Active();
                break;
            case LifecycleState.InActive:
                _routeStack.LastOrDefault()?.InActive();
                break;
            case LifecycleState.Destroyed:
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

public record Route
{
    public required string Path { get; init; }
    public required Func<BackstackEntry, Widget> Content { get; init; }
}