using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Microsoft.UI.Xaml;
using WeiPoX.DeclarativeUI.Internal;
using WeiPoX.DeclarativeUI.Layout;
using WeiPoX.DeclarativeUI.WinUI3.Renderer;

namespace WeiPoX.DeclarativeUI.WinUI3.Internal;

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