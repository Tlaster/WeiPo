using System.Collections.Immutable;
using Microsoft.Maui.Layouts;
using WeiPoX.Core.DeclarativeUI.Platform.MAUI.Internal;
using WeiPoX.Core.DeclarativeUI.Widgets.Layout;
using Rect = WeiPoX.Core.DeclarativeUI.Foundation.Rect;
using MAUIPanel = Microsoft.Maui.Controls.Layout;

namespace WeiPoX.Core.DeclarativeUI.Platform.MAUI.Renderer;

public abstract class LayoutPanelRenderer<T> : RendererObject<T, WeiPoXPanel> where T: LayoutPanel
{
    protected override void Update(WeiPoXPanel control, T widget)
    {
        control.Panel = widget;
    }
}

public class WeiPoXPanel : MAUIPanel, ILayoutManager
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
            }
            else
            {
                _panel = value;
            }
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

    protected override ILayoutManager CreateLayoutManager()
    {
        return this;
    }

    public Size Measure(double widthConstraint, double heightConstraint)
    {
        if (_panel == null || Children.Count == 0)
        {
            return new Size(widthConstraint, heightConstraint);
        }
        else
        {
            return _panel.Measure(new LayoutContext(new WeiPoX.Core.DeclarativeUI.Foundation.Size(widthConstraint, heightConstraint), GetChildren())).ToSize();
        }
    }

    public Size ArrangeChildren(Microsoft.Maui.Graphics.Rect bounds)
    {
        if (_panel == null || Children.Count == 0)
        {
            return new Size(bounds.Width, bounds.Height);
        }
        else
        {
            _panel.Arrange(new LayoutContext(
                new WeiPoX.Core.DeclarativeUI.Foundation.Size(bounds.Width, bounds.Height), GetChildren()));
            return new Size(bounds.Width, bounds.Height);
        }
    }
}

internal class LayoutChild : ILayoutChild
{
    private readonly IView _element;

    public LayoutChild(IView element)
    {
        _element = element;
    }

    public Foundation.Size Measure(Foundation.Size availableSize)
    {
        _element.Measure(availableSize.Width, availableSize.Height);
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