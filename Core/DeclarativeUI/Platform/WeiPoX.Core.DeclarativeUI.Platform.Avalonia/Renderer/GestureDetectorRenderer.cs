using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Styling;
using WeiPoX.Core.DeclarativeUI.Widgets;

namespace WeiPoX.Core.DeclarativeUI.Platform.Avalonia.Renderer;

internal class GestureDetectorRenderer : RendererObject<GestureDetector, WeiPoXGestureDetector>
{
    protected override void Update(WeiPoXGestureDetector control, GestureDetector widget)
    {
        control.OnTap = widget.OnTap;
        control.OnLongPress = widget.OnLongPress;
        control.OnDoubleTap = widget.OnDoubleTap;
    }
}

internal class WeiPoXGestureDetector : Grid, IStyleable
{
    Type IStyleable.StyleKey => typeof(Grid);

    public Action? OnTap { get; set; }
    public Action? OnLongPress { get; set; }
    public Action? OnDoubleTap { get; set; }
    
    public WeiPoXGestureDetector()
    {
        Tapped += OnTapped;
        DoubleTapped += OnDoubleTapped;
        Holding += OnHolding;
        Background = new SolidColorBrush(Colors.Transparent);
    }

    private void OnHolding(object? sender, HoldingRoutedEventArgs e)
    {
        OnLongPress?.Invoke();
    }

    private void OnDoubleTapped(object? sender, TappedEventArgs e)
    {
        OnDoubleTap?.Invoke();
    }

    private void OnTapped(object? sender, TappedEventArgs e)
    {
        OnTap?.Invoke();
    }
}