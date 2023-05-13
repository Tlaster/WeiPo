using Avalonia;
using Avalonia.Themes.Fluent;
using WeiPoX.Core.DeclarativeUI.Animation;
using WeiPoX.Core.DeclarativeUI.Animation.Platform.Avalonia;
using WeiPoX.Core.DeclarativeUI.Platform.Avalonia;
using WeiPoX.Core.DeclarativeUI.Sample.Core;

AppBuilder.Configure(() => new WeiPoXDeclarativeUiApp<SampleApp>
    {
        Styles =
        {
            new FluentTheme()
        },
        Title = "WeiPoX Declarative UI for Avalonia Sample",
        Renderers =
        {
            { typeof(PlatformAnimated), new PlatformAnimatedRenderer() }
        }
    })
    .UsePlatformDetect()
    .LogToTrace()
    .StartWithClassicDesktopLifetime(args);