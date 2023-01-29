using System.Collections.Immutable;

namespace WeiPoX.Core.DeclarativeUI.Widgets;

public record ContextProvider(ImmutableDictionary<Type, object> Providers, Widget Child) : StateWidget,
    IContextProvider, IPanelWidget
{
    public ImmutableList<Widget> Children { get; } = ImmutableList.Create(Child);
}