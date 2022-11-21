using WeiPoX.Core.DeclarativeUI.Layout;

namespace WeiPoX.Core.DeclarativeUI;

public record Padding(Thickness Thickness, WidgetObject Child) : SingleChildPanel(Child);