using System.Collections.Immutable;

namespace WeiPoX.Core.DeclarativeUI.Widgets.Layout;

public record Box(ImmutableList<Widget> Children) : Panel(Children);