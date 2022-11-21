using System.Collections.Immutable;

namespace WeiPoX.Core.DeclarativeUI;

public record WidgetObject;

internal interface IPanelWidget
{
    ImmutableList<WidgetObject> Children { get; }
}