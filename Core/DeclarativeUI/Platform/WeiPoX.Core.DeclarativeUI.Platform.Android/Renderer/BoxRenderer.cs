using Android.Content;
using Android.Views;
using WeiPoX.Core.DeclarativeUI.Widgets.Layout;

namespace WeiPoX.Core.DeclarativeUI.Platform.Android.Renderer;

internal class BoxRenderer : RendererObject<Box, FrameLayout>
{
    protected override FrameLayout Create(Context context)
    {
        return new FrameLayout(context);
    }

    protected override void Update(FrameLayout control, Box widget)
    {
    }

    public BoxRenderer(Context context) : base(context)
    {
    }
}