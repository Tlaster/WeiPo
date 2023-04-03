using System;
using Microsoft.UI;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using WeiPoX.Core.DeclarativeUI.Widgets;
using DoubleTappedRoutedEventArgs = Microsoft.UI.Xaml.Input.DoubleTappedRoutedEventArgs;
using HoldingRoutedEventArgs = Microsoft.UI.Xaml.Input.HoldingRoutedEventArgs;
using TappedRoutedEventArgs = Microsoft.UI.Xaml.Input.TappedRoutedEventArgs;

namespace WeiPoX.Core.DeclarativeUI.Platform.WinUI3.Renderer;

internal class GestureDetectorRenderer : RendererObject<GestureDetector, WeiPoXGestureDetector>
{
    protected override void Update(WeiPoXGestureDetector control, GestureDetector widget)
    {
        control.OnTap = widget.OnTap;
        control.OnLongPress = widget.OnLongPress;
        control.OnDoubleTap = widget.OnDoubleTap;
    }
}

internal class WeiPoXGestureDetector : Grid
{
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

    private void OnHolding(object sender, HoldingRoutedEventArgs e)
    {
        OnLongPress?.Invoke();
    }

    private void OnDoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
    {
        OnDoubleTap?.Invoke();
    }

    private void OnTapped(object sender, TappedRoutedEventArgs e)
    {
        OnTap?.Invoke();
    }
}