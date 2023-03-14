using System.Collections;
using System.Collections.Immutable;

namespace WeiPoX.Core.DeclarativeUI.Widgets;

public record ContextProvider : StateWidget, IContextProvider, IPanelWidget
{
    public Dictionary<Type, object> Providers { get; } = new();

    public required Widget Child { get; init; }
    public ImmutableList<Widget> Children => new [] { Child }.ToImmutableList();

    public virtual bool Equals(ContextProvider? other)
    {
        if (ReferenceEquals(null, other))
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return base.Equals(other) && Providers.SequenceEqual(other.Providers) && Child.Equals(other.Child);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(base.GetHashCode(), Providers, Child);
    }

    ImmutableDictionary<Type, object> IContextProvider.Providers => Providers.ToImmutableDictionary();
}