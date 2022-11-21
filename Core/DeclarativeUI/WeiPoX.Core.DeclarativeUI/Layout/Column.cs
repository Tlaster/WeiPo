using System.Collections.Immutable;

namespace WeiPoX.Core.DeclarativeUI.Layout;

public record Column(ImmutableList<WidgetObject> Children) : Panel(Children);