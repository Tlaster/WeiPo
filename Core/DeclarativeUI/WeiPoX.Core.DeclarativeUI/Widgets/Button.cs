using System.Collections.Immutable;
using WeiPoX.Core.DeclarativeUI.Widgets.Layout;

namespace WeiPoX.Core.DeclarativeUI.Widgets;

public record Button(string Text, Action OnClick) : MappingWidget;