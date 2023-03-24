using Avalonia.Controls;
using Avalonia.Layout;
using WeiPoX.Core.DeclarativeUI.Platform.Avalonia.Internal;
using WeiPoX.Core.DeclarativeUI.Widgets.Layout;

namespace WeiPoX.Core.DeclarativeUI.Platform.Avalonia.Renderer;

internal class RowRenderer : RendererObject<Row, StackPanel>
{
    protected override StackPanel Create(WidgetBuilder renderer)
    {
        return new StackPanel
        {
            Orientation = Orientation.Horizontal
        };
    }

    protected override void Update(StackPanel control, Row widget)
    {
        control.HorizontalAlignment = widget.Horizontal.ToHorizontalAlignment();
        control.VerticalAlignment = widget.Vertical.ToVerticalAlignment();
    }
}