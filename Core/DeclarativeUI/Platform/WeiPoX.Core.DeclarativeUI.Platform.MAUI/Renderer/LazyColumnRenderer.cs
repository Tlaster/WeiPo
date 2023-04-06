using WeiPoX.Core.DeclarativeUI.Internal;
using WeiPoX.Core.DeclarativeUI.Widgets;
using WeiPoX.Core.DeclarativeUI.Widgets.Layout;

namespace WeiPoX.Core.DeclarativeUI.Platform.MAUI.Renderer;

internal class LazyColumnRenderer : LazyRendererObject<LazyColumn, WeiPoXItemsRepeater>
{
    protected override void Update(WeiPoXItemsRepeater control, LazyColumn widget)
    {
        control.SetItems(widget.GenerateActualLazyItems());
    }

    protected override WeiPoXItemsRepeater Create(RendererContext<View> context)
    {
        return new WeiPoXItemsRepeater(context);
    }

    protected override bool IsVisible(WeiPoXItemsRepeater control, int index)
    {
        
    }

    protected override View? GetVisibleChild(WeiPoXItemsRepeater control, int index)
    {
    }

    protected override void UpdateChild(WeiPoXItemsRepeater control, int index, View childControl)
    {
    }

}

internal class WeiPoXItemsRepeater : ContentView
{
    private readonly RendererContext<View> _context;
    private List<ActualLazyItem> _actualLazyItems = new();

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

    public void SetItems(List<ActualLazyItem> generateActualLazyItems)
    {
        if (generateActualLazyItems.Count != _actualLazyItems.Count)
        {
            CollectionView.ItemsSource = generateActualLazyItems
                .Select((item, index) => new RepeaterItem(() => _actualLazyItems[index].Builder.Invoke(), _context.BuildOwner)).ToList();
        }
        _actualLazyItems = generateActualLazyItems;
    }
}

internal record RepeaterItem(Func<Widget> WidgetBuilder, IBuildOwner BuildOwner);