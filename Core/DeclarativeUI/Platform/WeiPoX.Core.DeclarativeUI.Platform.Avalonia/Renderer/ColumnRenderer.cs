using Avalonia.Controls;
using Avalonia.Layout;
using WeiPoX.Core.DeclarativeUI.Internal;
using WeiPoX.Core.DeclarativeUI.Platform.Avalonia.Internal;
using WeiPoX.Core.DeclarativeUI.Widgets.Layout;

namespace WeiPoX.Core.DeclarativeUI.Platform.Avalonia.Renderer;

internal class ColumnRenderer : RendererObject<Column, StackPanel>
{
    protected override StackPanel Create(RendererContext<Control> context)
    {
        return new StackPanel
        {
            Orientation = Orientation.Vertical
        };
    }

    protected override void Update(StackPanel control, Column widget)
    {
        control.HorizontalAlignment = widget.Alignment.ToHorizontalAlignment();
        control.VerticalAlignment = widget.Vertical.ToVerticalAlignment();
    }
}