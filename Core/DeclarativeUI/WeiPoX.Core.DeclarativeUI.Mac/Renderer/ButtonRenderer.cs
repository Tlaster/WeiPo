using WeiPoX.Core.DeclarativeUI.Widgets;

namespace WeiPoX.Core.DeclarativeUI.Mac.Renderer;

internal class ButtonRenderer : RendererObject<Button, NSButton>
{
    protected override void Update(NSButton control, Button widget)
    {
        // control.Action = new ObjCRuntime.Selector("OnClicked");
    }
}