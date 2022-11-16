using System.Collections.Immutable;

namespace WeiPoX.DeclarativeUI.Layout;

public record Column(ImmutableList<WidgetObject> Children) : Panel(Children);