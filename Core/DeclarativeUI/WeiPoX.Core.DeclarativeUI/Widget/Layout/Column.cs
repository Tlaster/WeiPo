using System.Collections.Immutable;

namespace WeiPoX.Core.DeclarativeUI.Widget.Layout;

public record Column(ImmutableList<WidgetObject> Children) : Panel(Children);