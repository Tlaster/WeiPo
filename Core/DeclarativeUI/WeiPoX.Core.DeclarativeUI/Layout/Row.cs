using System.Collections.Immutable;

namespace WeiPoX.Core.DeclarativeUI.Layout;

public record Row(ImmutableList<WidgetObject> Children) : Panel(Children);