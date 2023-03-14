namespace WeiPoX.Core.DeclarativeUI.Widgets.Layout;

public abstract record Alignment
{
    public enum Horizontal
    {
        Start,
        End,
        Center,
    }

    public enum Vertical
    {
        Top,
        Bottom,
        Center,
    } 
}