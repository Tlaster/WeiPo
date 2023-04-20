using Microsoft.UI.Xaml;
using WeiPoX.Core.DeclarativeUI.Widgets;
using WeiPoX.Core.Lifecycle;

namespace WeiPoX.Core.DeclarativeUI.Platform.WinUI3;

public abstract class WeiPoXDeclarativeUiApp : Application
{
    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        new Window
        {
            Content = new DeclarativeView
            {
                Widget = new AppWidget
                {
                    App = CreateWidget(),
                }
            },
        }.Activate();
    }
    
    protected abstract Widget CreateWidget();
    
}