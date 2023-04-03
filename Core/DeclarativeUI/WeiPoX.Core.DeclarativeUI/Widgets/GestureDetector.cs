using WeiPoX.Core.DeclarativeUI.Widgets.Layout;

namespace WeiPoX.Core.DeclarativeUI.Widgets;

public record GestureDetector : Panel
{
    public Action? OnTap { get; init; }
    public Action? OnLongPress { get; init; }
    public Action? OnDoubleTap { get; init; }
}