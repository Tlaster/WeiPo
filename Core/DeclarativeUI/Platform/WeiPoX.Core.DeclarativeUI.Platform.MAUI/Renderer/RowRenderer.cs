using WeiPoX.Core.DeclarativeUI.Internal;
using WeiPoX.Core.DeclarativeUI.Platform.MAUI.Internal;
using WeiPoX.Core.DeclarativeUI.Widgets.Layout;

namespace WeiPoX.Core.DeclarativeUI.Platform.MAUI.Renderer;

internal class RowRenderer : RendererObject<Row, StackLayout>
{
    protected override StackLayout Create(RendererContext<View> context)
    {
        return new StackLayout
        {
            Orientation = StackOrientation.Horizontal,
        };
    }

    protected override void Update(StackLayout control, Row widget)
    {
        control.HorizontalOptions = widget.Horizontal.ToLayoutOptions();
        control.VerticalOptions = widget.Vertical.ToLayoutOptions();
    }
}