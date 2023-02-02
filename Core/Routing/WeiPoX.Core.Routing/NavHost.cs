using System.Collections.Immutable;
using WeiPoX.Core.DeclarativeUI;
using WeiPoX.Core.DeclarativeUI.Widgets;
using WeiPoX.Core.Lifecycle;

namespace WeiPoX.Core.Routing;

public record NavHost(Navigator Navigator, string InitialRoute, ImmutableList<Route> Routes) : StatefulWidget
{
    protected override Widget Build()
    {
        var stateHolder = UseContext<StateHolder>();
        var lifecycleHolder = UseContext<LifecycleHolder>();
        UseEffect(() => { Navigator.Init(InitialRoute, Routes, stateHolder, lifecycleHolder); }, true);
        var currentRoute = this.UseObservable(Navigator.CurrentRoute, null);
        if (currentRoute is null)
        {
            return Box();
        }

        UseEffect(() =>
        {
            currentRoute.Active();
            return currentRoute.InActive;
        }, currentRoute);
        return Box(
            ContextProvider(
                Providers(
                    (typeof(StateHolder), currentRoute.State),
                    (typeof(LifecycleHolder), currentRoute.Lifecycle)
                ),
                currentRoute.Route.Content.Invoke(currentRoute)
            )
        );
    }
}
