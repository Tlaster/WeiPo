using System.Collections.Immutable;

namespace WeiPoX.DeclarativeUI.Layout;

public record Box(ImmutableList<WidgetObject> Children) : Panel(Children);