using Android.Content.Res;
using Android.Util;

namespace WeiPoX.Core.DeclarativeUI.Platform.Android.Internal;

internal static class PlatformExtension
{
    // write an extensions method that cover dp to px
    public static int ToDp(this double dp)
    {
        if (Resources.System != null)
        {
            return (int)TypedValue.ApplyDimension(ComplexUnitType.Dip, (float)dp, Resources.System.DisplayMetrics);
        }
        return (int)dp;
    }
}