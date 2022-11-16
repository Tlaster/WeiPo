using System.Collections.Immutable;
using System.Windows.Input;
using WeiPoX.DeclarativeUI.Layout;

namespace WeiPoX.DeclarativeUI;

public record Button(ICommand OnClick, ImmutableList<WidgetObject> Children) : Panel(Children);