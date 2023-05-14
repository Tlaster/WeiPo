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
    private readonly Action<Action> _runInUi;
    private readonly Action<T> _updateChild;
    private Widget? _previousWidget;
    private T? _renderedControl;
    private bool _rendering;
    private int _requestBuildCount = 1;
    private readonly IBuildOwner _buildOwner;
    private WidgetBuilder<T>.Element? _element;

    public DeclarativeCore(WidgetBuilder<T> renderer, Action<T> updateChild, Action<Action> runInUi, IBuildOwner? buildOwner = null)
    {
        _renderer = renderer;
        _updateChild = updateChild;
        _runInUi = runInUi;
        _buildOwner = buildOwner ?? new BuildOwner(RequestRender);
    }

    internal void RequestRender()
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
        _rendering = true;
        try
        {
            while (_requestBuildCount > 0)
            {
                _element = await _renderer.BuildElement(_previousWidget, widget, _buildOwner);
                _previousWidget = widget;
                _requestBuildCount--;
                if (_requestBuildCount == 0)
                {
                    _buildOwner.CleanUp();
                    _runInUi.Invoke(() =>
                    {
                        _renderedControl = _renderer.ApplyElement(_element, _renderedControl, _buildOwner);
                        _updateChild(_renderedControl);
                    });
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
            _renderedControl = await _renderer.Build(text, text, _renderedControl, _buildOwner);
            throw;
        }
        finally
        {
            _rendering = false;
        }
    }
}
