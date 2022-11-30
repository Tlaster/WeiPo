using WeiPoX.Core.DeclarativeUI.Widgets.Layout;

namespace WeiPoX.Core.DeclarativeUI.Widgets;

public record Padding(Thickness Thickness, Widget Child) : SingleChildPanel(Child);