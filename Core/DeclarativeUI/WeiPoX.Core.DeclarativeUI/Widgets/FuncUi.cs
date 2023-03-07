using System.Collections.Immutable;
using WeiPoX.Core.DeclarativeUI.Widgets.Layout;

namespace WeiPoX.Core.DeclarativeUI.Widgets;

public static class FuncUi
{
    public static Text Text(string text)
    {
        return new Text(text);
    }

    public static Button Button(string text, Action onClick)
    {
        return new Button(text, onClick);
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

    public static ContextProvider ContextProvider(IDictionary<Type, object> providers, Widget child)
    {
        return new ContextProvider(providers.ToImmutableDictionary(), child);
    }

    public static ImmutableDictionary<Type, object> Providers(params (Type, object)[] providers)
    {
        return providers.ToImmutableDictionary(it => it.Item1, it => it.Item2);
    }
    
    public static ImmutableList<T> ImmutableListOf<T>(params T[] items)
    {
        return items.ToImmutableList();
    }

    public static Input Input(InputState text, Action<InputState> onTextChanged)
    {
        return new Input(text, onTextChanged);
    }
}