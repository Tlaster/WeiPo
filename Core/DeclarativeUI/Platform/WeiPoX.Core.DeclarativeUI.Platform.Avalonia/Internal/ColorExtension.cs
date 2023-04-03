using WeiPoX.Core.DeclarativeUI.Foundation;
using AvaloniaColor = Avalonia.Media.Color;

namespace WeiPoX.Core.DeclarativeUI.Platform.Avalonia.Internal;

internal static class ColorExtension
{
    public static AvaloniaColor ToColor(this Color color)
    {
        return new AvaloniaColor(a: color.A, r: color.R, g: color.G, b: color.B);
    }
}