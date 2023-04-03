using WeiPoX.Core.DeclarativeUI.Foundation;
using WinUIColor = Windows.UI.Color;

namespace WeiPoX.Core.DeclarativeUI.Platform.WinUI3.Internal;

internal static class ColorExtension
{
    public static WinUIColor ToColor(this Color color)
    {
        return WinUIColor.FromArgb(a: color.A, r: color.R, g: color.G, b: color.B);
    }
}