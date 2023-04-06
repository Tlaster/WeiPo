using System.Collections.Immutable;

namespace WeiPoX.Core.DeclarativeUI.Widgets;

public abstract record Widget;

public abstract record MappingWidget : Widget;

public abstract record StateWidget : Widget;

internal interface IPanelWidget
{
    ImmutableList<Widget> Children { get; }
}

internal interface IContextProvider
{
    ImmutableDictionary<Type, object> Providers { get; }
}

internal interface ILazyWidget
{
    int Count { get; }
    Func<Widget>? GetBuilder(int index);
}
