using Avalonia;
using Avalonia.ReactiveUI;
using FluentAvalonia.UI.Windowing;
using WeiPoX.App.Avalonia;

BuildAvaloniaApp()
    .StartWithClassicDesktopLifetime(args);
    
static AppBuilder BuildAvaloniaApp()
{
    return AppBuilder.Configure<App>()
        .UsePlatformDetect()
        .LogToTrace()
        .UseReactiveUI()
        .UseFAWindowing();
}
