namespace WeiPoX.Core.DeclarativeUI.Foundation;

public struct Rect
{
    public double X { get; }
    public double Y { get; }
    public double Width { get; }
    public double Height { get; }

    public Rect(double x, double y, double width, double height)
    {
        X = x;
        Y = y;
        Width = width;
        Height = height;
    }

    public static Rect operator +(Rect left, Rect right)
    {
        return new Rect(left.X + right.X, left.Y + right.Y, left.Width + right.Width, left.Height + right.Height);
    }

    public static Rect operator -(Rect left, Rect right)
    {
        return new Rect(left.X - right.X, left.Y - right.Y, left.Width - right.Width, left.Height - right.Height);
    }

    public static Rect operator *(Rect left, double right)
    {
        return new Rect(left.X * right, left.Y * right, left.Width * right, left.Height * right);
    }

    public static Rect operator *(double left, Rect right)
    {
        return new Rect(left * right.X, left * right.Y, left * right.Width, left * right.Height);
    }

    public static Rect operator /(Rect left, double right)
    {
        return new Rect(left.X / right, left.Y / right, left.Width / right, left.Height / right);
    }

    public static Rect operator /(double left, Rect right)
    {
        return new Rect(left / right.X, left / right.Y, left / right.Width, left / right.Height);
    }

    public static bool operator ==(Rect left, Rect right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Rect left, Rect right)
    {
        return !left.Equals(right);
    }

    public bool Equals(Rect other)
    {
        return X.Equals(other.X) && Y.Equals(other.Y) && Width.Equals(other.Width) && Height.Equals(other.Height);
    }

    public override bool Equals(object? obj)
    {
        return obj is Rect other && Equals(other);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = X.GetHashCode();
            hashCode = (hashCode * 397) ^ Y.GetHashCode();
            hashCode = (hashCode * 397) ^ Width.GetHashCode();
            hashCode = (hashCode * 397) ^ Height.GetHashCode();
            return hashCode;
        }
    }
}