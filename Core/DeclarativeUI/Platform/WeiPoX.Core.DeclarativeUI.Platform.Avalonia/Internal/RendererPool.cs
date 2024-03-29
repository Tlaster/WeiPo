using System.Collections.Immutable;
using Avalonia.Controls;
using WeiPoX.Core.DeclarativeUI.Internal;
using WeiPoX.Core.DeclarativeUI.Platform.Avalonia.Renderer;
using WeiPoX.Core.DeclarativeUI.Widgets;
using WeiPoX.Core.DeclarativeUI.Widgets.Layout;
using Button = WeiPoX.Core.DeclarativeUI.Widgets.Button;

namespace WeiPoX.Core.DeclarativeUI.Platform.Avalonia.Internal;

internal static class RendererPool
{
    private static readonly Dictionary<Type, IRenderer<Control>> Renderers =
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

    public static IRenderer<Control> GetRenderer(Type type)
    {
        if (Renderers.TryGetValue(type, out var renderer))
        {
            return renderer;
        }

        throw new NotSupportedException($"Renderer for {type} is not supported.");
    }
    
    public static void RegisterRenderer(Type type, IRenderer<Control> renderer)
    {
        Renderers[type] = renderer;
    }
}