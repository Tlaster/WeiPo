using Android.Content;
using Android.Views;
using WeiPoX.Core.DeclarativeUI.Internal;
using WeiPoX.Core.DeclarativeUI.Platform.Android.Internal;
using WeiPoX.Core.DeclarativeUI.Widgets.Layout;

namespace WeiPoX.Core.DeclarativeUI.Platform.Android.Renderer;

internal class BoxRenderer : RendererObject<Box, FrameLayout>
{
    protected override FrameLayout Create(Context context, RendererContext<View> rendererContext)
    {
        return new FrameLayout(context);
    }

    protected override void Update(FrameLayout control, Box widget)
    {
        // control.SetGravity(widget.Horizontal.ToGravityFlags() | widget.Vertical.ToGravityFlags());
    }

    public BoxRenderer(Context context) : base(context)
    {
    }
}