using WeiPoX.Core.DeclarativeUI.Platform.UIKit.Internal;
using WeiPoX.Core.DeclarativeUI.Widgets.Layout;

namespace WeiPoX.Core.DeclarativeUI.Platform.UIKit.Renderer;

internal class BoxRenderer : LayoutPanelRenderer<Box>
{
    protected override void Update(WeiPoXPanel control, Box widget)
    {
        base.Update(control, widget);
        control.BackgroundColor = widget.BackgroundColor.ToColor();
    }
}
