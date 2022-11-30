using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using WeiPoX.Core.DeclarativeUI.Widgets;
using WeiPoX.Core.DeclarativeUI.WinUI3.Internal;

namespace WeiPoX.Core.DeclarativeUI.WinUI3;

public class DeclarativeView : UserControl
{
    private readonly WidgetRenderer _renderer = new();
    private Widget? _widget;

    protected void Render(Widget widget)
    {
        Content = _renderer.RenderIfNeeded(_widget, widget, Content != null ? Content : null);
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