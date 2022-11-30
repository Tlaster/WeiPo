using System.Collections.Immutable;

namespace WeiPoX.Core.DeclarativeUI.Widgets;

public abstract record Widget;

public abstract record MappingWidget : Widget;

public abstract record StatelessWidget : Widget
{
    protected abstract Widget Build();
}


internal interface IPanelWidget
{
    ImmutableList<Widget> Children { get; }
}