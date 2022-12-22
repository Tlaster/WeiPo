using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using WeiPoX.Core.DeclarativeUI.Internal;

namespace WeiPoX.Core.DeclarativeUI.Widgets;

public abstract record Widget;

public abstract record MappingWidget : Widget;

public abstract record StateWidget : Widget;

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

    protected State<T> UseState<T>(T initialState) where T : notnull
    {
        if (_hooks.Count <= _hookId)
        {
            _hooks.Add(initialState);
        }

        var index = _hookId;
        var setState = new Action<T>(value => { _hooks[index] = value; });
        return new State<T>((T)_hooks[_hookId++], setState);
    }


    protected State<T> UseState<T>(Func<T> initialState) where T : notnull
    {
        if (_hooks.Count <= _hookId)
        {
            _hooks.Add(initialState.Invoke());
        }

        var index = _hookId;
        var setState = new Action<T>(value => { _hooks[index] = value; });
        return new State<T>((T)_hooks[_hookId++], setState);
    }

    protected void UseEffect(Action effect, params object[] dependencies)
    {
        var hasNoDependencies = dependencies.Length == 0;
        var deps = _hooks.Count <= _hookId ? null : (object[])_hooks[_hookId];
        var hasChanged = deps == null || !deps.SequenceEqual(dependencies);
        if (hasNoDependencies || hasChanged)
        {
            effect();
            if (_hooks.Count <= _hookId)
            {
                _hooks.Add(dependencies);
            }
            else
            {
                _hooks[_hookId] = dependencies;
            }
        }

        _hookId++;
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

        return base.Equals(other) && _hooks.SequenceEqual(other._hooks);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(base.GetHashCode(), _hooks);
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
    }
}


internal interface IPanelWidget
{
    ImmutableList<Widget> Children { get; }
}