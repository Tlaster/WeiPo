using System.Collections.Immutable;
using WeiPoX.Core.DeclarativeUI.Widgets;

namespace WeiPoX.Core.Routing;

public record NavHost(ImmutableList<Route> Routes, string InitialRoute) : StatefulWidget
{
    protected override Widget Build()
    {
        throw new NotImplementedException();
    }
}