using System.Collections.Immutable;
using WeiPoX.Core.DeclarativeUI.Internal;
using WeiPoX.Core.DeclarativeUI.Platform.UIKit.Renderer;
using WeiPoX.Core.DeclarativeUI.Widgets;
using WeiPoX.Core.DeclarativeUI.Widgets.Layout;

namespace WeiPoX.Core.DeclarativeUI.Platform.UIKit.Internal;

internal static class RendererPool
{
    private static readonly ImmutableDictionary<Type, IRenderer<UIView>> Renderers =
        new Dictionary<Type, IRenderer<UIView>>
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

    public static IRenderer<UIView> GetRenderer(Type type)
    {
        if (Renderers.TryGetValue(type, out var renderer))
        {
            return renderer;
        }

        throw new NotSupportedException($"Renderer for {type} is not supported.");
    }
}