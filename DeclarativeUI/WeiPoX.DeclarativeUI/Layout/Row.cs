using System.Collections.Immutable;

namespace WeiPoX.DeclarativeUI.Layout;

public record Row(ImmutableList<WidgetObject> Children) : Panel(Children);