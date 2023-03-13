using WeiPoX.Core.DeclarativeUI.Widgets;

namespace WeiPoX.Core.DeclarativeUI.Platform.UIKit.Renderer;

internal class TextRenderer : RendererObject<Text, UILabel>
{
    protected override UILabel Create()
    {
        return new UILabel();
    }

    protected override void Update(UILabel control, Text widget)
    {
        control.Text = widget.Content;
    }
}