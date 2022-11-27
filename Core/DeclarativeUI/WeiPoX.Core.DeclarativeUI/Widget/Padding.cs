using WeiPoX.Core.DeclarativeUI.Widget.Layout;

namespace WeiPoX.Core.DeclarativeUI.Widget;

public record Padding(Thickness Thickness, WidgetObject Child) : SingleChildPanel(Child);