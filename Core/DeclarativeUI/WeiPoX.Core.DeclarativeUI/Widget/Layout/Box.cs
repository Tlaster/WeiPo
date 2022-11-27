using System.Collections.Immutable;

namespace WeiPoX.Core.DeclarativeUI.Widget.Layout;

public record Box(ImmutableList<WidgetObject> Children) : Panel(Children);