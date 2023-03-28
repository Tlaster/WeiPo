using System.Collections.Immutable;
using Android.Content;
using Android.Views;
using WeiPoX.Core.DeclarativeUI.Internal;
using WeiPoX.Core.DeclarativeUI.Platform.Android.Renderer;
using WeiPoX.Core.DeclarativeUI.Widgets;
using WeiPoX.Core.DeclarativeUI.Widgets.Layout;

namespace WeiPoX.Core.DeclarativeUI.Platform.Android.Internal;

internal class WidgetBuilder : WidgetBuilder<View>
{
    private readonly ImmutableDictionary<Type, IRenderer<View>> _renderers;

    public WidgetBuilder(Context context)
    {
        _renderers = new Dictionary<Type, IRenderer<View>>
        {
            { typeof(Text), new TextRenderer(context) },
            { typeof(Box), new BoxRenderer(context) },
            { typeof(Widgets.Button), new ButtonRenderer(context) },
            { typeof(Column), new ColumnRenderer(context) },
            { typeof(Row), new RowRenderer(context) },
            { typeof(Padding), new PaddingRenderer(context) },
            { typeof(Input), new InputRenderer(context) },
            { typeof(LazyColumn), new LazyColumnRenderer(context) },
        }.ToImmutableDictionary();
    }

    protected override IRenderer<View> GetRenderer(Type widgetType)
    {
        if (_renderers.TryGetValue(widgetType, out var renderer))
        {
            return renderer;
        }

        throw new NotSupportedException($"Renderer for {widgetType} is not supported.");
    }
}