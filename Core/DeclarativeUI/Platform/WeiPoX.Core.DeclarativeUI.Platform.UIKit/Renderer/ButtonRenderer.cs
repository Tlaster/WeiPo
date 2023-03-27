using WeiPoX.Core.DeclarativeUI.Platform.UIKit.Internal;
using WeiPoX.Core.DeclarativeUI.Widgets;

namespace WeiPoX.Core.DeclarativeUI.Platform.UIKit.Renderer;

internal class ButtonRenderer : RendererObject<Button, UIButton>
{
    protected override UIButton Create(WidgetBuilder renderer)
    {
        return new UIButton(UIButtonType.System);
    }

    protected override void Update(UIButton control, Button widget)
    {
        control.SetTitle(widget.Text, UIControlState.Normal);
        control.RemoveTarget(null, null, UIControlEvent.TouchUpInside);
        control.AddTarget((_, _) => widget.OnClick.Invoke(), UIControlEvent.TouchUpInside);
    }
}