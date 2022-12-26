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

public abstract record StatefulWidget : StateWidget, IDisposable
{
    private readonly ObservableCollection<object> _hooks = new();
    private int _hookId = 0;
    private bool _dirty = true;
    
    internal bool Disposed { get; private set; }
    internal Widget? CachedBuild { get; set; }
    internal IBuildOwner? BuildOwner { get; set; }
    internal BuildContext? BuildContext { get; set; }

    protected StatefulWidget()
    {
        _hooks.CollectionChanged += (sender, args) =>
        {
            if (args.Action != NotifyCollectionChangedAction.Replace)
            {
                return;
            }

            if (_dirty)
            {
                return;
            }

            _dirty = true;
#if DEBUG
            if (BuildOwner == null)
            {
                throw new Exception("BuildOwner is null");
            }
#endif
            BuildOwner?.MarkNeedsBuild(this);
        };
    }

    internal Widget BuildInternal()
    {
        _hookId = 0;
        var result = Build();
        _dirty = false;
        return result;
    }

    protected abstract Widget Build();

    public State<T> UseState<T>(T initialState) where T : notnull
    {
        if (_hooks.Count <= _hookId)
        {
            _hooks.Add(initialState);
        }

        var index = _hookId;
        var setState = new Action<T>(value => { _hooks[index] = value; });
        return new State<T>((T)_hooks[_hookId++], setState);
    }


    public State<T> UseState<T>(Func<T> initialState) where T : notnull
    {
        if (_hooks.Count <= _hookId)
        {
            _hooks.Add(initialState.Invoke());
        }

        var index = _hookId;
        var setState = new Action<T>(value => { _hooks[index] = value; });
        return new State<T>((T)_hooks[_hookId++], setState);
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
        var memo = _hooks.Count <= _hookId ? null : (Memo<T>)_hooks[_hookId];
        var deps = memo?.Dependencies;
        var hasChanged = deps == null || !deps.SequenceEqual(dependencies);
        if (hasNoDependencies || hasChanged)
        {
            memo?.Dispose();
            var value = factory();
            if (_hooks.Count <= _hookId)
            {
                _hooks.Add(new Memo<T>(value, dependencies));
            }
            else
            {
                _hooks[_hookId] = new Memo<T>(value, dependencies);
            }
        }

        return ((Memo<T>)_hooks[_hookId++]).Value;
    }
    
    public T UseContext<T>()
    {
        if (BuildContext == null)
        {
            throw new Exception("BuildContext is null");
        }

        return BuildContext.Get<T>() ?? throw new Exception($"Context {typeof(T)} not found");
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

        return base.Equals(other) && _hooks.SequenceEqual(other._hooks) && Equals(CachedBuild, other.CachedBuild);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(base.GetHashCode(), _hooks, CachedBuild);
    }

    public void Dispose()
    {
        foreach (var hook in _hooks)
        {
            if (hook is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }

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