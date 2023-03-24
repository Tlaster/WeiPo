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
    private readonly Context _context;
    private readonly ImmutableDictionary<Type, IRenderer<View>> _renderers;

    public WidgetBuilder(IBuildOwner owner, Context context) : base(owner)
    {
        _context = context;
        _renderers = new Dictionary<Type, IRenderer<View>>
        {
            { typeof(Text), new TextRenderer(_context) },
            { typeof(Box), new BoxRenderer(_context) },
            { typeof(Widgets.Button), new ButtonRenderer(_context) },
            { typeof(Column), new ColumnRenderer(_context) },
            { typeof(Row), new RowRenderer(_context) },
            { typeof(Padding), new PaddingRenderer(_context) },
            { typeof(Input), new InputRenderer(_context) },
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