using MAUIColor = Microsoft.Maui.Graphics.Color;
using WeiPoXColor = WeiPoX.Core.DeclarativeUI.Foundation.Color;
using MAUISize = Microsoft.Maui.Graphics.Size;
using MAUIRect = Microsoft.Maui.Graphics.Rect;

namespace WeiPoX.Core.DeclarativeUI.Platform.MAUI.Internal;

internal static class ColorExtension
{
    public static MAUIColor ToColor(this WeiPoXColor color)
    {
        return new MAUIColor(red: color.R, green: color.G, blue: color.B, alpha: color.A);
    }
}

internal static class SizeExtension
{
    public static MAUISize ToSize(this WeiPoX.Core.DeclarativeUI.Foundation.Size size)
    {
        return new MAUISize(width: size.Width, height: size.Height);
    }
    
    public static WeiPoX.Core.DeclarativeUI.Foundation.Size ToSize(this MAUISize size)
    {
        return new WeiPoX.Core.DeclarativeUI.Foundation.Size(width: size.Width, height: size.Height);
    }
}

internal static class RectExtension
{
    public static MAUIRect ToRect(this WeiPoX.Core.DeclarativeUI.Foundation.Rect rect)
    {
        return new MAUIRect(x: rect.X, y: rect.Y, width: rect.Width, height: rect.Height);
    }
    
    public static WeiPoX.Core.DeclarativeUI.Foundation.Rect ToRect(this MAUIRect rect)
    {
        return new WeiPoX.Core.DeclarativeUI.Foundation.Rect(x: rect.X, y: rect.Y, width: rect.Width, height: rect.Height);
    }
}