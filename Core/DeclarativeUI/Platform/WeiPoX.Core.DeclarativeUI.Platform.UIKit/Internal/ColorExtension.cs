using WeiPoX.Core.DeclarativeUI.Foundation;
using UIKitColor = UIKit.UIColor;

namespace WeiPoX.Core.DeclarativeUI.Platform.UIKit.Internal;

internal static class ColorExtension
{
    public static UIKitColor ToColor(this Color color)
    {
        return new UIKitColor(red: color.R / 255f, green: color.G / 255f, blue: color.B / 255f, alpha: color.A / 255f);
    }
}