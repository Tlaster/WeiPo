using WeiPoX.Core.DeclarativeUI.Widgets;

namespace WeiPoX.Core.DeclarativeUI.Mac.Renderer;

internal class TextRenderer : RendererObject<Text, NSText>
{
    protected override void Update(NSText control, Text widget)
    {
        control.Value = widget.Content;
    }
}