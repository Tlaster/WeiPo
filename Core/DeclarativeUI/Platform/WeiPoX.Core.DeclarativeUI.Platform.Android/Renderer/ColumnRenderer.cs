using Android.Content;
using WeiPoX.Core.DeclarativeUI.Widgets.Layout;

namespace WeiPoX.Core.DeclarativeUI.Platform.Android.Renderer;

internal class ColumnRenderer : RendererObject<Column, LinearLayout>
{
    public ColumnRenderer(Context context) : base(context)
    {
    }

    protected override LinearLayout Create(Context context)
    {
        return new LinearLayout(context)
        {
            Orientation = Orientation.Vertical,
        };
    }

    protected override void Update(LinearLayout control, Column widget)
    {
    }
}