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
    private readonly RouteParser _routeParser = new();
    private readonly BehaviorSubject<ImmutableList<BackstackEntry>> _routeStack = new(ImmutableList<BackstackEntry>.Empty);
    private StateHolder? _stateHolderParent;
    private LifecycleHolder? _lifecycleHolderParent;

    public RouteStackManager()
    {
        CurrentRoute = _routeStack.Select(x => x.LastOrDefault()).Where(x => x is not null).Select(x => x!);
        CanGoBack = _routeStack.Select(x => x.Count > 1);
    }

    public IObservable<BackstackEntry> CurrentRoute { get; }
    public IObservable<bool> CanGoBack { get; }

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
        var currentRouteStack = _routeStack.Value;
        var query = path.Split('?').LastOrDefault() ?? string.Empty;
        var route = path.Split('?').FirstOrDefault() ?? throw new Exception("Route not found");
        var matchResult = _routeParser.Find(route);
        if (matchResult is null)
        {
            throw new Exception("Route not found");
        }

        var backstackEntry = new BackstackEntry(
            currentRouteStack.Count,
            matchResult.Route,
            _stateHolderParent,
            matchResult.PathMap,
            new QueryString(query)
        );
        _routeStack.OnNext(currentRouteStack.Add(backstackEntry));
    }

    public void Pop()
    {
        var currentRouteStack = _routeStack.Value;
        if (currentRouteStack.Count == 0)
        {
            return;
        }
        var backstackEntry = currentRouteStack.LastOrDefault();
        _routeStack.OnNext(currentRouteStack.RemoveAt(currentRouteStack.Count - 1));
        backstackEntry?.Destroy();
    }

    public BackstackEntry? Peek()
    {
        var currentRouteStack = _routeStack.Value;
        return currentRouteStack.LastOrDefault();
    }

    public void OnStateChanged(LifecycleState state)
    {
        switch (state)
        {
            case LifecycleState.Initialized:
                break;
            case LifecycleState.Active:
                _routeStack.Value.LastOrDefault()?.Active();
                break;
            case LifecycleState.InActive:
                _routeStack.Value.LastOrDefault()?.InActive();
                break;
            case LifecycleState.Destroyed:
                foreach (var backstackEntry in _routeStack.Value)
                {
                    backstackEntry.Destroy();
                }
                _routeStack.OnNext(ImmutableList<BackstackEntry>.Empty);
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