using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;
using WeiPoX.Core.DeclarativeUI.Platform.Avalonia.Internal;
using WeiPoX.Core.DeclarativeUI.Widgets.Layout;
using AvaloniaThickness = Avalonia.Thickness;

namespace WeiPoX.Core.DeclarativeUI.Platform.Avalonia.Renderer;

public class BoxRenderer : LayoutPanelRenderer<Box>
{
    protected override void Update(WeiPoXPanel control, Box widget)
    {
        base.Update(control, widget);
        control.Background = new SolidColorBrush(widget.BackgroundColor.ToColor());
    }
}
