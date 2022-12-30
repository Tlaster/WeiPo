namespace WeiPoX.Core.Routing;

public interface ILifecycleObserver
{
    void OnStateChanged(Lifecycle.State state);
}