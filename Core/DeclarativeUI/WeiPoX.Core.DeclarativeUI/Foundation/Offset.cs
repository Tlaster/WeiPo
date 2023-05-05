namespace WeiPoX.Core.DeclarativeUI.Foundation;

public readonly struct Offset
{
    public double X { get; }
    public double Y { get; }

    public Offset(double x, double y)
    {
        X = x;
        Y = y;
    }
    public static Offset operator +(Offset left, Offset right)
    {
        return new Offset(left.X + right.X, left.Y + right.Y);
    }

    public static Offset operator -(Offset left, Offset right)
    {
        return new Offset(left.X - right.X, left.Y - right.Y);
    }

    public static Offset operator *(Offset left, double right)
    {
        return new Offset(left.X * right, left.Y * right);
    }

    public static Offset operator *(double left, Offset right)
    {
        return new Offset(left * right.X, left * right.Y);
    }

    public static Offset operator /(Offset left, double right)
    {
        return new Offset(left.X / right, left.Y / right);
    }

    public static Offset operator /(double left, Offset right)
    {
        return new Offset(left / right.X, left / right.Y);
    }

    public static bool operator ==(Offset left, Offset right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Offset left, Offset right)
    {
        return !left.Equals(right);
    }

    public bool Equals(Offset other)
    {
        return X.Equals(other.X) && Y.Equals(other.Y);
    }

    public override bool Equals(object? obj)
    {
        return obj is Offset other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y);
    }

    public override string ToString()
    {
        return $"({X}, {Y})";
    }
}