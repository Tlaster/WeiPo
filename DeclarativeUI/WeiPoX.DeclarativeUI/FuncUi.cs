using System.Collections.Immutable;
using System.Windows.Input;
using WeiPoX.DeclarativeUI.Layout;

namespace WeiPoX.DeclarativeUI;

public static class FuncUi
{
    public static Text Text(string text)
    {
        return new Text(text);
    }

    public static Button Button(ICommand onClick, params WidgetObject[] children)
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
    
    public static Padding Padding(WidgetObject child, double start = 0.0, double top = 0.0, double end = 0.0, double bottom = 0.0)
    {
        return new Padding(new Thickness(start, top, end, bottom), child);
    }
    
    public static Padding Padding(WidgetObject child, double all = 0.0)
    {
        return new Padding(new Thickness(all), child);
    }
    
    public static Padding Padding(WidgetObject child, double horizontal = 0.0, double vertical = 0.0)
    {
        return new Padding(new Thickness(horizontal, vertical), child);
    }
    
    public static Padding Padding(WidgetObject child, Thickness thickness)
    {
        return new Padding(thickness, child);
    }
}