using Avalonia.Controls;
using Avalonia.Layout;
using WeiPoX.Core.DeclarativeUI.Layout;

namespace WeiPoX.Core.DeclarativeUI.Avalonia.Renderer;

internal class RowRenderer : RendererObject<Row, StackPanel>
{
    protected override StackPanel Create()
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