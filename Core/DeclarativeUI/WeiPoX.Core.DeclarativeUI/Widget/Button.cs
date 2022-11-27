using System.Collections.Immutable;
using WeiPoX.Core.DeclarativeUI.Widget.Layout;

namespace WeiPoX.Core.DeclarativeUI.Widget;

public record Button(Action OnClick, ImmutableList<WidgetObject> Children) : Panel(Children);