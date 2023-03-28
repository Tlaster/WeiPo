using Android.Content;
using Android.Views;
using WeiPoX.Core.DeclarativeUI.Internal;
using WeiPoX.Core.DeclarativeUI.Platform.Android.Internal;
using WeiPoX.Core.DeclarativeUI.Widgets.Layout;

namespace WeiPoX.Core.DeclarativeUI.Platform.Android.Renderer;

internal class ColumnRenderer : RendererObject<Column, LinearLayout>
{
    public ColumnRenderer(Context context) : base(context)
    {
    }

    protected override LinearLayout Create(Context context, RendererContext<View> rendererContext)
    {
        return new LinearLayout(context)
        {
            Orientation = Orientation.Vertical,
        };
    }

    protected override void Update(LinearLayout control, Column widget)
    {
        control.SetGravity(widget.Horizontal.ToGravityFlags() | widget.Vertical.ToGravityFlags());
    }
}
