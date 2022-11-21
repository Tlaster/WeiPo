using System.Collections.Immutable;
using Avalonia.Controls;
using WeiPoX.Core.DeclarativeUI.Avalonia.Renderer;
using WeiPoX.Core.DeclarativeUI.Internal;
using WeiPoX.Core.DeclarativeUI.Layout;

namespace WeiPoX.Core.DeclarativeUI.Avalonia.Internal;

internal static class RendererPool
{
    private static readonly ImmutableDictionary<Type, IRenderer<IControl>> Renderers =
        new Dictionary<Type, IRenderer<IControl>>
        {
            { typeof(Text), new TextRenderer() },
            { typeof(Box), new BoxRenderer() },
            { typeof(Button), new ButtonRenderer() },
            { typeof(Column), new ColumnRenderer() },
            { typeof(Row), new RowRenderer() },
            { typeof(Padding), new PaddingRenderer() }
        }.ToImmutableDictionary();

    public static IRenderer<IControl> GetRenderer(Type type)
    {
        if (Renderers.TryGetValue(type, out var renderer))
        {
            return renderer;
        }

        throw new NotSupportedException($"Renderer for {type} is not supported.");
    }
}