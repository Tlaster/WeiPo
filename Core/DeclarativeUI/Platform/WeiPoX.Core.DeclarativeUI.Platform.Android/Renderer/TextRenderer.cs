using Android.Content;
using WeiPoX.Core.DeclarativeUI.Widgets;

namespace WeiPoX.Core.DeclarativeUI.Platform.Android.Renderer;

internal class TextRenderer : RendererObject<Text, TextView>
{

    public TextRenderer(Context context) : base(context)
    {
    }

    protected override TextView Create(Context context)
    {
        return new TextView(context);
    }

    protected override void Update(TextView control, Text widget)
    {
        control.Text = widget.Content;
    }
}