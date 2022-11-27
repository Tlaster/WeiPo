using System.Collections.Immutable;

namespace WeiPoX.Core.DeclarativeUI.Widget;

public record WidgetObject;

internal interface IPanelWidget
{
    ImmutableList<WidgetObject> Children { get; }
}