using Avalonia.Controls;

namespace WeiPoX.Core.DeclarativeUI.Avalonia.Renderer;

internal class TextRenderer : RendererObject<Text, TextBlock>
{
    protected override void Update(TextBlock control, Text widget)
    {
        control.Text = widget.Content;
    }
}