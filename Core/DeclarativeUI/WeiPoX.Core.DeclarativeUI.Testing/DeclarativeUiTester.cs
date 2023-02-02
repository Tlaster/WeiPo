using System.Security.Principal;
using WeiPoX.Core.DeclarativeUI.Widgets;

namespace WeiPoX.Core.DeclarativeUI.Testing;

public class DeclarativeUiTester
{
    private readonly TestBuildOwner _buildOwner;
    private readonly TestWidgetBuilder _widgetBuilder;
    private readonly Widget _widget;
    private TestControl? _control;

    public DeclarativeUiTester(Widget widget)
    {
        _buildOwner = new TestBuildOwner();
        _widgetBuilder = new TestWidgetBuilder(_buildOwner);
        _widget = widget;
    }

    public void Render()
    {
        _control = _widgetBuilder.BuildIfNeeded(_widget, _widget, _control);
        _buildOwner.CleanUp();
    }
    
    public T GetWidget<T>() where T: Widget
    {
        return (T) _widget;
    }

    public static DeclarativeUiTester Create<T>() where T: Widget, new()
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
            return (T) panelWidget.Children[index];
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