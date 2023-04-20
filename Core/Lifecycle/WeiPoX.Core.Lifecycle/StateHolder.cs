namespace WeiPoX.Core.Lifecycle;

public class StateHolder : IDisposable
{
    private readonly Dictionary<string, object?> _states = new();

    public void Dispose()
    {
        foreach (var state in _states.Values)
        {
            if (state is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }
        _states.Clear();
    }

    public void Add(string key, object? state)
    {
        _states[key] = state;
    }
    
    public T GetOrElse<T>(string key, Func<T> defaultValueFactory)
    {
        if (_states.TryGetValue(key, out var value) && value is T actualValue)
        {
            return actualValue;
        }
        var defaultValue = defaultValueFactory.Invoke();
        _states.Add(key, defaultValue);
        return defaultValue;
    }
    
}
