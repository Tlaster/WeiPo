using System.Collections;
using System.Collections.Immutable;

namespace WeiPoX.Core.DeclarativeUI.Widgets.Layout;

public record Panel : MappingWidget, IPanelWidget, IEnumerable
{
    private readonly List<Widget> _children = new();

    public ImmutableList<Widget> Children => _children.ToImmutableList();

    // init => _children = value;
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

        return base.Equals(other) && _children.SequenceEqual(other._children);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(base.GetHashCode(), _children);
    }

    public IEnumerator GetEnumerator()
    {
        return _children.GetEnumerator();
    }
    
    public void Add(Widget widget)
    {
        _children.Add(widget);
    }
}