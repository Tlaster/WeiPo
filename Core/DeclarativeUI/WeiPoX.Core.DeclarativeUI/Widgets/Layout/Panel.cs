using System.Collections;
using System.Collections.Immutable;

namespace WeiPoX.Core.DeclarativeUI.Widgets.Layout;

public record Panel : MappingWidget, IPanelWidget, IEnumerable
{
    public List<Widget> Children { get; } = new();

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

    public IEnumerator GetEnumerator()
    {
        return Children.GetEnumerator();
    }
    
    public void Add(Widget widget)
    {
        Children.Add(widget);
    }

    ImmutableList<Widget> IPanelWidget.Children => Children.ToImmutableList();
}