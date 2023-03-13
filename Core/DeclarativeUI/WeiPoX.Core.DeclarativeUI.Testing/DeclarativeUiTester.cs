using System.Security.Principal;
using WeiPoX.Core.DeclarativeUI.Widgets;

namespace WeiPoX.Core.DeclarativeUI.Testing;

public class DeclarativeUiTester
{
    private readonly TestBuildOwner _buildOwner;
    private readonly TestWidgetBuilder _widgetBuilder;
    private readonly bool _enableAsyncBuilder;
    private readonly Widget _widget;
    private TestControl? _control;
    private bool _rendering;
    private bool _requireReRender;

    public DeclarativeUiTester(Widget widget, bool enableAsyncBuilder = false)
    {
        _buildOwner = new TestBuildOwner();
        _buildOwner.OnRequestBuild += BuildOwnerOnOnRequestBuild;
        _widgetBuilder = new TestWidgetBuilder(_buildOwner);
        _widget = widget;
        _enableAsyncBuilder = enableAsyncBuilder;
        if (_enableAsyncBuilder)
        {
            _ = RenderAsync();
        }
        else
        {
            Render();
        }
    }

    private void BuildOwnerOnOnRequestBuild()
    {
        if (_rendering)
        {
            _requireReRender = true;
        }
        else
        {
            if (_enableAsyncBuilder)
            {
                _ = RenderAsync();
            }
            else
            {
                Render();
            }
        }
    }

    public void Render()
    {
        _rendering = true;
        _control = _widgetBuilder.BuildIfNeeded(_widget, _widget, _control);
        _buildOwner.CleanUp();
        _rendering = false;
        if (!_requireReRender)
        {
            return;
        }
        _requireReRender = false;
        Render();
    }

    public async Task RenderAsync()
    {
        _rendering = true;
        _control = await _widgetBuilder.BuildIfNeededAsync(_widget, _widget, _control);
        _buildOwner.CleanUp();
        _rendering = false;
        if (!_requireReRender)
        {
            return;
        }
        _requireReRender = false;
        _ = RenderAsync();
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