using System.Collections.Immutable;
using WeiPoX.Core.DeclarativeUI.Widget.Layout;

namespace WeiPoX.Core.DeclarativeUI.Widget;

public static class FuncUi
{
    public static Text Text(string text)
    {
        return new Text(text);
    }

    public static Button Button(Action onClick, params WidgetObject[] children)
    {
        return new Button(onClick, children.ToImmutableList());
    }

    public static Column Column(params WidgetObject[] children)
    {
        return new Column(children.ToImmutableList());
    }

    public static Row Row(params WidgetObject[] children)
    {
        return new Row(children.ToImmutableList());
    }

    public static Box Box(params WidgetObject[] children)
    {
        return new Box(children.ToImmutableList());
    }

    public static Padding Padding(Thickness thickness, WidgetObject child)
    {
        return new Padding(thickness, child);
    }
}