using Avalonia;
using Avalonia.Controls;
using Avalonia.LogicalTree;
using WeiPoX.Core.DeclarativeUI.Internal;
using WeiPoX.Core.DeclarativeUI.Platform.Avalonia.Internal;
using WeiPoX.Core.DeclarativeUI.Widgets;

namespace WeiPoX.Core.DeclarativeUI.Platform.Avalonia;

public class DeclarativeView : UserControl, IBuildOwner
{
    private readonly WidgetBuilder _renderer;
    private Widget? _widget;

    public DeclarativeView()
    {
        _renderer = new WidgetBuilder(this);
    }


    public void MarkNeedsBuild(Widget widget)
    {
    }

    public bool IsBuildScheduled(Widget widget)
    {
        return false;
    }

    public void CleanUp()
    {
    }

    protected void Render(Widget widget)
    {
        if (Content is Control control)
        {
            Content = _renderer.BuildIfNeeded(_widget, widget, control);
        }
        else
        {
            Content = _renderer.BuildIfNeeded(_widget, widget, null);
        }

        _widget = widget;
    }
}

public abstract class DeclarativeView<T> : DeclarativeView
{
    private IDisposable? _disposable;
    protected abstract IObservable<T> State { get; }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        _disposable = State.Subscribe(state => Render(Render(state)));
    }

    protected override void OnDetachedFromLogicalTree(LogicalTreeAttachmentEventArgs e)
    {
        base.OnDetachedFromLogicalTree(e);
        _disposable?.Dispose();
    }

    protected abstract Widget Render(T state);
}