using WeiPoX.Core.DeclarativeUI.Widgets.Layout;

namespace WeiPoX.Core.DeclarativeUI.Platform.MAUI.Internal;

internal static class AlignmentExtension
{
    public static HorizontalAlignment ToHorizontalAlignment(this Alignment.Horizontal alignment)
    {
        return alignment switch
        {
            Alignment.Horizontal.Start => HorizontalAlignment.Left,
            Alignment.Horizontal.Center => HorizontalAlignment.Center,
            Alignment.Horizontal.End => HorizontalAlignment.Right,
            _ => HorizontalAlignment.Left,
        };
    }
    
    public static VerticalAlignment ToVerticalAlignment(this Alignment.Vertical alignment)
    {
        return alignment switch
        {
            Alignment.Vertical.Top => VerticalAlignment.Top,
            Alignment.Vertical.Center => VerticalAlignment.Center,
            Alignment.Vertical.Bottom => VerticalAlignment.Bottom,
            _ => VerticalAlignment.Top,
        };
    }
    
    public static LayoutOptions ToLayoutOptions(this Alignment.Horizontal alignment)
    {
        return alignment switch
        {
            Alignment.Horizontal.Start => LayoutOptions.Start,
            Alignment.Horizontal.Center => LayoutOptions.Center,
            Alignment.Horizontal.End => LayoutOptions.End,
            _ => LayoutOptions.Start,
        };
    }
    
    public static LayoutOptions ToLayoutOptions(this Alignment.Vertical alignment)
    {
        return alignment switch
        {
            Alignment.Vertical.Top => LayoutOptions.Start,
            Alignment.Vertical.Center => LayoutOptions.Center,
            Alignment.Vertical.Bottom => LayoutOptions.End,
            _ => LayoutOptions.Start,
        };
    }
}