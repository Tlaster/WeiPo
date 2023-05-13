using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Themes.Fluent;
using WeiPoX.Core.DeclarativeUI.Internal;
using WeiPoX.Core.DeclarativeUI.Platform.Avalonia.Internal;
using WeiPoX.Core.DeclarativeUI.Widgets;
using WeiPoX.Core.Lifecycle;

namespace WeiPoX.Core.DeclarativeUI.Platform.Avalonia;

public class WeiPoXDeclarativeUiApp<T> : Application where T : Widget, new()
{
    private readonly AppWidget _widget;

    public WeiPoXDeclarativeUiApp()
    {
        _widget = new AppWidget
        {
            App = new T(),
        };
    }

    public string? Title { get; set; }

    public Dictionary<Type, IRenderer<Control>> Renderers { get; } = new();

    public override void OnFrameworkInitializationCompleted()
    {
        foreach (var (key, value) in Renderers)
        {
            RendererPool.RegisterRenderer(key, value);
        }
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

