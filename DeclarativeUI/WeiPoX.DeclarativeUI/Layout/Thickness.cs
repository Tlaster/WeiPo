namespace WeiPoX.DeclarativeUI.Layout;

public record Thickness(double Start, double Top, double End, double Bottom)
{
    public static readonly Thickness Zero = new(0, 0, 0, 0);

    public Thickness(double value) : this(value, value, value, value)
    {
    }

    public Thickness(double vertical, double horizontal) : this(horizontal, vertical, horizontal, vertical)
    {
    }

    public static Thickness operator +(Thickness left, Thickness right)
    {
        return new Thickness(left.Start + right.Start, left.Top + right.Top, left.End + right.End,
            left.Bottom + right.Bottom);
    }

    public static Thickness operator -(Thickness left, Thickness right)
    {
        return new Thickness(left.Start - right.Start, left.Top - right.Top, left.End - right.End,
            left.Bottom - right.Bottom);
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