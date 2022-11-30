using System.Collections.Immutable;

namespace WeiPoX.Core.DeclarativeUI.Widgets.Layout;

public record Column(ImmutableList<Widget> Children) : Panel(Children);