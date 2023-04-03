using Android.Graphics;
using AndroidColor = Android.Graphics.Color;
using Color = WeiPoX.Core.DeclarativeUI.Foundation.Color;

namespace WeiPoX.Core.DeclarativeUI.Platform.Android.Internal;

internal static class ColorExtension
{
    public static AndroidColor ToColor(this Color color)
    {
        return new AndroidColor(r: color.R, g: color.G, b: color.B, a: color.A);
    }
    
}