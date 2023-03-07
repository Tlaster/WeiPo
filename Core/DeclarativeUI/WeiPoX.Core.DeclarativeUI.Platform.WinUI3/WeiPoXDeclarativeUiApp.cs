using Microsoft.UI.Xaml;
using WeiPoX.Core.DeclarativeUI.Widgets;

namespace WeiPoX.Core.DeclarativeUI.Platform.WinUI3;

public abstract class WeiPoXDeclarativeUiApp : Application
{
    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        new Window
        {
            Content = new Declarative(CreateWidget()),
        }.Activate();
    }
    
    protected abstract Widget CreateWidget();
    
}