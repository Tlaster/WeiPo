using Avalonia;
using Avalonia.Controls;
using Avalonia.LogicalTree;
using WeiPoX.Core.DeclarativeUI.Internal;
using WeiPoX.Core.DeclarativeUI.Platform.Avalonia.Internal;
using WeiPoX.Core.DeclarativeUI.Widgets;

namespace WeiPoX.Core.DeclarativeUI.Platform.Avalonia;

public abstract class DeclarativeView : UserControl, IBuildOwner
{
    public List<Widget> RebuiltWidgets { get; } = new();
    private readonly WidgetBuilder _renderer;
    private Control? _renderedControl;
    private bool _rendering;
    private bool _requireReRender;

    public DeclarativeView()
    {
        _renderer = new WidgetBuilder(this);
    }

    public void MarkNeedsBuild(Widget widget)
    {
        RebuiltWidgets.Add(widget);
        if (_rendering)
        {
            _requireReRender = true;
        }
        else
        {
            Render();
        }
    }

    public bool IsBuildScheduled(Widget widget)
    {
        return RebuiltWidgets.Contains(widget);
    }

    public void CleanUp()
    {
        RebuiltWidgets.Clear();
    }

    protected void Render()
    {
        _rendering = true;
        _renderedControl = _renderer.BuildIfNeeded(Widget, Widget, _renderedControl);
        _rendering = false;
        if (!_requireReRender)
        {
            if (!Equals(Content, _renderedControl))
            {
                Content = _renderedControl;
            }
            return;
        }

        _requireReRender = false;
        Render();
    }

    protected abstract Widget Widget { get; }
}
public class Declarative : DeclarativeView
{
    public Declarative(Widget widget)
    {
        Widget = widget;
        Render();
    }

    protected override Widget Widget { get; }
}