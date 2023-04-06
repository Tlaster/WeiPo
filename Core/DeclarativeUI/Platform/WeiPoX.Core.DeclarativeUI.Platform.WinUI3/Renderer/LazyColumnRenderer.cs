using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Markup;
using WeiPoX.Core.DeclarativeUI.Internal;
using WeiPoX.Core.DeclarativeUI.Platform.WinUI3.Internal;
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
        return (control.Repeater.TryGetElement(index) as RepeaterDeclarativeView)?.Content;
    }

    protected override void UpdateChild(WeiPoXItemsRepeater control, int index, UIElement childControl)
    {
        if (control.Repeater.TryGetElement(index) is RepeaterDeclarativeView item)
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
    private readonly RendererContext<UIElement> _context;
    private ILazyWidget? _lazyWidget;
    public List<int> VisibleIndex { get; } = new();

    public WeiPoXItemsRepeater(RendererContext<UIElement> context)
    {
        _context = context;
        Repeater = new ItemsRepeater
        {
            ItemTemplate = RepeaterDeclarativeView.GenerateDataTemplate()
        };
        Repeater.ElementClearing += RepeaterOnElementClearing;
        Repeater.ElementPrepared += RepeaterOnElementPrepared;
        Repeater.ElementIndexChanged += RepeaterOnElementIndexChanged;
        Content = new ScrollViewer
        {
            Content = Repeater
        };
    }

    private void RepeaterOnElementIndexChanged(ItemsRepeater sender, ItemsRepeaterElementIndexChangedEventArgs args)
    {
        VisibleIndex.Remove(args.OldIndex);
        VisibleIndex.Add(args.NewIndex);
    }

    private void RepeaterOnElementPrepared(ItemsRepeater sender, ItemsRepeaterElementPreparedEventArgs args)
    {
        VisibleIndex.Add(args.Index);
    }

    private void RepeaterOnElementClearing(ItemsRepeater sender, ItemsRepeaterElementClearingEventArgs args)
    {
        if (args.Element is RepeaterDeclarativeView repeaterDeclarativeView)
        {
            VisibleIndex.Remove(repeaterDeclarativeView.RepeaterItem.Index);
        }
    }

    public WeiPoXItemsRepeater()
    {
        throw new NotImplementedException();
    }

    internal ItemsRepeater Repeater { get; }

    public void SetItems(ILazyWidget widget)
    {
        if (_lazyWidget == null || widget.Count != _lazyWidget.Count)
        {
            Repeater.ItemsSource = Enumerable.Range(0, widget.Count).Select(i =>
                new RepeaterItem(
                    () => _lazyWidget?.GetBuilder(i)?.Invoke(),
                    _context.BuildOwner,
                    i
                    )
            );
        }
        _lazyWidget = widget;
    }
}

public record RepeaterItem(Func<Widget?> WidgetBuilder, IBuildOwner BuildOwner, int Index);
