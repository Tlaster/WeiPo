using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using WeiPoX.Core.DeclarativeUI.Widgets;

namespace WeiPoX.Core.DeclarativeUI.WinUI3.Renderer;

internal class PaddingRenderer : RendererObject<Padding, Grid>
{
    protected override void Update(Grid control, Padding widget)
    {
        control.Margin = new Thickness(widget.Thickness.Start, widget.Thickness.Top, widget.Thickness.End,
            widget.Thickness.Bottom);
    }
}