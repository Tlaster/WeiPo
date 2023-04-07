using System.Collections.Immutable;
using WeiPoX.Core.DeclarativeUI.Platform.UIKit.Internal;
using WeiPoX.Core.DeclarativeUI.Widgets.Layout;
using Rect = WeiPoX.Core.DeclarativeUI.Foundation.Rect;

namespace WeiPoX.Core.DeclarativeUI.Platform.UIKit.Renderer;

public abstract class LayoutPanelRenderer<T> : RendererObject<T, WeiPoXPanel> where T: LayoutPanel
{
    protected override void Update(WeiPoXPanel control, T widget)
    {
        control.Panel = widget;
    }
}

public class WeiPoXPanel : UIView
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
                SetNeedsLayout();
            }
            else
            {
                _panel = value;
            }
        }
    }

    public override void LayoutSubviews()
    {
        if (_panel == null || Subviews.Length == 0)
        {
            return;
        }

        var children = GetChildren();
        var size = _panel.Measure(new LayoutContext(new Foundation.Size(Bounds.Width, Bounds.Height), children));
        this.Frame = new CoreGraphics.CGRect(this.Frame.X, this.Frame.Y, size.Width, size.Height);
        _panel.Arrange(new LayoutContext(size, children));
    }
    
    private ImmutableList<ILayoutChild> GetChildren()
    {
        return Subviews.Select(x => new LayoutChild(x) as ILayoutChild).ToImmutableList();
    }

}

internal class LayoutChild : ILayoutChild
{
    public UIView Element { get; }

    public LayoutChild(UIView element)
    {
        Element = element;
    }

    public Foundation.Size Measure(Foundation.Size availableSize)
    {
        var size = Element.SizeThatFits(availableSize.ToSize());
        DesiredSize = new Foundation.Size(size.Width, size.Height);
        return new Foundation.Size(size.Width, size.Height);
    }

    public void Arrange(Rect rect)
    {
        Element.Frame = rect.ToRect();
    }

    public Foundation.Size DesiredSize { get; private set; }
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