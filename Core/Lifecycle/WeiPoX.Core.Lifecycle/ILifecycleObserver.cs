namespace WeiPoX.Core.Lifecycle;

public interface ILifecycleObserver
{
    void OnStateChanged(LifecycleState lifecycleState);
}