using System.Collections.Immutable;

namespace WeiPoX.Core.DeclarativeUI.Widgets.Layout;

public record Panel(ImmutableList<Widget> Children) : MappingWidget, IPanelWidget
{
    public virtual bool Equals(Panel? other)
    {
        if (ReferenceEquals(null, other))
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return base.Equals(other) && Children.SequenceEqual(other.Children);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(base.GetHashCode(), Children);
    }
}