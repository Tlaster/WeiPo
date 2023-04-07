using System.Collections.Immutable;
using WeiPoX.Core.DeclarativeUI.Foundation;

namespace WeiPoX.Core.DeclarativeUI.Widgets.Layout;

public abstract record LayoutPanel : Panel, ILayoutPanel
{
    protected abstract Func<ILayoutContext, Size> Measure { get; }
    protected abstract Action<ILayoutContext> Arrange { get; }
    
    Size ILayoutPanel.Measure(ILayoutContext availableSize)
    {
        return Measure(availableSize);
    }

    void ILayoutPanel.Arrange(ILayoutContext size)
    {
        Arrange(size);
    }
}

public interface ILayoutContext
{
    Size Size { get; }
    ImmutableList<ILayoutChild> Children { get; }
    double ToNativePixels(double value);
}

public interface ILayoutPanel
{
    Size Measure(ILayoutContext context);
    void Arrange(ILayoutContext size);
}

public interface ILayoutChild
{
    Size Measure(Size availableSize);
    void Arrange(Rect rect);
    Size DesiredSize { get; }
}

