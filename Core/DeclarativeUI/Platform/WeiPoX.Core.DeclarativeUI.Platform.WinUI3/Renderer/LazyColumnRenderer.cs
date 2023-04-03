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
        control.SetItems(widget.GenerateActualLazyItems());
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
        return (control.Repeater.TryGetElement(index) as RepeaterDeclarativeView)?.Content as UIElement;
    }

    protected override void UpdateChild(WeiPoXItemsRepeater control, int index, UIElement childControl)
    {
        if (control.Repeater.TryGetElement(index) is RepeaterDeclarativeView item)
        {
            item.UpdateChild(childControl);
        }
    }
}

internal class WeiPoXItemsRepeater : UserControl
{
    private List<ActualLazyItem> _actualLazyItems = new();
    private readonly RendererContext<UIElement> _context;

    public WeiPoXItemsRepeater(RendererContext<UIElement> context)
    {
        _context = context;
        Repeater = new ItemsRepeater
        {
            ItemTemplate = RepeaterDeclarativeView.GenerateDataTemplate()
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

    internal ItemsRepeater Repeater { get; }

    public void SetItems(List<ActualLazyItem> generateActualLazyItems)
    {
        if (generateActualLazyItems.Count != _actualLazyItems.Count)
        {
            Repeater.ItemsSource = generateActualLazyItems
                .Select((item, index) => new RepeaterItem(() => _actualLazyItems[index].Builder.Invoke(), _context.BuildOwner)).ToList();
        }

        _actualLazyItems = generateActualLazyItems;
    }
}

public record RepeaterItem(Func<Widget> WidgetBuilder, IBuildOwner BuildOwner);

internal class WeiPoXElementFactory : IElementFactory
{
    private const string Key = "WeiPoX";
    private readonly Func<int, ActualLazyItem> _builder;
    private readonly RecyclePool _recyclePool = new();
    private readonly RendererContext<UIElement> _context;

    public WeiPoXElementFactory(RendererContext<UIElement> context, Func<int, ActualLazyItem> builder)
    {
        _context = context;
        _builder = builder;
    }

    public UIElement GetElement(ElementFactoryGetArgs args)
    {
        var element = _recyclePool.TryGetElement(Key, args.Parent) ?? new DeclarativeView(_context.BuildOwner);
        if (element is DeclarativeView subDeclarativeView && args.Data is int index)
        {
            subDeclarativeView.Widget = _builder(index).Builder.Invoke();
        }
        else
        {
            throw new Exception("element is not SubDeclarativeView");
        }

        return element;
    }

    public void RecycleElement(ElementFactoryRecycleArgs args)
    {
        var element = args.Element!;
        _recyclePool.PutElement(element, Key, args.Parent);
    }
}

internal class RecyclePool
{
    private readonly Dictionary<string, List<ElementInfo>> _elements = new();

    public void PutElement(UIElement element, string key, UIElement? owner)
    {
        var ownerAsPanel = EnsureOwnerIsPanelOrNull(owner);
        var elementInfo = new ElementInfo(element, ownerAsPanel);

        if (!_elements.TryGetValue(key, out var pool))
        {
            pool = new List<ElementInfo>();
            _elements.Add(key, pool);
        }

        pool.Add(elementInfo);
    }

    public UIElement? TryGetElement(string key, UIElement? owner)
    {
        if (_elements.TryGetValue(key, out var elements))
        {
            if (elements.Count > 0)
            {
                // Prefer an element from the same owner or with no owner so that we don't incur
                // the enter/leave cost during recycling.
                // TODO: prioritize elements with the same owner to those without an owner.
                var elementInfo = elements.FirstOrDefault(x => x.Owner == owner) ?? elements.LastOrDefault();
                elements.Remove(elementInfo!);

                var ownerAsPanel = EnsureOwnerIsPanelOrNull(owner);
                if (elementInfo!.Owner != null && elementInfo.Owner != ownerAsPanel)
                {
                    // Element is still under its parent. remove it from its parent.
                    var panel = elementInfo.Owner;
                    if (panel != null)
                    {
                        var childIndex = panel.Children.IndexOf(elementInfo.Element);
                        if (childIndex == -1)
                        {
                            throw new KeyNotFoundException(
                                "ItemsRepeater's child not found in its Children collection.");
                        }

                        panel.Children.RemoveAt(childIndex);
                    }
                }

                return elementInfo.Element;
            }
        }

        return null;
    }

    private Panel? EnsureOwnerIsPanelOrNull(UIElement? owner)
    {
        if (owner is Panel panel)
        {
            return panel;
        }

        if (owner != null)
        {
            throw new InvalidOperationException("Owner must be IPanel or null.");
        }

        return null;
    }

    private class ElementInfo
    {
        public ElementInfo(UIElement element, Panel? owner)
        {
            Element = element;
            Owner = owner;
        }

        public UIElement Element { get; }
        public Panel? Owner { get; }
    }
}