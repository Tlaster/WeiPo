using System.Collections.Immutable;

namespace WeiPoX.Core.DeclarativeUI.Widgets.Layout;

public record Row(ImmutableList<Widget> Children) : Panel(Children);