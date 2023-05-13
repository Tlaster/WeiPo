using System.Security.Principal;
using WeiPoX.Core.DeclarativeUI.Widgets;

namespace WeiPoX.Core.DeclarativeUI.Testing;

public class DeclarativeUiTester
{
    
    private readonly DeclarativeCore<TestControl> _core;
    private readonly Widget _widget;

    public DeclarativeUiTester(Widget widget)
    {
        var buildOwner = new BuildOwner(RequestRender);
        _core = new DeclarativeCore<TestControl>(new TestWidgetBuilder(buildOwner), UpdateChild, RunInUi, buildOwner);
        _widget = widget;
        _core.Widget = widget;
    }

    private void RequestRender()
    {
        _core.RequestRender();
    }

    private void RunInUi(Action obj)
    {
        obj.Invoke();
    }

    private void UpdateChild(TestControl obj)
    {
        
    }

    public T GetWidget<T>() where T : Widget
    {
        return (T)_widget;
    }

    public static DeclarativeUiTester Create<T>() where T : Widget, new()
    {
        return new DeclarativeUiTester(new T());
    }
}

public static class WidgetExtensions
{
    public static DeclarativeUiTester Test(this Widget widget)
    {
        return new DeclarativeUiTester(widget);
    }

    public static T? FindChildAtIndex<T>(this Widget widget, int index) where T : Widget
    {
        if (widget is IPanelWidget panelWidget)
        {
            return (T)panelWidget.Children[index];
        }
        else
        {
            return null;
        }
    }

    public static T? GetChild<T>(this StatefulWidget widget) where T : Widget
    {
        return widget.State.CachedBuild as T;
    }
}