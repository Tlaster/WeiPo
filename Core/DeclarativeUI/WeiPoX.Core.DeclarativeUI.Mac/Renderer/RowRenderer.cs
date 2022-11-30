using WeiPoX.Core.DeclarativeUI.Widgets.Layout;

namespace WeiPoX.Core.DeclarativeUI.Mac.Renderer;

internal class RowRenderer : RendererObject<Row, NSStackView>
{
    protected override NSStackView Create()
    {
        return new NSStackView
        {
            Orientation = NSUserInterfaceLayoutOrientation.Horizontal
        };
    }

    protected override void Update(NSStackView control, Row widget)
    {
    }
}