using System.Collections.Immutable;
using Generator.Equals;
using WeiPoX.Core.DeclarativeUI.Foundation;

namespace WeiPoX.Core.DeclarativeUI.Widgets.Layout;

[Equatable]
public partial record Box : LayoutPanel
{
    public Box()
    {
        Arrange = context =>
        {
            var children = context.Children;
            var size = context.Size;
            var padding = new Thickness(Start: context.ToNativePixels(Padding.Start),
                End: context.ToNativePixels(Padding.End), Top: context.ToNativePixels(Padding.Top),
                Bottom: context.ToNativePixels(Padding.Bottom));
            var width = size.Width - padding.Start - padding.End;
            var height = size.Height - padding.Top - padding.Bottom;
            var x = padding.Start;
            var y = padding.Top;
            foreach (var child in children)
            {
                var childSize = child.DesiredSize;
                var childWidth = childSize.Width;
                var childHeight = childSize.Height;
                var childX = x;
                var childY = y;
                childX += Horizontal switch
                {
                    Alignment.Horizontal.Center => (width - childWidth) / 2,
                    Alignment.Horizontal.End => width - childWidth,
                    Alignment.Horizontal.Start => 0,
                    _ => throw new ArgumentOutOfRangeException()
                };

                childY += Vertical switch
                {
                    Alignment.Vertical.Center => (height - childHeight) / 2,
                    Alignment.Vertical.Bottom => height - childHeight,
                    Alignment.Vertical.Top => 0,
                    _ => throw new ArgumentOutOfRangeException()
                };
                child.Arrange(new Rect(childX, childY, childWidth, childHeight));
            }
        };
        Measure = context =>
        {
            var children = context.Children;
            var availableSize = context.Size;
            var padding = new Thickness(Start: context.ToNativePixels(Padding.Start),
                End: context.ToNativePixels(Padding.End), Top: context.ToNativePixels(Padding.Top),
                Bottom: context.ToNativePixels(Padding.Bottom));
            var width = availableSize.Width - padding.Start - padding.End;
            var height = availableSize.Height - padding.Top - padding.Bottom;
            var maxWidth = 0.0;
            var maxHeight = 0.0;
            foreach (var childSize in children.Select(child => child.Measure(new Size(width, height))))
            {
                maxWidth = Math.Max(maxWidth, childSize.Width);
                maxHeight = Math.Max(maxHeight, childSize.Height);
            }

            maxWidth += padding.Start + padding.End;
            maxHeight += padding.Top + padding.Bottom;
            return new Size(Width, Height) switch
            {
                {Width: > 0, Height: > 0} => new Size(context.ToNativePixels(Width), context.ToNativePixels(Height)),
                {Width: > 0} => new Size(context.ToNativePixels(Width), maxHeight),
                {Height: > 0} => new Size(maxWidth, context.ToNativePixels(Height)),
                _ => new Size(maxWidth, maxHeight)
            };
        };
    }

    public Alignment.Horizontal Horizontal { get; init; } = Alignment.Horizontal.Start;
    public Alignment.Vertical Vertical { get; init; } = Alignment.Vertical.Top;
    
    public double Width { get; init; } = double.NaN;
    public double Height { get; init; } = double.NaN;

    public Color BackgroundColor { get; init; } = Color.Transparent;

    public Thickness Padding { get; init; } = new();
    
    public double Alpha { get; init; } = 1.0;
    public bool ClipBounds { get; init; } = false;
    
    [IgnoreEquality]
    protected override Func<ILayoutContext, Size> Measure { get; }
    
    [IgnoreEquality]
    protected override Action<ILayoutContext> Arrange { get; }
    
    
}