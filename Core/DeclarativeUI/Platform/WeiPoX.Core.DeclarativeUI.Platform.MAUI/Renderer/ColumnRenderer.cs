using WeiPoX.Core.DeclarativeUI.Internal;
using WeiPoX.Core.DeclarativeUI.Platform.MAUI.Internal;
using WeiPoX.Core.DeclarativeUI.Widgets.Layout;

namespace WeiPoX.Core.DeclarativeUI.Platform.MAUI.Renderer;

internal class ColumnRenderer : RendererObject<Column, StackLayout>
{
    protected override StackLayout Create(RendererContext<View> context)
    {
        return new StackLayout
        {
            Orientation = StackOrientation.Vertical,
        };
    }

    protected override void Update(StackLayout control, Column widget)
    {
        control.HorizontalOptions = widget.Horizontal.ToLayoutOptions();
        control.VerticalOptions = widget.Vertical.ToLayoutOptions();
    }
}