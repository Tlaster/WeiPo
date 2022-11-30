using WeiPoX.Core.DeclarativeUI.Widgets;

namespace WeiPoX.Core.DeclarativeUI;

public static class Hooks
{
    // TODO: clean up unused hooks after rendered
    // TODO: subscribe to list changes and render the UI accordingly
    private static readonly List<object> _hooks = new();
    private static int _hookId;

    internal static Widget RenderWithHooks(Func<Widget> render)
    {
        _hookId = 0;
        return render();
    }

    public static State<T> UseState<T>(T initialState) where T : notnull
    {
        if (_hooks.Count <= _hookId)
        {
            _hooks.Add(initialState);
        }

        var index = _hookId;
        var setState = new Action<T>(value => { _hooks[index] = value; });
        return new State<T>((T)_hooks[_hookId++], setState);
    }


    public static State<T> UseState<T>(Func<T> initialState) where T : notnull
    {
        if (_hooks.Count <= _hookId)
        {
            _hooks.Add(initialState.Invoke());
        }

        var index = _hookId;
        var setState = new Action<T>(value => { _hooks[index] = value; });
        return new State<T>((T)_hooks[_hookId++], setState);
    }

    public static void UseEffect(Action effect, params object[] dependencies)
    {
        var hasNoDependencies = dependencies.Length == 0;
        var deps = _hooks.Count <= _hookId ? null : (object[])_hooks[_hookId];
        var hasChanged = deps == null || !deps.SequenceEqual(dependencies);
        if (hasNoDependencies || hasChanged)
        {
            effect();
            if (_hooks.Count <= _hookId)
            {
                _hooks.Add(dependencies);
            }
            else
            {
                _hooks[_hookId] = dependencies;
            }
        }

        _hookId++;
    }
}