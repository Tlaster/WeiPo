namespace WeiPoX.Core.Lifecycle;

public sealed class LifecycleHolder : IDisposable
{
    private readonly List<ILifecycleObserver> _observers = new();
    private LifecycleState _currentLifecycleState = LifecycleState.Initialized;

    public LifecycleState CurrentLifecycleState
    {
        get => _currentLifecycleState;
        set
        {
            if (_currentLifecycleState == LifecycleState.Destroyed || value == LifecycleState.Initialized)
            {
                return;
            }

            _currentLifecycleState = value;
            DispatchStateChange(value);
        }
    }

    public void Dispose()
    {
        _observers.Clear();
    }

    private void DispatchStateChange(LifecycleState value)
    {
        foreach (var observer in _observers)
        {
            observer.OnStateChanged(value);
        }
    }

    public void AddObserver(ILifecycleObserver observer)
    {
        _observers.Add(observer);
    }

    public void RemoveObserver(ILifecycleObserver observer)
    {
        _observers.Remove(observer);
    }

    public bool HasObservers()
    {
        return _observers.Count > 0;
    }
}

public enum LifecycleState
{
    Initialized,
    Active,
    InActive,
    Destroyed
}