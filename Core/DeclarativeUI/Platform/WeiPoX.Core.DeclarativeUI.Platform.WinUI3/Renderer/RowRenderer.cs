using Microsoft.UI.Xaml.Controls;
using WeiPoX.Core.DeclarativeUI.Platform.WinUI3.Internal;
using WeiPoX.Core.DeclarativeUI.Widgets.Layout;

namespace WeiPoX.Core.DeclarativeUI.Platform.WinUI3.Renderer;

internal class RowRenderer : RendererObject<Row, StackPanel>
{
    protected override StackPanel Create(WidgetBuilder renderer)
    {
        return new StackPanel
        {
            Orientation = Orientation.Horizontal
        };
    }

    protected override void Update(StackPanel control, Row widget)
    {
    }
}