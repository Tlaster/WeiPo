using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using System.Linq;
using WeiPoX.Core.DeclarativeUI.Platform.WinUI3.Internal;
using WeiPoX.Core.DeclarativeUI.Widgets.Layout;

namespace WeiPoX.Core.DeclarativeUI.Platform.WinUI3.Renderer;

public class BoxRenderer : LayoutPanelRenderer<Box>
{
    protected override void Update(WeiPoXPanel control, Box widget)
    {
        base.Update(control, widget);
        control.Background = new SolidColorBrush(widget.BackgroundColor.ToColor());
    }

    public override bool IsPanel(UIElement value)
    {
        return true;
    }
}
