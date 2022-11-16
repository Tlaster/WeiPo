using Microsoft.UI.Xaml.Controls;

namespace WeiPoX.DeclarativeUI.WinUI3.Renderer;

internal class TextRenderer : RendererObject<Text, TextBlock>
{
    protected internal override void Update(TextBlock control, Text widget)
    {
        control.Text = widget.Content;
    }
}