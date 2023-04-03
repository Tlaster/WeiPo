using Android.Content;
using Android.Views;
using WeiPoX.Core.DeclarativeUI.Internal;
using WeiPoX.Core.DeclarativeUI.Platform.Android.Internal;
using WeiPoX.Core.DeclarativeUI.Widgets;
using GestureDetector = WeiPoX.Core.DeclarativeUI.Widgets.GestureDetector;

namespace WeiPoX.Core.DeclarativeUI.Platform.Android.Renderer;

internal class GestureDetectorRenderer : RendererObject<GestureDetector, RelativeLayout>
{
    public GestureDetectorRenderer(Context context) : base(context)
    {
    }

    protected override RelativeLayout Create(Context context, RendererContext<View> rendererContext)
    {
        return new RelativeLayout(context)
        {
            LayoutParameters = new RelativeLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent,
                ViewGroup.LayoutParams.WrapContent)
        };
    }

    protected override void Update(RelativeLayout control, GestureDetector widget)
    {
        if (widget.OnTap != null)
        {
            control.SetOnClickListener(new OnClickListener(widget.OnTap));
        }
        if (widget.OnLongPress != null)
        {
            control.SetOnLongClickListener(new OnLongClickListener(widget.OnLongPress));
        }
        if (widget.OnDoubleTap != null)
        {
            control.SetOnTouchListener(new OnDoubleTapListener(widget.OnDoubleTap));
        }
    }
}