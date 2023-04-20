using System.Collections.Immutable;
using System.Security.Cryptography;
using WeiPoX.Core.Lifecycle;

namespace WeiPoX.Core.Routing;

public sealed record BackstackEntry(
    long Index,
    Route Route,
    StateHolder ParentStateHolder,
    ImmutableDictionary<string, string> PathMap,
    QueryString? QueryString = null
) : IDisposable
{
    private bool _destroyAfterTransition;
    internal StateHolder State => ParentStateHolder.GetOrElse($"{Index}-{Route.Path}", () => new StateHolder()); 
    internal LifecycleHolder Lifecycle { get; } = new();

    public void Dispose()
    {
        State.Dispose();
    }

    internal void Active()
    {
        Lifecycle.CurrentLifecycleState = LifecycleState.Active;
    }

    internal void InActive()
    {
        Lifecycle.CurrentLifecycleState = LifecycleState.InActive;
        if (_destroyAfterTransition)
        {
            Destroy();
        }
    }

    internal void Destroy()
    {
        if (Lifecycle.CurrentLifecycleState != LifecycleState.InActive)
        {
            _destroyAfterTransition = true;
        }
        else
        {
            Lifecycle.CurrentLifecycleState = LifecycleState.Destroyed;
            Dispose();
        }
    }
    
    public T? Path<T>(string key, T? defaultValue = default)
    {
        if (PathMap.TryGetValue(key, out var value))
        {
            return (T) Convert.ChangeType(value, typeof(T));
        }

        return defaultValue;
    }
    
    public T? Query<T>(string key, T? defaultValue = default)
    {
        if (QueryString is null)
        {
            return defaultValue;
        }

        if (!QueryString.Value.TryGetValue(key, out var value))
        {
            return defaultValue;
        }

        var first = value.FirstOrDefault();
        if (first is null)
        {
            return defaultValue;
        }
        return (T) Convert.ChangeType(first, typeof(T));
    }
    
    public ImmutableList<T> QueryList<T>(string key)
    {
        if (QueryString is null)
        {
            return ImmutableList<T>.Empty;
        }

        if (!QueryString.Value.TryGetValue(key, out var value))
        {
            return ImmutableList<T>.Empty;
        }

        return value.Select(v => (T) Convert.ChangeType(v, typeof(T))).ToImmutableList();
    }
}