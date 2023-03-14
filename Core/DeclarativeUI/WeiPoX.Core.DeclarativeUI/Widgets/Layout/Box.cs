using System.Collections.Immutable;

namespace WeiPoX.Core.DeclarativeUI.Widgets.Layout;

public record Box : Panel
{
    public Alignment.Horizontal Horizontal { get; init; } = Alignment.Horizontal.Start;
    public Alignment.Vertical Vertical { get; init; } = Alignment.Vertical.Top;
}