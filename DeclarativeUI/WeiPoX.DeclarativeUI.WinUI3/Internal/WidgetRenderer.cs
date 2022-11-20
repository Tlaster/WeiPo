﻿using System;
using System.Linq;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using WeiPoX.DeclarativeUI.Internal;

namespace WeiPoX.DeclarativeUI.WinUI3.Internal;

internal class WidgetRenderer : WidgetRenderer<UIElement>
{
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