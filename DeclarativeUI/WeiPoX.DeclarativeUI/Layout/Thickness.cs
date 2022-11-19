namespace WeiPoX.DeclarativeUI.Layout;

public class Thickness
{
    public static readonly Thickness Zero = new(0, 0, 0, 0);

    public Thickness(double start, double top, double end, double bottom)
    {
        Start = start;
        Top = top;
        End = end;
        Bottom = bottom;
    }
    
    public Thickness(double value)
    {
        Start = value;
        Top = value;
        End = value;
        Bottom = value;
    }
    
    public Thickness(double vertical, double horizontal)
    {
        Start = horizontal;
        Top = vertical;
        End = horizontal;
        Bottom = vertical;
    }
    
    public double Start { get; }
    public double Top { get; }
    public double End { get; }
    public double Bottom { get; }

    public static Thickness operator +(Thickness left, Thickness right)
    {
        return new Thickness(left.Start + right.Start, left.Top + right.Top, left.End + right.End, left.Bottom + right.Bottom);
    }

    public static Thickness operator -(Thickness left, Thickness right)
    {
        return new Thickness(left.Start - right.Start, left.Top - right.Top, left.End - right.End, left.Bottom - right.Bottom);
    }

    public static Thickness operator *(Thickness left, double right)
    {
        return new Thickness(left.Start * right, left.Top * right, left.End * right, left.Bottom * right);
    }

    public static Thickness operator /(Thickness left, double right)
    {
        return new Thickness(left.Start / right, left.Top / right, left.End / right, left.Bottom / right);
    }

    public override string ToString()
    {
        return $"[{Start}, {Top}, {End}, {Bottom}]";
    }
}