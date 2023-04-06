using WeiPoX.Core.DeclarativeUI.Widgets;

namespace WeiPoX.Core.DeclarativeUI.Platform.MAUI.Renderer;

internal class TextRenderer : RendererObject<Text, Label>
{
    protected override void Update(Label control, Text widget)
    {
        control.Text = widget.Content;
    }
}