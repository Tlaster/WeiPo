using Microsoft.UI.Xaml.Controls;

namespace WeiPoX.Core.DeclarativeUI.WinUI3.Renderer;

internal class TextRenderer : RendererObject<Text, TextBlock>
{
    protected override void Update(TextBlock control, Text widget)
    {
        control.Text = widget.Content;
    }
}