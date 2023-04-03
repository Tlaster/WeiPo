using Avalonia.Controls;
using Avalonia.Layout;
using WeiPoX.Core.DeclarativeUI.Internal;
using WeiPoX.Core.DeclarativeUI.Widgets;

namespace WeiPoX.Core.DeclarativeUI.Platform.Avalonia.Renderer;

internal class TextRenderer : RendererObject<Text, TextBlock>
{
    protected override void Update(TextBlock control, Text widget)
    {
        control.Text = widget.Content;
    }
}