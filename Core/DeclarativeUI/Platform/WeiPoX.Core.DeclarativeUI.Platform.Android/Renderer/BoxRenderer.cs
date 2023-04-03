using Android.Content;
using Android.Views;
using WeiPoX.Core.DeclarativeUI.Internal;
using WeiPoX.Core.DeclarativeUI.Platform.Android.Internal;
using WeiPoX.Core.DeclarativeUI.Widgets.Layout;

namespace WeiPoX.Core.DeclarativeUI.Platform.Android.Renderer;

internal class BoxRenderer : RendererObject<Box, RelativeLayout>
{
    protected override RelativeLayout Create(Context context, RendererContext<View> rendererContext)
    {
        return new RelativeLayout(context)
        {
            LayoutParameters = new RelativeLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent,
                ViewGroup.LayoutParams.WrapContent)
        };
    }

    protected override void Update(RelativeLayout control, Box widget)
    {
        if (!double.IsNaN(widget.Height) && !double.IsInfinity(widget.Height) && control.LayoutParameters != null)
        {
            control.LayoutParameters.Height = widget.Height.ToDp();
        }
        if (!double.IsNaN(widget.Width) && !double.IsInfinity(widget.Width) && control.LayoutParameters != null)
        {
            control.LayoutParameters.Width = widget.Width.ToDp();
        }
        control.SetBackgroundColor(widget.BackgroundColor.ToColor());
        control.SetGravity(widget.Horizontal.ToGravityFlags() | widget.Vertical.ToGravityFlags());
        control.SetPaddingRelative(start: Convert.ToInt32(widget.Padding.Start),
            top: Convert.ToInt32(widget.Padding.Top),
            end: Convert.ToInt32(widget.Padding.End),
            bottom: Convert.ToInt32(widget.Padding.Bottom));
    }

    public BoxRenderer(Context context) : base(context)
    {
    }
}