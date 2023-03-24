using Android.Content;
using Android.Views;
using WeiPoX.Core.DeclarativeUI.Internal;
using WeiPoX.Core.DeclarativeUI.Platform.Android.Internal;
using WeiPoX.Core.DeclarativeUI.Widgets;

namespace WeiPoX.Core.DeclarativeUI.Platform.Android.Renderer;

internal class PaddingRenderer : RendererObject<Padding, FrameLayout>
{
    public PaddingRenderer(Context context) : base(context)
    {
    }

    protected override FrameLayout Create(Context context, WidgetBuilder renderer)
    {
        return new FrameLayout(context);
    }

    protected override void Update(FrameLayout control, Padding widget)
    {
        control.SetPaddingRelative(start: Convert.ToInt32(widget.Thickness.Start),
            top: Convert.ToInt32(widget.Thickness.Top),
            end: Convert.ToInt32(widget.Thickness.End),
            bottom: Convert.ToInt32(widget.Thickness.Bottom));
    }
}