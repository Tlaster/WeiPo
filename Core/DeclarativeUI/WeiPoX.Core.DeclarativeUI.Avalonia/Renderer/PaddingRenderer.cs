using Avalonia;
using Avalonia.Controls;
using WeiPoX.Core.DeclarativeUI.Widget;

namespace WeiPoX.Core.DeclarativeUI.Avalonia.Renderer;

internal class PaddingRenderer : RendererObject<Padding, Grid>
{
    protected override void Update(Grid control, Padding widget)
    {
        control.Margin = new Thickness(widget.Thickness.Start, widget.Thickness.Top, widget.Thickness.End,
            widget.Thickness.Bottom);
    }
}