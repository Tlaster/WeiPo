using Microsoft.UI.Xaml.Controls;
using WeiPoX.DeclarativeUI.Layout;

namespace WeiPoX.DeclarativeUI.WinUI3.Renderer;

internal class ColumnRenderer : RendererObject<Column, StackPanel>
{
    protected override StackPanel Create()
    {
        return new StackPanel
        {
            Orientation = Orientation.Vertical
        };
    }

    protected override void Update(StackPanel control, Column widget)
    {
    }
}