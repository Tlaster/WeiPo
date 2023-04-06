using System.Collections.Immutable;
using WeiPoX.Core.DeclarativeUI.Internal;
using WeiPoX.Core.DeclarativeUI.Platform.MAUI.Renderer;
using WeiPoX.Core.DeclarativeUI.Widgets;
using WeiPoX.Core.DeclarativeUI.Widgets.Layout;
using Button = WeiPoX.Core.DeclarativeUI.Widgets.Button;

namespace WeiPoX.Core.DeclarativeUI.Platform.MAUI.Internal;

internal static class RendererPool
{
    private static readonly ImmutableDictionary<Type, IRenderer<View>> Renderers =
        new Dictionary<Type, IRenderer<View>>
        {
            { typeof(Text), new TextRenderer() },
            { typeof(Box), new BoxRenderer() },
            { typeof(Button), new ButtonRenderer() },
            { typeof(Column), new ColumnRenderer() },
            { typeof(Row), new RowRenderer() },
            { typeof(GestureDetector), new GestureDetectorRenderer() },
            { typeof(Input), new InputRenderer() },
            { typeof(LazyColumn), new LazyColumnRenderer() },
        }.ToImmutableDictionary();

    public static IRenderer<View> GetRenderer(Type type)
    {
        if (Renderers.TryGetValue(type, out var renderer))
        {
            return renderer;
        }

        throw new NotSupportedException($"Renderer for {type} is not supported.");
    }
}