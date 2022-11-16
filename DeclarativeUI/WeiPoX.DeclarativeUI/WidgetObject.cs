using System.Collections.Immutable;
using System.Runtime.CompilerServices;

[assembly:InternalsVisibleTo("WeiPoX.DeclarativeUI.WinUI3")]

namespace WeiPoX.DeclarativeUI;

public record WidgetObject;

internal interface IPanelWidget
{
    ImmutableList<WidgetObject> Children { get; }
}