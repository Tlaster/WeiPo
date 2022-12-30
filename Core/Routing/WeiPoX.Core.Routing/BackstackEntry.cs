using System.Collections.Immutable;

namespace WeiPoX.Core.Routing;

public record BackstackEntry(Route Route, ImmutableDictionary<string, string> PathMap, QueryString? QueryString = null) : IDisposable
{
    private bool _destroyAfterTransition = false;
    public StateHolder State { get; } = new();
    public Lifecycle Lifecycle { get; } = new();
    
    public void Active()
    {
        Lifecycle.CurrentState = Lifecycle.State.Active;
    }
    
    public void InActive()
    {
        Lifecycle.CurrentState = Lifecycle.State.InActive;
        if (_destroyAfterTransition)
        {
            Destroy();
        }
    }
    
    public void Destroy()
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

    public void Dispose()
    {
        State.Dispose();
    }
}