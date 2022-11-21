using Avalonia.Controls;
using Avalonia.Layout;
using WeiPoX.Core.DeclarativeUI.Layout;

namespace WeiPoX.Core.DeclarativeUI.Avalonia.Renderer;

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