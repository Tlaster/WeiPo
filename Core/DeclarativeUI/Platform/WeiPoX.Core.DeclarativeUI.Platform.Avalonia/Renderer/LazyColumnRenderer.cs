using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Styling;
using WeiPoX.Core.DeclarativeUI.Internal;
using WeiPoX.Core.DeclarativeUI.Platform.Avalonia.Internal;
using WeiPoX.Core.DeclarativeUI.Widgets;
using WeiPoX.Core.DeclarativeUI.Widgets.Layout;

namespace WeiPoX.Core.DeclarativeUI.Platform.Avalonia.Renderer;

internal class LazyColumnRenderer : LazyRendererObject<LazyColumn, WeiPoXItemsRepeater>
{
    protected override void Update(WeiPoXItemsRepeater control, LazyColumn widget)
    {
        control.SetItems(widget);
    }

    protected override WeiPoXItemsRepeater Create(RendererContext<Control> context)
    {
        return new WeiPoXItemsRepeater(context);
    }

    protected override bool IsVisible(WeiPoXItemsRepeater control, int index)
    {
        return control.Repeater.TryGetElement(index) is not null;
    }

    protected override Control? GetVisibleChild(WeiPoXItemsRepeater control, int index)
    {
        return (control.Repeater.TryGetElement(index) as DeclarativeView)?.Content as Control;
    }

    protected override void UpdateChild(WeiPoXItemsRepeater control, int index, Control childControl)
    {
        if (control.Repeater.TryGetElement(index) is DeclarativeView item)
        {
            item.UpdateChild(childControl);
        }
    }

    protected override Range GetVisibleRange(WeiPoXItemsRepeater control)
    {
        var first = control.VisibleIndex.Count > 0 ? control.VisibleIndex.Min() : -1;
        var last = control.VisibleIndex.Count > 0 ? control.VisibleIndex.Max() : -1;
        return new Range(Math.Max(first, 0), Math.Max(last, 0));
    }
}

internal class WeiPoXItemsRepeater : ScrollViewer
{
    protected override Type StyleKeyOverride { get; } = typeof(ScrollViewer);
    private ILazyWidget? _lazyWidget;
    public List<int> VisibleIndex { get; }= new();

    internal ItemsRepeater Repeater { get; }

    public WeiPoXItemsRepeater(RendererContext<Control> context)
    {
        Repeater = new ItemsRepeater
        {
            ItemTemplate = new WeiPoXElementFactory(context, index => _lazyWidget?.GetBuilder(index),
                index => VisibleIndex.Add(index), index => VisibleIndex.Remove(index))
        };
        Content = Repeater;
    }

    public WeiPoXItemsRepeater()
    {
        throw new NotImplementedException();
    }

    public void SetItems(ILazyWidget widget)
    {
        if (_lazyWidget == null || widget.Count != _lazyWidget.Count)
        {
            Repeater.ItemsSource = Enumerable.Range(0, widget.Count);
        }
        _lazyWidget = widget;
    }
}

internal class WeiPoXElementFactory : ElementFactory
{
    private const string Key = "WeiPoX";
    private readonly RecyclePool _recyclePool = new();
    private readonly Func<int, Func<Widget>?> _builder;
    private readonly RendererContext<Control> _context;
    private readonly Action<int> _onGetElement;
    private readonly Action<int> _onRecycleElement;

    public WeiPoXElementFactory(RendererContext<Control> context, Func<int, Func<Widget>?> builder, Action<int> onGetElement, Action<int> onRecycleElement)
    {
        _context = context;
        _builder = builder;
        _onGetElement = onGetElement;
        _onRecycleElement = onRecycleElement;
    }

    protected override Control GetElementCore(ElementFactoryGetArgs args)
    {
        var element = _recyclePool.TryGetElement(Key, args.Parent) ?? new DeclarativeView(_context.BuildOwner);
        element.Tag = args.Index;
        if (element is DeclarativeView subDeclarativeView)
        {
            subDeclarativeView.Widget = _builder(args.Index)?.Invoke();
        }
        else
        {
            throw new Exception("element is not SubDeclarativeView");
        }
        _onGetElement(args.Index);
        return element;
    }

    protected override void RecycleElementCore(ElementFactoryRecycleArgs args)
    {
        var element = args.Element!;
        _recyclePool.PutElement(element, Key, args.Parent);
        _onRecycleElement((int)element.Tag!);
    }
}