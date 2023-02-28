using WeiPoX.Core.DeclarativeUI.Widgets;

namespace WeiPoX.Core.DeclarativeUI.Platform.Mac.Renderer;

internal class TextRenderer : RendererObject<Text, NSTextField>
{
    protected override NSTextField Create()
    {
        return new NSTextField
        {
            Editable = false
        };
    }

    protected override void Update(NSTextField control, Text widget)
    {
        control.StringValue = widget.Content;
    }
}