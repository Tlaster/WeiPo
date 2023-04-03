using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Themes.Fluent;
using WeiPoX.Core.DeclarativeUI.Widgets;
using WeiPoX.Core.Lifecycle;

namespace WeiPoX.Core.DeclarativeUI.Platform.Avalonia;

public class WeiPoXDeclarativeUiApp<T> : Application where T : Widget, new()
{
    private readonly StateHolder _stateHolder = new();
    private readonly LifecycleHolder _lifecycleHolder = new();
    private readonly AppWidget _widget;

    public WeiPoXDeclarativeUiApp()
    {
        _widget = new AppWidget
        {
            App = new T(),
            StateHolder = _stateHolder,
            LifecycleHolder = _lifecycleHolder,
        };
    }

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

