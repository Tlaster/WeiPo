using WeiPoX.Core.DeclarativeUI.Foundation;
using UIKitColor = UIKit.UIColor;
using UIKitSize = CoreGraphics.CGSize;
using UIKitRect = CoreGraphics.CGRect;

namespace WeiPoX.Core.DeclarativeUI.Platform.UIKit.Internal;

internal static class ColorExtension
{
    public static UIKitColor ToColor(this Color color)
    {
        return new UIKitColor(red: color.R / 255f, green: color.G / 255f, blue: color.B / 255f, alpha: color.A / 255f);
    }
}

internal static class SizeExtension
{
    public static UIKitSize ToSize(this Size size)
    {
        return new UIKitSize(width: size.Width, height: size.Height);
    }
    
    public static Size ToSize(this UIKitSize size)
    {
        return new Size(width: size.Width, height: size.Height);
    }
}

internal static class RectExtension
{
    public static UIKitRect ToRect(this Rect rect)
    {
        return new UIKitRect(x: rect.X, y: rect.Y, width: rect.Width, height: rect.Height);
    }
    
    public static Rect ToRect(this UIKitRect rect)
    {
        return new Rect(x: rect.X, y: rect.Y, width: rect.Width, height: rect.Height);
    }
}