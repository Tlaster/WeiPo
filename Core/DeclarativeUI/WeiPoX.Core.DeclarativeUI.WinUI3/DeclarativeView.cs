using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using WeiPoX.Core.DeclarativeUI.Internal;
using WeiPoX.Core.DeclarativeUI.Widgets;
using WeiPoX.Core.DeclarativeUI.WinUI3.Internal;

namespace WeiPoX.Core.DeclarativeUI.WinUI3;

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
        Content = _renderer.BuildIfNeeded(_widget, widget, Content != null ? Content : null);
        _widget = widget;
    }
}

public abstract class DeclarativeView<T> : DeclarativeView
{
    private IDisposable? _disposable;

    protected DeclarativeView()
    {
        Loaded += OnLoaded;
        Unloaded += OnUnloaded;
    }

    protected abstract IObservable<T> State { get; }

    private void OnUnloaded(object sender, RoutedEventArgs e)
    {
        _disposable?.Dispose();
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        _disposable = State.Subscribe(state => Render(Render(state)));
    }

    protected abstract Widget Render(T state);
}