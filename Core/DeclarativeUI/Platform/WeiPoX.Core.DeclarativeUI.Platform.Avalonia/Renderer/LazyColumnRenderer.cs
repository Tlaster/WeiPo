using Avalonia.Controls;
using Avalonia.Styling;
using WeiPoX.Core.DeclarativeUI.Platform.Avalonia.Internal;
using WeiPoX.Core.DeclarativeUI.Widgets;
using WeiPoX.Core.DeclarativeUI.Widgets.Layout;

namespace WeiPoX.Core.DeclarativeUI.Platform.Avalonia.Renderer;

internal class LazyColumnRenderer : LazyRendererObject<LazyColumn, WeiPoXItemsRepeater>
{
    protected override void Update(WeiPoXItemsRepeater control, LazyColumn widget)
    {
        control.SetItems(widget.GenerateActualLazyItems());
    }

    protected override WeiPoXItemsRepeater Create(WidgetBuilder renderer)
    {
        return new WeiPoXItemsRepeater(renderer);
    }

    protected override bool IsVisible(WeiPoXItemsRepeater control, int index)
    {
        return control.Repeater.TryGetElement(index) is not null;
    }

    protected override Control? GetVisibleChild(WeiPoXItemsRepeater control, int index)
    {
        return (control.Repeater.TryGetElement(index) as SubDeclarativeView)?.Content as Control;
    }

    protected override void UpdateChild(WeiPoXItemsRepeater control, int index, Control childControl)
    {
        if (control.Repeater.TryGetElement(index) is SubDeclarativeView item)
        {
            item.UpdateChild(childControl);
        }
    }

}

internal class WeiPoXItemsRepeater : ScrollViewer, IStyleable
{
    Type IStyleable.StyleKey => typeof(ScrollViewer);
    private List<ActualLazyItem> _actualLazyItems = new();

    internal ItemsRepeater Repeater { get; }

    public WeiPoXItemsRepeater(WidgetBuilder renderer)
    {
        Repeater = new ItemsRepeater
        {
            ItemTemplate = new WeiPoXElementFactory(renderer, index => _actualLazyItems[index])
        };
        Content = Repeater;
    }

    public WeiPoXItemsRepeater()
    {
        throw new NotImplementedException();
    }

    public void SetItems(List<ActualLazyItem> generateActualLazyItems)
    {
        if (generateActualLazyItems.Count != _actualLazyItems.Count)
        {
            Repeater.Items = generateActualLazyItems.Select((_, i) => i).ToList();
        }
        _actualLazyItems = generateActualLazyItems;
    }
}

internal class WeiPoXElementFactory : ElementFactory
{
    private const string Key = "WeiPoX";
    private readonly RecyclePool _recyclePool = new();
    private readonly Func<int, ActualLazyItem> _builder;
    private readonly WidgetBuilder _renderer;

    public WeiPoXElementFactory(WidgetBuilder renderer, Func<int, ActualLazyItem> builder)
    {
        _renderer = renderer;
        _builder = builder;
    }

    protected override Control GetElementCore(ElementFactoryGetArgs args)
    {
        var element = _recyclePool.TryGetElement(Key, args.Parent) ?? new SubDeclarativeView(_renderer, _builder);
        if (element is SubDeclarativeView subDeclarativeView)
        {
            subDeclarativeView.SetIndex(args.Index);
        }
        else
        {
            throw new Exception("element is not SubDeclarativeView");
        }
        return element;
    }

    protected override void RecycleElementCore(ElementFactoryRecycleArgs args)
    {
        var element = args.Element!;
        _recyclePool.PutElement(element, Key, args.Parent);
    }
}