using WeiPoX.Core.DeclarativeUI.Foundation;
using WeiPoX.Core.DeclarativeUI.Widgets;
using WeiPoX.Core.Lifecycle;

namespace WeiPoX.Core.DeclarativeUI;

internal record AppWidget : StatefulWidget
{
    public required Widget App { get; init; }
    public AppState AppState { get; init; } = new();
    protected override Widget Build()
    {
        return new ContextProvider
        {
            Providers =
            {
                {typeof(StateHolder), AppState.StateHolder},
                {typeof(LifecycleHolder), AppState.LifecycleHolder},
                {typeof(BackDispatcher), AppState.BackDispatcher}
            },
            Child = App,
        };
    }
}

internal record AppState
{
    public StateHolder StateHolder { get; init; } = new();
    public LifecycleHolder LifecycleHolder { get; init; } = new();
    public BackDispatcher BackDispatcher { get; init; } = new();
};