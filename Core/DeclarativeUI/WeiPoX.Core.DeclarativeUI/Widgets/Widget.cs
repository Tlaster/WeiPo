using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using WeiPoX.Core.DeclarativeUI.Internal;

namespace WeiPoX.Core.DeclarativeUI.Widgets;

public abstract record Widget;

public abstract record MappingWidget : Widget;

public abstract record StateWidget : Widget;

public record ContextProvider(ImmutableDictionary<Type, object> Providers, Widget Child) : StateWidget, IContextProvider, IPanelWidget
{
    public ImmutableList<Widget> Children { get; } = ImmutableList.Create(Child);
}

public abstract record StatelessWidget : StateWidget
{
    protected internal abstract Widget Content { get; }

    public virtual bool Equals(StatelessWidget? other)
    {
        if (ReferenceEquals(null, other))
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return base.Equals(other) && Content.Equals(other.Content);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(base.GetHashCode(), Content);
    }
}

internal class StatefulWidgetState : IDisposable
{
    public ObservableCollection<object> Hooks { get; } = new();
    public int HookId { get; set; }
    public bool Dirty { get; set; }
    public IBuildOwner? BuildOwner { get; set; }
    public BuildContext? BuildContext { get; set; }
    public Dictionary<Type, object> UsedProviders { get; } = new();
    public Widget? Widget { get; set; }
    internal Widget? CachedBuild { get; set; }

    public StatefulWidgetState()
    {
        Hooks.CollectionChanged += (sender, args) =>
        {
            if (args.Action != NotifyCollectionChangedAction.Replace)
            {
                return;
            }

            if (Dirty)
            {
                return;
            }

            Dirty = true;
#if DEBUG
            if (BuildOwner == null)
            {
                throw new Exception("BuildOwner is null");
            }
#endif
            if (Widget != null)
            {
                BuildOwner?.MarkNeedsBuild(Widget);
            }
        };
    }

    public void Dispose()
    {
        foreach (var hook in Hooks)
        {
            if (hook is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }
    }
}

public abstract record StatefulWidget : StateWidget, IDisposable
{
    internal StatefulWidgetState State { get; private set; } = new();
    // for testing only
    internal bool Disposed { get; private set; }

    protected StatefulWidget()
    {
        State.Widget = this;
    }

    internal Widget BuildInternal()
    {
        State.HookId = 0;
        var result = Build();
        State.Dirty = false;
        return result;
    }
    
    internal void Merge(StatefulWidget other)
    {
        State = other.State;
        State.Widget = this;
    }

    protected abstract Widget Build();

    public State<T> UseState<T>(T initialState) where T : notnull
    {
        if (State.Hooks.Count <= State.HookId)
        {
            State.Hooks.Add(initialState);
        }

        var index = State.HookId;
        var setState = new Action<T>(value => { State.Hooks[index] = value; });
        return new State<T>((T)State.Hooks[State.HookId++], setState);
    }


    public State<T> UseState<T>(Func<T> initialState) where T : notnull
    {
        if (State.Hooks.Count <= State.HookId)
        {
            State.Hooks.Add(initialState.Invoke());
        }

        var index = State.HookId;
        var setState = new Action<T>(value => { State.Hooks[index] = value; });
        return new State<T>((T)State.Hooks[State.HookId++], setState);
    }

    public void UseEffect(Action effect, params object[] dependencies)
    {
        UseMemo(() =>
        {
            effect.Invoke();
            return true;
        }, dependencies);
    }
    
    public void UseEffect(Func<Action> effect, params object[] dependencies)
    {
        UseMemo(() =>
        {
            var dispose = effect.Invoke();
            return new DisposableEffect(dispose);
        }, dependencies);
    }
    
    public T UseMemo<T>(Func<T> factory, params object[] dependencies)
    {
        var hasNoDependencies = dependencies.Length == 0;
        var memo = State.Hooks.Count <= State.HookId ? null : (Memo<T>)State.Hooks[State.HookId];
        var deps = memo?.Dependencies;
        var hasChanged = deps == null || !deps.SequenceEqual(dependencies);
        if (hasNoDependencies || hasChanged)
        {
            memo?.Dispose();
            var value = factory();
            if (State.Hooks.Count <= State.HookId)
            {
                State.Hooks.Add(new Memo<T>(value, dependencies));
            }
            else
            {
                State.Hooks[State.HookId] = new Memo<T>(value, dependencies);
            }
        }

        return ((Memo<T>)State.Hooks[State.HookId++]).Value;
    }
    
    public T UseContext<T>()
    {
        if (State.BuildContext == null)
        {
            throw new Exception("BuildContext is null");
        }
        var value = State.BuildContext.Get<T>() ?? throw new Exception($"Context {typeof(T)} not found");
        State.UsedProviders[typeof(T)] = value;
        return value;
    }

    public virtual bool Equals(StatefulWidget? other)
    {
        if (ReferenceEquals(null, other))
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return base.Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(base.GetHashCode());
    }

    public void Dispose()
    {
        State.Dispose();
        Disposed = true;
        GC.SuppressFinalize(this);
    }
}

internal record Memo<T>(T Value, object[] Dependencies) : IDisposable
{
    public void Dispose()
    {
        if (Value is IDisposable disposable)
        {
            disposable.Dispose();
        }
    }
}

internal record DisposableEffect : IDisposable
{
    private readonly Action _dispose;

    public DisposableEffect(Action dispose)
    {
        _dispose = dispose;
    }
    public void Dispose()
    {
        _dispose();
    }
}

internal interface IPanelWidget
{
    ImmutableList<Widget> Children { get; }
}

internal interface IContextProvider
{
    ImmutableDictionary<Type, object> Providers { get; }
}