using System.Diagnostics;
using WeiPoX.Core.DeclarativeUI.Internal;
using WeiPoX.Core.DeclarativeUI.Widgets;

namespace WeiPoX.Core.DeclarativeUI;

public class BuildOwner : IBuildOwner
{
    private readonly Action _requestRender;
    private readonly List<Widget> _rebuiltWidgets = new();

    public BuildOwner(Action requestRender)
    {
        _requestRender = requestRender;
    }

    public void MarkNeedsBuild(Widget widget)
    {
        _rebuiltWidgets.Add(widget);
        _requestRender.Invoke();
    }

    public bool IsBuildScheduled(Widget widget)
    {
        return _rebuiltWidgets.Contains(widget);
    }

    public void CleanUp()
    {
        _rebuiltWidgets.Clear();
    }
}

public class DeclarativeCore<T>
{
    private readonly WidgetBuilder<T> _renderer;
    private readonly Action<T> _updateChild;
    private Widget? _previousWidget;
    private T? _renderedControl;
    private bool _rendering;
    private int _requestBuildCount = 1;
    private readonly IBuildOwner _buildOwner;

    public DeclarativeCore(WidgetBuilder<T> renderer, Action<T> updateChild, IBuildOwner? buildOwner = null)
    {
        _renderer = renderer;
        _updateChild = updateChild;
        _buildOwner = buildOwner ?? new BuildOwner(RequestRender);
    }

    private void RequestRender()
    {
        _requestBuildCount++;
        if (!_rendering && _previousWidget != null)
        {
            _ = Render(_previousWidget);
        }
    }
    
    public Widget? Widget
    {
        get => _previousWidget;
        set
        {
            if (value != null)
            {
                _requestBuildCount++;
                if (!_rendering)
                {
                    _ = Render(value);
                }
            }
            _previousWidget = value;
        }
    }

    private async Task Render(Widget widget)
    {
        try
        {
            while (_requestBuildCount > 0)
            {
                _rendering = true;
                _renderedControl = await _renderer.BuildIfNeededAsync(_previousWidget, widget, _renderedControl, _buildOwner);
                _previousWidget = widget;
                _rendering = false;
                _requestBuildCount--;
                if (_requestBuildCount == 0)
                {
                    _buildOwner.CleanUp();
                    _updateChild(_renderedControl);
                }
                else
                {
                    _requestBuildCount = 1;
                }
            }
        }
        catch (Exception e)
        {
            var text = new Text(e.ToString());
            _renderedControl = await _renderer.BuildIfNeededAsync(text, text, _renderedControl, _buildOwner);
            _buildOwner.CleanUp();
            _updateChild(_renderedControl);
            throw;
        }
    }
}
