using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using WeiPoX.DeclarativeUI.Layout;
using WeiPoX.DeclarativeUI.WinUI3.Renderer;

namespace WeiPoX.DeclarativeUI.WinUI3.Internal;

internal static class RendererPool
{
    private static readonly ImmutableDictionary<Type, IRenderer> Renderers = new Dictionary<Type, IRenderer>
    {
        { typeof(Text), new TextRenderer() },
        { typeof(Box), new BoxRenderer() },
        { typeof(Button), new ButtonRenderer() },
        { typeof(Column), new ColumnRenderer() },
        { typeof(Row), new RowRenderer() }
    }.ToImmutableDictionary();

    public static IRenderer GetRenderer<T>() where T : WidgetObject
    {
        return Renderers[typeof(T)];
    }

    public static IRenderer GetRenderer(Type type)
    {
        return Renderers[type];
    }
}