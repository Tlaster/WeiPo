using WeiPoX.Core.DeclarativeUI.Internal;

namespace WeiPoX.Core.DeclarativeUI.Platform.MAUI.Internal;

public class WidgetBuilder : WidgetBuilder<View>
{
    protected override IRenderer<View> GetRenderer(Type widgetType)
    {
        return RendererPool.GetRenderer(widgetType);
    }
}