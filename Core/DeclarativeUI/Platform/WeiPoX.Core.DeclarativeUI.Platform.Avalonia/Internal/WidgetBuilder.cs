using Avalonia.Controls;
using WeiPoX.Core.DeclarativeUI.Internal;

namespace WeiPoX.Core.DeclarativeUI.Platform.Avalonia.Internal;

internal class WidgetBuilder : WidgetBuilder<Control>
{
    protected override IRenderer<Control> GetRenderer(Type widgetType)
    {
        return RendererPool.GetRenderer(widgetType);
    }
}