using System.Collections;
using System.Collections.Immutable;

namespace WeiPoX.DeclarativeUI.Layout;

public record Panel(ImmutableList<WidgetObject> Children) : WidgetObject, IPanelWidget
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