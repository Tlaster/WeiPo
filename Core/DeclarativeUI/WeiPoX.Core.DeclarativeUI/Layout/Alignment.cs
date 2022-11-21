namespace WeiPoX.Core.DeclarativeUI.Layout;

public record Alignment
{
    public static Horizontal Start { get; } = new();
    public static Horizontal End { get; } = new();
    public static Horizontal CenterHorizontally { get; } = new();
    public static Vertical Top { get; } = new();
    public static Vertical Bottom { get; } = new();
    public static Vertical CenterVertically { get; } = new();

    public static Alignment BottomCenter { get; } = new();
    public static Alignment BottomEnd { get; } = new();
    public static Alignment BottomStart { get; } = new();
    public static Alignment Center { get; } = new();
    public static Alignment CenterEnd { get; } = new();
    public static Alignment CenterStart { get; } = new();
    public static Alignment TopCenter { get; } = new();
    public static Alignment TopEnd { get; } = new();
    public static Alignment TopStart { get; } = new();

    public record Horizontal : Alignment;

    public record Vertical : Alignment;
}