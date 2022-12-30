namespace WeiPoX.Core.Routing;

public class StateHolder : IDisposable
{
    private readonly Dictionary<string, object> _states = new();
    
    public void Add(string key, object state)
    {
        _states.Add(key, state);
    }
    
    public T Get<T>(string key)
    {
        return (T) _states[key];
    }
    
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
}