using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
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
        return (control.Repeater.TryGetElement(index) as DeclarativeView)?.Content as Control;
    }

    protected override void UpdateChild(WeiPoXItemsRepeater control, int index, Control childControl)
    {
        if (control.Repeater.TryGetElement(index) is SubDeclarativeView item)
        {
            item.UpdateChild(childControl);
        }
    }
}

internal class WeiPoXItemsRepeater : UserControl
{
    private List<ActualLazyItem> _actualLazyItems = new();

    public WeiPoXItemsRepeater(WidgetBuilder renderer)
    {
        Repeater = new ItemsRepeater
        {
            ItemTemplate = new WeiPoXElementFactory(renderer, index => _actualLazyItems[index])
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
            Repeater.ItemsSource = generateActualLazyItems.Select((_, i) => i).ToList();
        }

        _actualLazyItems = generateActualLazyItems;
    }
}

internal class WeiPoXElementFactory : IElementFactory
{
    private const string Key = "WeiPoX";
    private readonly Func<int, ActualLazyItem> _builder;
    private readonly RecyclePool _recyclePool = new();
    private readonly WidgetBuilder _renderer;

    public WeiPoXElementFactory(WidgetBuilder renderer, Func<int, ActualLazyItem> builder)
    {
        _renderer = renderer;
        _builder = builder;
    }

    public UIElement GetElement(ElementFactoryGetArgs args)
    {
        var element = _recyclePool.TryGetElement(Key, args.Parent) ?? new SubDeclarativeView(_renderer, _builder);
        if (element is SubDeclarativeView subDeclarativeView && args.Data is int index)
        {
            subDeclarativeView.SetIndex(index);
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