using Android.Content;
using Android.Views;
using WeiPoX.Core.DeclarativeUI.Internal;
using WeiPoX.Core.DeclarativeUI.Platform.Android.Internal;
using WeiPoX.Core.DeclarativeUI.Widgets.Layout;

namespace WeiPoX.Core.DeclarativeUI.Platform.Android.Renderer;

internal class RowRenderer : RendererObject<Row, LinearLayout>
{
    public RowRenderer(Context context) : base(context)
    {
    }

    protected override LinearLayout Create(Context context, WidgetBuilder renderer)
    {
        return new LinearLayout(context)
        {
            Orientation = Orientation.Horizontal,
        };
    }

    protected override void Update(LinearLayout control, Row widget)
    {
        control.SetGravity(widget.Horizontal.ToGravityFlags() | widget.Vertical.ToGravityFlags());
    }
}