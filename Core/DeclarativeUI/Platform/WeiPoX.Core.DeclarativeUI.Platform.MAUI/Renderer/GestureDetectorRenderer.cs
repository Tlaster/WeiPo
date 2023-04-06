using WeiPoX.Core.DeclarativeUI.Widgets;

namespace WeiPoX.Core.DeclarativeUI.Platform.MAUI.Renderer;

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
        var tapGestureRecognizer = new TapGestureRecognizer();
        tapGestureRecognizer.Tapped += OnTapped;
        GestureRecognizers.Add(tapGestureRecognizer);
        var doubleTapGestureRecognizer = new TapGestureRecognizer
        {
            NumberOfTapsRequired = 2
        };
        doubleTapGestureRecognizer.Tapped += OnDoubleTapped;
        GestureRecognizers.Add(doubleTapGestureRecognizer);
        // TODO: LongPressGestureRecognizer
        Background = new SolidColorBrush(Colors.Transparent);
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