using Avalonia;
using Avalonia.Themes.Fluent;
using WeiPoX.Core.DeclarativeUI.Platform.Avalonia;
using WeiPoX.Core.DeclarativeUI.Sample.Core;

AppBuilder.Configure(() => new WeiPoXDeclarativeUiApp<SampleApp>
    {
        Styles =
        {
            new FluentTheme()
        },
        Title = "WeiPoX Declarative UI for Avalonia Sample"
    })
    .UsePlatformDetect()
    .LogToTrace()
    .StartWithClassicDesktopLifetime(args);