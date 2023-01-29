using Microsoft.UI.Xaml;

namespace WeiPoX.App.WinUI;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
    }

    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        new Window().Activate();
    }
}