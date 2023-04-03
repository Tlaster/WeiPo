using System.Collections.Immutable;
using WeiPoX.Core.DeclarativeUI.Foundation;

namespace WeiPoX.Core.DeclarativeUI.Widgets.Layout;

public record Box : Panel
{
    public Alignment.Horizontal Horizontal { get; init; } = Alignment.Horizontal.Start;
    public Alignment.Vertical Vertical { get; init; } = Alignment.Vertical.Top;
    
    public double Width { get; init; } = double.NaN;
    public double Height { get; init; } = double.NaN;
    
    public Color BackgroundColor { get; init; } = Color.Transparent;

    public Thickness Padding { get; init; } = new();
}