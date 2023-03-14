using Avalonia.Controls;
using WeiPoX.Core.DeclarativeUI.Platform.Avalonia.Internal;
using WeiPoX.Core.DeclarativeUI.Widgets.Layout;

namespace WeiPoX.Core.DeclarativeUI.Platform.Avalonia.Renderer;

internal class BoxRenderer : RendererObject<Box, Grid>
{
    protected override void Update(Grid control, Box widget)
    {
        control.HorizontalAlignment = widget.Horizontal.ToHorizontalAlignment();
        control.VerticalAlignment = widget.Vertical.ToVerticalAlignment();
    }
}