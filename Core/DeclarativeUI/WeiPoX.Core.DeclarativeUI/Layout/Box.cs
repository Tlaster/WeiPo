using System.Collections.Immutable;

namespace WeiPoX.Core.DeclarativeUI.Layout;

public record Box(ImmutableList<WidgetObject> Children) : Panel(Children);