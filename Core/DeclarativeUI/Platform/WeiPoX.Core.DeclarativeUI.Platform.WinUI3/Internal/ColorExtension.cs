using Windows.Devices.WiFiDirect.Services;
using WeiPoX.Core.DeclarativeUI.Foundation;
using WinUIColor = Windows.UI.Color;
using WinUISize = Windows.Foundation.Size;
using WinUIRect = Windows.Foundation.Rect;

namespace WeiPoX.Core.DeclarativeUI.Platform.WinUI3.Internal;

internal static class ColorExtension
{
    public static WinUIColor ToColor(this Color color)
    {
        return WinUIColor.FromArgb(a: color.A, r: color.R, g: color.G, b: color.B);
    }
}

internal static class SizeExtension
{
    public static WinUISize ToSize(this Size size)
    {
        return new WinUISize(width: size.Width, height: size.Height);
    }
    
    public static Size ToSize(this WinUISize size)
    {
        return new Size(width: size.Width, height: size.Height);
    }
}

internal static class RectExtension
{
    public static WinUIRect ToRect(this Rect rect)
    {
        return new WinUIRect(x: rect.X, y: rect.Y, width: rect.Width, height: rect.Height);
    }
    
    public static Rect ToRect(this WinUIRect rect)
    {
        return new Rect(x: rect.X, y: rect.Y, width: rect.Width, height: rect.Height);
    }
}