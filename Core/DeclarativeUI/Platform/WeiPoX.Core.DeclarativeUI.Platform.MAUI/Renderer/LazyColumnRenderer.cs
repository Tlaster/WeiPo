using DynamicData;
using WeiPoX.Core.DeclarativeUI.Internal;
using WeiPoX.Core.DeclarativeUI.Widgets;
using WeiPoX.Core.DeclarativeUI.Widgets.Layout;

namespace WeiPoX.Core.DeclarativeUI.Platform.MAUI.Renderer;

internal class LazyColumnRenderer : LazyRendererObject<LazyColumn, WeiPoXItemsRepeater>
{
    protected override void Update(WeiPoXItemsRepeater control, LazyColumn widget)
    {
        control.SetItems(widget);
    }

    protected override WeiPoXItemsRepeater Create(RendererContext<View> context)
    {
        return new WeiPoXItemsRepeater(context);
    }

    protected override bool IsVisible(WeiPoXItemsRepeater control, int index)
    {
        if (control.VisibleItems[index] is { } declarativeView)
        {
            return declarativeView.Content != null;
        }
        return false;
    }

    protected override View? GetVisibleChild(WeiPoXItemsRepeater control, int index)
    {
        if (control.VisibleItems[index] is { } declarativeView)
        {
            return declarativeView.Content;
        }
        return null;
    }

    protected override void UpdateChild(WeiPoXItemsRepeater control, int index, View childControl)
    {
        if (control.VisibleItems[index] is { } declarativeView)
        {
            declarativeView.UpdateChild(childControl);
        }
    }

    protected override Range GetVisibleRange(WeiPoXItemsRepeater control)
    {
        var first = -1;
        var last = -1;
        for (var i = 0; i < control.VisibleItems.Length; i++)
        {
            if (control.VisibleItems[i] is not null && first == -1)
            {
                first = i;
            }
            if (first != -1)
            {
                if (control.VisibleItems[i] is null)
                {
                    last = i - 1;
                    break;
                }
            }
        }
        return new Range(Math.Max(first, 0), Math.Max(last, 0));
    }
}

internal class WeiPoXItemsRepeater : ContentView
{
    private readonly RendererContext<View> _context;
    private ILazyWidget? _lazyWidget;
    public RepeaterDeclarativeView?[] VisibleItems { get; private set; } = Array.Empty<RepeaterDeclarativeView?>();

    internal CollectionView CollectionView { get; }

    public WeiPoXItemsRepeater(RendererContext<View> context)
    {
        _context = context;
        CollectionView = new CollectionView
        {
            ItemTemplate = RepeaterDeclarativeView.GenerateDataTemplate(),
            ItemsLayout = new LinearItemsLayout(ItemsLayoutOrientation.Vertical)
        };
        Content = CollectionView;
    }

    public WeiPoXItemsRepeater()
    {
        throw new NotImplementedException();
    }

    public void SetItems(ILazyWidget widget)
    {
        if (_lazyWidget == null || widget.Count != _lazyWidget.Count)
        {
            VisibleItems = new RepeaterDeclarativeView?[widget.Count];
            CollectionView.ItemsSource = Enumerable.Range(0, widget.Count)
                .Select((item, index) => new RepeaterItem(
                    () => _lazyWidget?.GetBuilder(index)?.Invoke(),
                    (view) => { VisibleItems[index] = view; },
                    (view) => { VisibleItems[index] = null; },
                    _context.BuildOwner));
        }

        _lazyWidget = widget;
    }
}

internal record RepeaterItem(Func<Widget?> WidgetBuilder, Action<RepeaterDeclarativeView> Loaded, Action<RepeaterDeclarativeView> UnLoaded, IBuildOwner BuildOwner);
