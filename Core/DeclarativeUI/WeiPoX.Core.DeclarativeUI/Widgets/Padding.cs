using WeiPoX.Core.DeclarativeUI.Widgets.Layout;

namespace WeiPoX.Core.DeclarativeUI.Widgets;

public record Padding : Panel
{
    public required Thickness Thickness { get; init; }
}