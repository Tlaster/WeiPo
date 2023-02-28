using WeiPoX.Core.DeclarativeUI.Internal;
using WeiPoX.Core.DeclarativeUI.Platform.Mac.Internal;
using WeiPoX.Core.DeclarativeUI.Widgets;

namespace WeiPoX.Core.DeclarativeUI.Platform.Mac;

public abstract class DeclarativeView : NSControl, IBuildOwner
{
    public List<Widget> RebuiltWidgets { get; } = new();
    private readonly WidgetBuilder _renderer;
    private NSView? _content;
    private bool _rendering;
    private bool _requireReRender;

    protected DeclarativeView()
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
        _content = _renderer.BuildIfNeeded(Content, Content, _content);
        if (!Subviews.Any())
        {
            AddSubview(_content);
        }
        else
        {
            ReplaceSubviewWith(Subviews[0], _content);
        }
        _rendering = false;
        if (!_requireReRender)
        {
            return;
        }

        _requireReRender = false;
        Render();
    }
    
    protected abstract Widget Content { get; }
}

public class Declarative : DeclarativeView
{
    public Declarative(Widget content)
    {
        Content = content;
        Render();
    }

    protected override Widget Content { get; }
}