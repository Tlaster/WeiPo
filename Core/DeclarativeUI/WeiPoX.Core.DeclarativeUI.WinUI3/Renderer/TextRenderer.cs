using Microsoft.UI.Xaml.Controls;
using WeiPoX.Core.DeclarativeUI.Widget;

namespace WeiPoX.Core.DeclarativeUI.WinUI3.Renderer;

internal class TextRenderer : RendererObject<Text, TextBlock>
{
    protected override void Update(TextBlock control, Text widget)
    {
        control.Text = widget.Content;
    }
}