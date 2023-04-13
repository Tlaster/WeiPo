using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using WeiPoX.Core.DeclarativeUI.Internal;
using WeiPoX.Core.DeclarativeUI.Widgets;
using WeiPoX.Core.DeclarativeUI.Widgets.Layout;
using Panel = Microsoft.UI.Xaml.Controls.Panel;

namespace WeiPoX.Core.DeclarativeUI.Platform.WinUI3.Renderer;

internal class LazyColumnRenderer : LazyRendererObject<LazyColumn, WeiPoXItemsRepeater>
{
    protected override void Update(WeiPoXItemsRepeater control, LazyColumn widget)
    {
        control.SetItems(widget);
    }

    protected override WeiPoXItemsRepeater Create(RendererContext<UIElement> context)
    {
        return new WeiPoXItemsRepeater(context);
    }

    protected override bool IsVisible(WeiPoXItemsRepeater control, int index)
    {
        return control.Repeater.TryGetElement(index) is not null;
    }

    protected override UIElement? GetVisibleChild(WeiPoXItemsRepeater control, int index)
    {
        return (control.Repeater.TryGetElement(index) as DeclarativeView)?.Content;
    }

    protected override void UpdateChild(WeiPoXItemsRepeater control, int index, UIElement childControl)
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

internal class WeiPoXItemsRepeater : UserControl
{
    private ILazyWidget? _lazyWidget;

    public WeiPoXItemsRepeater(RendererContext<UIElement> context)
    {
        Repeater = new ItemsRepeater
        {
            ItemTemplate = new WeiPoXElementFactory(context, index => _lazyWidget?.GetBuilder(index),
                index => VisibleIndex.Add(index), index => VisibleIndex.Remove(index))
        };
        Content = new ScrollViewer
        {
            Content = Repeater
        };
    }

    public WeiPoXItemsRepeater()
    {
        throw new NotImplementedException();
    }

    public List<int> VisibleIndex { get; } = new();

    internal ItemsRepeater Repeater { get; }

    public void SetItems(ILazyWidget widget)
    {
        if (_lazyWidget == null || widget.Count != _lazyWidget.Count)
        {
            Repeater.ItemsSource = Enumerable.Range(0, widget.Count);
        }

        _lazyWidget = widget;
    }
}

internal class WeiPoXElementFactory : IElementFactory
{
    private readonly Func<int, Func<Widget>?> _builder;
    private readonly RendererContext<UIElement> _context;
    private readonly Action<int> _onGetElement;
    private readonly Action<int> _onRecycleElement;
    private readonly List<UIElement> _elements = new();

    public WeiPoXElementFactory(RendererContext<UIElement> context, Func<int, Func<Widget>?> builder,
        Action<int> onGetElement, Action<int> onRecycleElement)
    {
        _context = context;
        _builder = builder;
        _onGetElement = onGetElement;
        _onRecycleElement = onRecycleElement;
    }

    public UIElement GetElement(ElementFactoryGetArgs args)
    {
        var element = _elements.FirstOrDefault() ?? new DeclarativeView(_context.BuildOwner);
        _elements.Remove(element);
        var index = args.Data as int? ?? throw new Exception("args.Data is not int");
        if (element is DeclarativeView declarativeView)
        {
            declarativeView.Tag = index;
            declarativeView.Widget = _builder(index)?.Invoke();
        }

        _onGetElement(index);
        return element;
    }

    public void RecycleElement(ElementFactoryRecycleArgs args)
    {
        var element = args.Element as DeclarativeView ?? throw new Exception("args.Element is not DeclarativeView");
        _elements.Add(element);
        _onRecycleElement((int)element.Tag!);
    }
}
