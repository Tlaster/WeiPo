using System.Collections.Immutable;
using ReactiveUI;
using WeiPoX.Core.DeclarativeUI;
using WeiPoX.Core.DeclarativeUI.Widgets;

namespace WeiPoX.Core.Routing;

public record NavHost(Navigator Navigator, string InitialRoute, ImmutableList<Route> Routes) : StatefulWidget
{
    protected override Widget Build()
    {
        UseEffect(() =>
        {
            Navigator.Init(InitialRoute, Routes);
        });
        var currentRoute = this.UseObservable(Navigator.CurrentRoute, null);
        
    }
}


public static class NavigatorExtensions
{
    public static Navigator UseNavigator(this StatefulWidget widget)
    {
        return widget.UseMemo(() => new Navigator());
    }
}