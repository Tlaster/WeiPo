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
    private static readonly ImmutableDictionary<Type, IRenderer<UIElement>> Renderers =
        new Dictionary<Type, IRenderer<UIElement>>
        {
            { typeof(Text), new TextRenderer() },
            { typeof(Box), new BoxRenderer() },
            { typeof(Button), new ButtonRenderer() },
            { typeof(Column), new ColumnRenderer() },
            { typeof(Row), new RowRenderer() },
            { typeof(Padding), new PaddingRenderer() }
        }.ToImmutableDictionary();

    public static IRenderer<UIElement> GetRenderer(Type type)
    {
        if (Renderers.TryGetValue(type, out var renderer))
        {
            return renderer;
        }

        throw new NotSupportedException($"Renderer for {type} is not supported.");
    }
}