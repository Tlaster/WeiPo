using WeiPoX.Core.DeclarativeUI.Widget.Layout;

namespace WeiPoX.Core.DeclarativeUI.Mac.Renderer;

internal class ColumnRenderer : RendererObject<Column, NSStackView>
{
    protected override NSStackView Create()
    {
        return new NSStackView
        {
            Orientation = NSUserInterfaceLayoutOrientation.Vertical,
        };
    }

    protected override void Update(NSStackView control, Column widget)
    {
    }
}