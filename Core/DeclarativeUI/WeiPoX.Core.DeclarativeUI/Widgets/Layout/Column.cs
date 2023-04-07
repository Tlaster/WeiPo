using System.Collections.Immutable;
using Generator.Equals;
using WeiPoX.Core.DeclarativeUI.Foundation;

namespace WeiPoX.Core.DeclarativeUI.Widgets.Layout;

[Equatable]
public partial record Column : LayoutPanel
{
    public Column()
    {
        Arrange = context =>
        {
            var children = context.Children;
            var size = context.Size;
            var width = size.Width;
            var height = size.Height;
            var x = 0.0;
            var y = 0.0;
            foreach (var child in children)
            {
                var childSize = child.DesiredSize;
                var childWidth = childSize.Width;
                var childHeight = childSize.Height;
                var childX = x;
                var childY = y;
                childX += Alignment switch
                {
                    Layout.Alignment.Horizontal.Center => (width - childWidth) / 2,
                    Layout.Alignment.Horizontal.End => width - childWidth,
                    Layout.Alignment.Horizontal.Start => 0,
                    _ => throw new System.ArgumentOutOfRangeException()
                };
                childY += Vertical switch
                {
                    Layout.Alignment.Vertical.Center => (height - childHeight) / 2,
                    Layout.Alignment.Vertical.Bottom => height - childHeight,
                    Layout.Alignment.Vertical.Top => 0,
                    _ => throw new System.ArgumentOutOfRangeException()
                };
                child.Arrange(new Rect(childX, childY, childWidth, childHeight));
                y += childHeight;
            }
        };
    }

    public Alignment.Horizontal Alignment { get; init; } = Layout.Alignment.Horizontal.Start;
    public Alignment.Vertical Vertical { get; init; } = Layout.Alignment.Vertical.Top;
    
    [IgnoreEquality]
    protected override Func<ILayoutContext, Size> Measure { get; } = context =>
    {
        var availableSize = context.Size;
        var children = context.Children;
        var width = 0.0;
        var height = 0.0;
        foreach (var child in children)
        {
            var childSize = child.Measure(availableSize);
            width = System.Math.Max(width, childSize.Width);
            height += childSize.Height;
        }
        return new Size(width, height);
    };
    
    // arrange children depends on Horizontal and Vertical
    [IgnoreEquality]
    protected override Action<ILayoutContext> Arrange { get; }
}