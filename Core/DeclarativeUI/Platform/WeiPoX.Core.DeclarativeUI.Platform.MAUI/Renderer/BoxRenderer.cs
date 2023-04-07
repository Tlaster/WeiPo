using WeiPoX.Core.DeclarativeUI.Platform.MAUI.Internal;
using WeiPoX.Core.DeclarativeUI.Widgets.Layout;
using MAUIThickness = Microsoft.Maui.Thickness;

namespace WeiPoX.Core.DeclarativeUI.Platform.MAUI.Renderer;

internal class BoxRenderer : LayoutPanelRenderer<Box>
{
    protected override void Update(WeiPoXPanel control, Box widget)
    {
        base.Update(control, widget);
        control.Background = new SolidColorBrush(widget.BackgroundColor.ToColor());
    }


    public override bool IsPanel(View value)
    {
        return true;
    }
}
