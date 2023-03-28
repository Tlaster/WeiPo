using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Themes.Fluent;
using WeiPoX.Core.DeclarativeUI.Widgets;

namespace WeiPoX.Core.DeclarativeUI.Platform.Avalonia;

public class WeiPoXDeclarativeUiApp<T> : Application where T : Widget, new()
{
    private readonly T _widget = new();
    public string? Title { get; set; }

    public override void OnFrameworkInitializationCompleted()
    {
        switch (ApplicationLifetime)
        {
            case IClassicDesktopStyleApplicationLifetime desktop:
                desktop.MainWindow = new Window
                {
                    Content = new DeclarativeView
                    {
                        Widget = _widget,
                    },
                    Title = Title
                };
                break;
            case ISingleViewApplicationLifetime singleViewApplicationLifetime:
                singleViewApplicationLifetime.MainView = new DeclarativeView
                {
                    Widget = _widget,
                };
                break;
        }

        base.OnFrameworkInitializationCompleted();
    }
}

