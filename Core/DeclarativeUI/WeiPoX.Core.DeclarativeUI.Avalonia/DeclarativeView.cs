using Avalonia;
using Avalonia.Controls;
using Avalonia.LogicalTree;
using WeiPoX.Core.DeclarativeUI.Avalonia.Internal;
using WeiPoX.Core.DeclarativeUI.Widgets;

namespace WeiPoX.Core.DeclarativeUI.Avalonia;

public class DeclarativeView : UserControl
{
    private readonly WidgetRenderer _renderer = new();
    private Widget? _widget;

    protected void Render(Widget widget)
    {
        if (Content is Control control)
        {
            Content = _renderer.RenderIfNeeded(_widget, widget, control);
        }
        else
        {
            Content = _renderer.RenderIfNeeded(_widget, widget, null);
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