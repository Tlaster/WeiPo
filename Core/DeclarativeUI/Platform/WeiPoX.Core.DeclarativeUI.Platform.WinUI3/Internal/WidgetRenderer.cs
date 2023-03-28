using System;
using System.Linq;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using WeiPoX.Core.DeclarativeUI.Internal;

namespace WeiPoX.Core.DeclarativeUI.Platform.WinUI3.Internal;

internal class WidgetBuilder : WidgetBuilder<UIElement>
{
    protected override IRenderer<UIElement> GetRenderer(Type widgetType)
    {
        return RendererPool.GetRenderer(widgetType);
    }
}