using Android.Runtime;
using AndroidX.Core.App;
using WeiPoX.Core.DeclarativeUI.Widgets;
using WeiPoX.Core.Lifecycle;
using Object = Java.Lang.Object;

namespace WeiPoX.Core.DeclarativeUI.Platform.Android;

public abstract class DeclarativeActivity<T> : ComponentActivity where T : Widget, new() 
{
    private DeclarativeState? _state;
    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        if (LastNonConfigurationInstance is DeclarativeState state)
        {
            _state = state;
        }
        else
        {
            _state = new DeclarativeState(new StateHolder(), new LifecycleHolder());
        }
        SetContentView(new DeclarativeView(this)
        {
            Widget = new AppWidget
            {
                App = new T(),
                StateHolder = _state.StateHolder,
                LifecycleHolder = _state.LifecycleHolder,
            }
        });
    }

    public override Object? OnRetainNonConfigurationInstance()
    {
        return _state;
    }
}

internal class DeclarativeState : Object
{
    public DeclarativeState(StateHolder stateHolder, LifecycleHolder lifecycleHolder)
    {
        StateHolder = stateHolder;
        LifecycleHolder = lifecycleHolder;
    }

    public StateHolder StateHolder { get; }
    public LifecycleHolder LifecycleHolder { get; }
    
}