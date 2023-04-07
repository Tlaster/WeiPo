using Android.Content;
using WeiPoX.Core.DeclarativeUI.Platform.Android.Internal;
using WeiPoX.Core.DeclarativeUI.Widgets.Layout;

namespace WeiPoX.Core.DeclarativeUI.Platform.Android.Renderer;

internal class BoxRenderer : LayoutPanelRenderer<Box>
{
    protected override void Update(WeiPoXPanel control, Box widget)
    {
        base.Update(control, widget);
        control.SetBackgroundColor(widget.BackgroundColor.ToColor());
    }
    
    public BoxRenderer(Context context) : base(context)
    {
    }
}