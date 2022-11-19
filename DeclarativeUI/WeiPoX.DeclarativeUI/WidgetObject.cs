using System.Collections.Immutable;

namespace WeiPoX.DeclarativeUI;

public record WidgetObject;

internal interface IPanelWidget
{
    ImmutableList<WidgetObject> Children { get; }
}