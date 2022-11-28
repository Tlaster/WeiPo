using WeiPoX.Core.DeclarativeUI.Internal;

namespace WeiPoX.Core.DeclarativeUI.Mac.Internal;

internal class WidgetRenderer : WidgetRenderer<NSView>
{
    protected override NSView? GetChildAt(NSView control, int index)
    {
        if (control is NSGridView panel)
        {
            return panel.Subviews.ElementAtOrDefault(index);
        }

        return null;
    }

    protected override IRenderer<NSView> GetRenderer(Type widgetType)
    {
        return RendererPool.GetRenderer(widgetType);
    }

    protected override bool IsPanel(NSView value)
    {
        return value is NSGridView or NSStackView;
    }
}