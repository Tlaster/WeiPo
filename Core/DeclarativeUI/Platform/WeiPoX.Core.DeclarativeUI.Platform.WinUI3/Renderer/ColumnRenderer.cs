using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using WeiPoX.Core.DeclarativeUI.Internal;
using WeiPoX.Core.DeclarativeUI.Platform.WinUI3.Internal;
using WeiPoX.Core.DeclarativeUI.Widgets.Layout;

namespace WeiPoX.Core.DeclarativeUI.Platform.WinUI3.Renderer;

internal class ColumnRenderer : RendererObject<Column, StackPanel>
{
    protected override StackPanel Create(RendererContext<UIElement> context)
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