using System.Collections.Immutable;
using WeiPoX.Core.DeclarativeUI;
using WeiPoX.Core.DeclarativeUI.Widgets;

namespace WeiPoX.Core.Routing;

public record NavHost(Navigator Navigator, string InitialRoute, ImmutableList<Route> Routes) : StatefulWidget
{
    protected override Widget Build()
    {
        UseEffect(() => { Navigator.Init(InitialRoute, Routes); });
        var currentRoute = this.UseObservable(Navigator.CurrentRoute, null);
        if (currentRoute is null)
        {
            return Box();
        }

        UseEffect(() =>
        {
            currentRoute.Active();
            return currentRoute.InActive;
        });
        return Box(
            ContextProvider(
                Providers(
                    (typeof(StateHolder), currentRoute.State),
                    (typeof(Lifecycle), currentRoute.Lifecycle)
                ),
                currentRoute.Route.Content.Invoke(currentRoute)
            )
        );
    }
}

public static class NavigatorExtensions
{
    public static Navigator UseNavigator(this StatefulWidget widget)
    {
        var stateHolder = widget.UseContext<StateHolder>();
        var lifecycle = widget.UseContext<Lifecycle>();
        return widget.UseMemo(() => new Navigator(stateHolder, lifecycle));
    }
}