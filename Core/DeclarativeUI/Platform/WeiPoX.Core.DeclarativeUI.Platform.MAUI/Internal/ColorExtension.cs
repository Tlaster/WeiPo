using MAUIColor = Microsoft.Maui.Graphics.Color;
using WeiPoXColor = WeiPoX.Core.DeclarativeUI.Foundation.Color;

namespace WeiPoX.Core.DeclarativeUI.Platform.MAUI.Internal;

internal static class ColorExtension
{
    public static MAUIColor ToColor(this WeiPoXColor color)
    {
        return new MAUIColor(red: color.R, green: color.G, blue: color.B, alpha: color.A);
    }
}