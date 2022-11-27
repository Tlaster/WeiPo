using System.Collections.Immutable;

namespace WeiPoX.Core.DeclarativeUI.Widget.Layout;

public record Row(ImmutableList<WidgetObject> Children) : Panel(Children);