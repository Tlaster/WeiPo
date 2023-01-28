namespace WeiPoX.Core.DeclarativeUI.Widgets;

public abstract record StatelessWidget : StateWidget
{
    protected internal abstract Widget Content { get; }

    public virtual bool Equals(StatelessWidget? other)
    {
        if (ReferenceEquals(null, other))
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return base.Equals(other) && Content.Equals(other.Content);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(base.GetHashCode(), Content);
    }
}