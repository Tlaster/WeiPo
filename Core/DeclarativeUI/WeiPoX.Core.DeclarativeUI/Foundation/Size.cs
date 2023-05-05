namespace WeiPoX.Core.DeclarativeUI.Foundation;

public readonly struct Size
{
    public double Width { get; }
    public double Height { get; }

    public Size(double width, double height)
    {
        Width = width;
        Height = height;
    }

    public static Size operator +(Size left, Size right)
    {
        return new Size(left.Width + right.Width, left.Height + right.Height);
    }

    public static Size operator -(Size left, Size right)
    {
        return new Size(left.Width - right.Width, left.Height - right.Height);
    }

    public static Size operator *(Size left, double right)
    {
        return new Size(left.Width * right, left.Height * right);
    }

    public static Size operator *(double left, Size right)
    {
        return new Size(left * right.Width, left * right.Height);
    }

    public static Size operator /(Size left, double right)
    {
        return new Size(left.Width / right, left.Height / right);
    }

    public static Size operator /(double left, Size right)
    {
        return new Size(left / right.Width, left / right.Height);
    }

    public static bool operator ==(Size left, Size right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Size left, Size right)
    {
        return !left.Equals(right);
    }

    public bool Equals(Size other)
    {
        return Width.Equals(other.Width) && Height.Equals(other.Height);
    }

    public override bool Equals(object? obj)
    {
        return obj is Size other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Width, Height);
    }

    public override string ToString()
    {
        return $"({Width}, {Height})";
    }
}