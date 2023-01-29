namespace WeiPoX.Core.DeclarativeUI;

public record State<T>(T Value, Action<T> SetValue)
{
    public virtual bool Equals(State<T>? other)
    {
        if (ReferenceEquals(null, other))
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return EqualityComparer<T>.Default.Equals(Value, other.Value);
    }

    public static implicit operator T(State<T> state)
    {
        return state.Value;
    }

    public override int GetHashCode()
    {
        return EqualityComparer<T>.Default.GetHashCode(Value!);
    }
}