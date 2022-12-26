﻿using System.Collections.Immutable;

namespace WeiPoX.Core.DeclarativeUI.Widgets.Layout;

public record SingleChildPanel(Widget Child) : Panel(ImmutableList.Create(Child))
{
    public virtual bool Equals(SingleChildPanel? other)
    {
        if (ReferenceEquals(null, other))
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return base.Equals(other) && Child.Equals(other.Child);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(base.GetHashCode(), Child);
    }
}