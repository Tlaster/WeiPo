using WeiPoX.Core.DeclarativeUI.Foundation;
using AvaloniaColor = Avalonia.Media.Color;
using AvaloniaSize = Avalonia.Size;
using AvaloniaRect = Avalonia.Rect;

namespace WeiPoX.Core.DeclarativeUI.Platform.Avalonia.Internal;

internal static class ColorExtension
{
    public static AvaloniaColor ToColor(this Color color)
    {
        return new AvaloniaColor(a: color.A, r: color.R, g: color.G, b: color.B);
    }
}

internal static class SizeExtension
{
    public static AvaloniaSize ToSize(this Size size)
    {
        return new AvaloniaSize(width: size.Width, height: size.Height);
    }
    
    public static Size ToSize(this AvaloniaSize size)
    {
        return new Size(width: size.Width, height: size.Height);
    }
}

internal static class RectExtension
{
    public static AvaloniaRect ToRect(this Rect rect)
    {
        return new AvaloniaRect(x: rect.X, y: rect.Y, width: rect.Width, height: rect.Height);
    }
    
    public static Rect ToRect(this AvaloniaRect rect)
    {
        return new Rect(x: rect.X, y: rect.Y, width: rect.Width, height: rect.Height);
    }
}