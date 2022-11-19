using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace WeiPoX.DeclarativeUI.WinUI3.Renderer;

internal class PaddingRenderer : RendererObject<Padding, Grid>
{
    protected override void Update(Grid control, Padding widget)
    {
        control.Margin = new Thickness(left: widget.Thickness.Start, top: widget.Thickness.Top, right: widget.Thickness.End, bottom: widget.Thickness.Bottom);
    }
}