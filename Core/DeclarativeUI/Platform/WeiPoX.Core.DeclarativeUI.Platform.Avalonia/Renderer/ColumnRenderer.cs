using Avalonia.Controls;
using Avalonia.Layout;
using WeiPoX.Core.DeclarativeUI.Platform.Avalonia.Internal;
using WeiPoX.Core.DeclarativeUI.Widgets.Layout;

namespace WeiPoX.Core.DeclarativeUI.Platform.Avalonia.Renderer;

internal class ColumnRenderer : RendererObject<Column, StackPanel>
{
    protected override StackPanel Create()
    {
        return new StackPanel
        {
            Orientation = Orientation.Vertical
        };
    }

    protected override void Update(StackPanel control, Column widget)
    {
        control.HorizontalAlignment = widget.Horizontal.ToHorizontalAlignment();
        control.VerticalAlignment = widget.Vertical.ToVerticalAlignment();
    }
}