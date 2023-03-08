using Android.Content;
using WeiPoX.Core.DeclarativeUI.Widgets.Layout;

namespace WeiPoX.Core.DeclarativeUI.Platform.Android.Renderer;

internal class RowRenderer : RendererObject<Row, LinearLayout>
{
    public RowRenderer(Context context) : base(context)
    {
    }

    protected override LinearLayout Create(Context context)
    {
        return new LinearLayout(context)
        {
            Orientation = Orientation.Horizontal,
        };
    }

    protected override void Update(LinearLayout control, Row widget)
    {
    }
}