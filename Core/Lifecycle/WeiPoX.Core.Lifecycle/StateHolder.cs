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
    }

    public void Add(string key, object? state)
    {
        _states[key] = state;
    }
    
    public T GetOrElse<T>(string key, T defaultValue)
    {
        if (_states.TryGetValue(key, out var value) && value is T actualValue)
        {
            return actualValue;
        }

        _states.Add(key, defaultValue);
        return defaultValue;
    }
}
