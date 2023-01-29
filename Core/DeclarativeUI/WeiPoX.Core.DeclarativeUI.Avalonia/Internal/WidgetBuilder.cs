using Avalonia.Controls;
using WeiPoX.Core.DeclarativeUI.Internal;

namespace WeiPoX.Core.DeclarativeUI.Avalonia.Internal;

internal class WidgetBuilder : WidgetBuilder<IControl>
{
    public WidgetBuilder(IBuildOwner owner) : base(owner)
    {
    }

    protected override IRenderer<IControl> GetRenderer(Type widgetType)
    {
        return RendererPool.GetRenderer(widgetType);
    }

    protected override bool IsPanel(IControl value)
    {
        return value is Panel;
    }

    protected override IControl? GetChildAt(IControl control, int index)
    {
        if (control is Panel panel)
        {
            return panel.Children.ElementAtOrDefault(index);
        }

        return null;
    }
}