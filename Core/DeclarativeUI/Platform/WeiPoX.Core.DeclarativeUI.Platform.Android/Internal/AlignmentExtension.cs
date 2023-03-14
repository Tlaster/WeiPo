using Android.Views;
using WeiPoX.Core.DeclarativeUI.Widgets.Layout;

namespace WeiPoX.Core.DeclarativeUI.Platform.Android.Internal;

internal static class AlignmentExtension
{
    public static GravityFlags ToGravityFlags(this Alignment.Horizontal alignment)
    {
        return alignment switch
        {
            Alignment.Horizontal.Start => GravityFlags.Start,
            Alignment.Horizontal.Center => GravityFlags.CenterHorizontal,
            Alignment.Horizontal.End => GravityFlags.End,
            _ => GravityFlags.Start,
        };
    }
    
    public static GravityFlags ToGravityFlags(this Alignment.Vertical alignment)
    {
        return alignment switch
        {
            Alignment.Vertical.Top => GravityFlags.Top,
            Alignment.Vertical.Center => GravityFlags.CenterVertical,
            Alignment.Vertical.Bottom => GravityFlags.Bottom,
            _ => GravityFlags.Top,
        };
    }
}