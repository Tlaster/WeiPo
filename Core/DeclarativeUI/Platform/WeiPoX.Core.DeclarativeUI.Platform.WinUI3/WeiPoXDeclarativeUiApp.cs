using Microsoft.UI.Xaml;
using WeiPoX.Core.DeclarativeUI.Widgets;
using WeiPoX.Core.Lifecycle;

namespace WeiPoX.Core.DeclarativeUI.Platform.WinUI3;

public abstract class WeiPoXDeclarativeUiApp : Application
{
    private readonly StateHolder _stateHolder = new();
    private readonly LifecycleHolder _lifecycleHolder = new();
    
    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        new Window
        {
            Content = new DeclarativeView
            {
                Widget = new AppWidget
                {
                    App = CreateWidget(),
                    StateHolder = _stateHolder,
                    LifecycleHolder = _lifecycleHolder,
                }
            },
        }.Activate();
    }
    
    protected abstract Widget CreateWidget();
    
}