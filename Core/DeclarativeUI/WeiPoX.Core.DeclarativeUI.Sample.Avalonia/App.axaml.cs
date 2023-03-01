using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using WeiPoX.Core.DeclarativeUI.Platform.Avalonia;
using WeiPoX.Core.DeclarativeUI.Sample.Core;

namespace WeiPoX.Core.DeclarativeUI.Sample.Avalonia;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new Window
            {
                Content = new Declarative(new SampleApp())
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}