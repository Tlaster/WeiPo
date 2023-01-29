using System.Collections.Immutable;
using System.Security.Cryptography;

namespace WeiPoX.Core.Routing;

public sealed record BackstackEntry(
    string Id,
    Route Route,
    StateHolder ParentStateHolder,
    ImmutableDictionary<string, string> PathMap,
    QueryString? QueryString = null
) : IDisposable
{
    private bool _destroyAfterTransition;
    internal StateHolder State => ParentStateHolder.GetOrElse(Id, new StateHolder()); 
    internal Lifecycle Lifecycle { get; } = new();

    public void Dispose()
    {
        State.Dispose();
    }

    internal void Active()
    {
        Lifecycle.CurrentState = Lifecycle.State.Active;
    }

    internal void InActive()
    {
        Lifecycle.CurrentState = Lifecycle.State.InActive;
        if (_destroyAfterTransition)
        {
            Destroy();
        }
    }

    internal void Destroy()
    {
        if (Lifecycle.CurrentState != Lifecycle.State.InActive)
        {
            _destroyAfterTransition = true;
        }
        else
        {
            Lifecycle.CurrentState = Lifecycle.State.Destroyed;
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