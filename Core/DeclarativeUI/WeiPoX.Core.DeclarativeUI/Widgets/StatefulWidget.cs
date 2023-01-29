namespace WeiPoX.Core.DeclarativeUI.Widgets;

public abstract record StatefulWidget : StateWidget, IDisposable
{
    protected StatefulWidget()
    {
        State.Widget = this;
    }

    internal StatefulWidgetState State { get; private set; } = new();

    // for testing only
    internal bool Disposed { get; private set; }

    public void Dispose()
    {
        State.Dispose();
        Disposed = true;
        GC.SuppressFinalize(this);
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

    public State<T> UseState<T>(T initialState)
    {
        return UseState(() => initialState);
    }

    public State<T> UseState<T>(Func<T> initialState)
    {
        if (State.Hooks.Count <= State.HookId)
        {
            State.Hooks.Add(new StateValue<T>(initialState.Invoke()));
        }

        var index = State.HookId;
        var setState = new Action<T>(value => { State.Hooks[index] = new StateValue<T>(value); });
        if (State.Hooks[State.HookId++] is not StateValue<T> state)
        {
            throw new Exception("State is null");
        }
        
        return new State<T>(state.Value, setState);
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

    public override int GetHashCode()
    {
        return HashCode.Combine(base.GetHashCode());
    }
}