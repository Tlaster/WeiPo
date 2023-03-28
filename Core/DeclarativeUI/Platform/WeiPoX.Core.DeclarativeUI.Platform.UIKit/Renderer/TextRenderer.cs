using WeiPoX.Core.DeclarativeUI.Internal;
using WeiPoX.Core.DeclarativeUI.Platform.UIKit.Internal;
using WeiPoX.Core.DeclarativeUI.Widgets;

namespace WeiPoX.Core.DeclarativeUI.Platform.UIKit.Renderer;

internal class TextRenderer : RendererObject<Text, UILabel>
{
    protected override void Update(UILabel control, Text widget)
    {
        control.Text = widget.Content;
    }
}