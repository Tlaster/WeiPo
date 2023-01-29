namespace WeiPoX.Core.Routing;

public class Lifecycle : IDisposable
{
    public enum State
    {
        Initialized,
        Active,
        InActive,
        Destroyed
    }

    private readonly List<ILifecycleObserver> _observers = new();
    private State _currentState = State.Initialized;

    public State CurrentState
    {
        get => _currentState;
        set
        {
            if (_currentState == State.Destroyed || value == State.Initialized)
            {
                return;
            }

            _currentState = value;
            DispatchStateChange(value);
        }
    }

    public void Dispose()
    {
        _observers.Clear();
    }

    private void DispatchStateChange(State value)
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
