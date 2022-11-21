using System.Collections.Immutable;
using System.Windows.Input;
using WeiPoX.Core.DeclarativeUI.Layout;

namespace WeiPoX.Core.DeclarativeUI;

public record Button(ICommand OnClick, ImmutableList<WidgetObject> Children) : Panel(Children);