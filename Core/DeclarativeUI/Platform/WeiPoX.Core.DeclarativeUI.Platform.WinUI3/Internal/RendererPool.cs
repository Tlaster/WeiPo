using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Microsoft.UI.Xaml;
using WeiPoX.Core.DeclarativeUI.Internal;
using WeiPoX.Core.DeclarativeUI.Platform.WinUI3.Renderer;
using WeiPoX.Core.DeclarativeUI.Widgets;
using WeiPoX.Core.DeclarativeUI.Widgets.Layout;

namespace WeiPoX.Core.DeclarativeUI.Platform.WinUI3.Internal;

internal static class RendererPool
{
    private static readonly Dictionary<Type, IRenderer<UIElement>> Renderers =
        new()
        {
            { typeof(Text), new TextRenderer() },
            { typeof(Box), new BoxRenderer() },
            { typeof(Button), new ButtonRenderer() },
            { typeof(Column), new ColumnRenderer() },
            { typeof(Row), new RowRenderer() },
            { typeof(GestureDetector), new GestureDetectorRenderer() },
            { typeof(Input), new InputRenderer() },
            { typeof(LazyColumn), new LazyColumnRenderer() },
        };

    public static IRenderer<UIElement> GetRenderer(Type type)
    {
        if (Renderers.TryGetValue(type, out var renderer))
        {
            return renderer;
        }

        throw new NotSupportedException($"Renderer for {type} is not supported.");
    }
    
    public static void RegisterRenderer(Type type, IRenderer<UIElement> renderer)
    {
        Renderers[type] = renderer;
    }
}