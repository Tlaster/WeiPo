using WeiPoX.Core.DeclarativeUI.Internal;

namespace WeiPoX.Core.DeclarativeUI.Platform.UIKit.Internal;

internal class WidgetBuilder : WidgetBuilder<UIView>
{
    public WidgetBuilder(IBuildOwner owner) : base(owner)
    {
    }

    protected override IRenderer<UIView> GetRenderer(Type widgetType)
    {
        return RendererPool.GetRenderer(widgetType);
    }
}