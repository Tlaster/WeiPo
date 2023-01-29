using System;
using System.Linq;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using WeiPoX.Core.DeclarativeUI.Internal;

namespace WeiPoX.Core.DeclarativeUI.WinUI3.Internal;

internal class WidgetBuilder : WidgetBuilder<UIElement>
{
    public WidgetBuilder(IBuildOwner owner) : base(owner)
    {
    }

    protected override UIElement? GetChildAt(UIElement control, int index)
    {
        if (control is Panel panel)
        {
            return panel.Children.ElementAtOrDefault(index);
        }

        return null;
    }

    protected override IRenderer<UIElement> GetRenderer(Type widgetType)
    {
        return RendererPool.GetRenderer(widgetType);
    }

    protected override bool IsPanel(UIElement value)
    {
        return value is Panel;
    }
}