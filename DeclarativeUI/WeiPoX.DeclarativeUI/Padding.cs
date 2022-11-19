using WeiPoX.DeclarativeUI.Layout;

namespace WeiPoX.DeclarativeUI;

public record Padding(Thickness Thickness, WidgetObject Child) : SingleChildPanel(Child);