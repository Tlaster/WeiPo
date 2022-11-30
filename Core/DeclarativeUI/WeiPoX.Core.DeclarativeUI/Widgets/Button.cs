using System.Collections.Immutable;
using WeiPoX.Core.DeclarativeUI.Widgets.Layout;

namespace WeiPoX.Core.DeclarativeUI.Widgets;

public record Button(Action OnClick, ImmutableList<Widget> Children) : Panel(Children);