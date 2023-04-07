using System.Collections.Immutable;
using Windows.Foundation;
using Microsoft.UI.Xaml;
using WeiPoX.Core.DeclarativeUI.Platform.WinUI3.Internal;
using WeiPoX.Core.DeclarativeUI.Widgets.Layout;
using Rect = WeiPoX.Core.DeclarativeUI.Foundation.Rect;
using WinUIPanel = Microsoft.UI.Xaml.Controls.Panel;

namespace WeiPoX.Core.DeclarativeUI.Platform.WinUI3.Renderer;

public abstract class LayoutPanelRenderer<T> : RendererObject<T, WeiPoXPanel> where T: LayoutPanel
{
    protected override void Update(WeiPoXPanel control, T widget)
    {
        control.Panel = widget;
    }
}

public class WeiPoXPanel : WinUIPanel
{
    private ILayoutPanel? _panel;

    public ILayoutPanel? Panel
    {
        get => _panel;
        set
        {
            if (value != null && value != _panel)
            {
                _panel = value;
                InvalidateMeasure();
                InvalidateArrange();
            }
            else
            {
                _panel = value;
            }
        }
    }

    protected override Size MeasureOverride(Size availableSize)
    {
        if (_panel == null || Children.Count == 0)
        {
            return base.MeasureOverride(availableSize);
        }
        else
        {
            return _panel.Measure(new LayoutContext(availableSize.ToSize(), GetChildren())).ToSize();
        }
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
        if (_panel == null || Children.Count == 0)
        {
            return base.ArrangeOverride(finalSize);
        }
        else
        {
            _panel.Arrange(new LayoutContext(finalSize.ToSize(), GetChildren()));
            return finalSize;
        }
    }
    
    private ImmutableList<ILayoutChild> GetChildren()
    {
        var builder = ImmutableList.CreateBuilder<ILayoutChild>();
        foreach (var child in Children)
        {
            if (child != null)
            {
                builder.Add(new LayoutChild(child));
            }
        }
        return builder.ToImmutable();
    }
}

internal class LayoutChild : ILayoutChild
{
    private readonly UIElement _element;

    public LayoutChild(UIElement element)
    {
        _element = element;
    }

    public Foundation.Size Measure(Foundation.Size availableSize)
    {
        _element.Measure(availableSize.ToSize());
        return _element.DesiredSize.ToSize();
    }

    public void Arrange(Rect rect)
    {
        _element.Arrange(rect.ToRect());
    }

    public Foundation.Size DesiredSize => _element.DesiredSize.ToSize();
}

internal class LayoutContext : ILayoutContext
{
    public LayoutContext(Foundation.Size size, ImmutableList<ILayoutChild> children)
    {
        Size = size;
        Children = children;
    }

    public Foundation.Size Size { get; }

    public ImmutableList<ILayoutChild> Children { get; }

    public double ToNativePixels(double value)
    {
        return value;
    }
}