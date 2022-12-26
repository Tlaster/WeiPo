using System.Collections.Immutable;
using WeiPoX.Core.DeclarativeUI.Widgets.Layout;

namespace WeiPoX.Core.DeclarativeUI.Widgets;

public static class FuncUi
{
    public static Text Text(string text)
    {
        return new Text(text);
    }

    public static Button Button(Action onClick, params Widget[] children)
    {
        return new Button(onClick, children.ToImmutableList());
    }

    public static Column Column(params Widget[] children)
    {
        return new Column(children.ToImmutableList());
    }

    public static Row Row(params Widget[] children)
    {
        return new Row(children.ToImmutableList());
    }

    public static Box Box(params Widget[] children)
    {
        return new Box(children.ToImmutableList());
    }

    public static Padding Padding(Thickness thickness, Widget child)
    {
        return new Padding(thickness, child);
    }

    public static ContextProvider ContextProvider(IEnumerable<(Type, object)> providers, Widget child)
    {
        return new ContextProvider(
            providers.Select(it => KeyValuePair.Create(it.Item1, it.Item2)).ToImmutableDictionary(), child);
    }
}