using System.Collections.Immutable;
using WeiPoX.Core.DeclarativeUI;
using WeiPoX.Core.DeclarativeUI.Foundation;
using WeiPoX.Core.DeclarativeUI.Widgets;
using WeiPoX.Core.DeclarativeUI.Widgets.Layout;
using WeiPoX.Core.Lifecycle;

namespace WeiPoX.Core.Routing;

public record NavHost : StatefulWidget
{
    public required Navigator Navigator { get; init; }
    public required string InitialRoute { get; init; }
    public required ImmutableList<Route> Routes { get; init; }

    protected override Widget Build()
    {
        var stateHolder = UseContext<StateHolder>();
        var lifecycleHolder = UseContext<LifecycleHolder>();
        UseEffect(() => { Navigator.Init(InitialRoute, Routes, stateHolder, lifecycleHolder); }, true);
        var currentRoute = this.UseObservable(Navigator.CurrentRoute, null);
        var canGoBack = this.UseObservable(Navigator.CanGoBack, false);
        if (currentRoute is null)
        {
            return new Box();
        }
        this.UseBackHandler(canGoBack, () => Navigator.Pop());
        UseEffect(() =>
        {
            currentRoute.Active();
            return currentRoute.InActive;
        }, currentRoute);
        return new Box
        {
            new ContextProvider
            {
                Providers =
                {
                    { typeof(StateHolder), currentRoute.State },
                    { typeof(LifecycleHolder), currentRoute.Lifecycle }
                },
                Child = currentRoute.Route.Content.Invoke(currentRoute)
            }
        };
    }
}