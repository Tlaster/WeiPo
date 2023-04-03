using WeiPoX.Core.DeclarativeUI.Widgets;
using WeiPoX.Core.Lifecycle;

namespace WeiPoX.Core.DeclarativeUI;

internal record AppWidget : StatefulWidget
{
    public required Widget App { get; init; }
    public required StateHolder StateHolder { get; init; }
    public required LifecycleHolder LifecycleHolder { get; init; }
    protected override Widget Build()
    {
        return new ContextProvider
        {
            Providers =
            {
                {typeof(StateHolder), StateHolder},
                {typeof(LifecycleHolder), LifecycleHolder},
            },
            Child = App,
        };
    }
}