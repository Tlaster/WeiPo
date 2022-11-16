using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using WeiPoX.DeclarativeUI.WinUI3.Internal;

namespace WeiPoX.DeclarativeUI.WinUI3;

public class DeclarativeView : UserControl
{
    private readonly WidgetRenderer _renderer = new();
    private WidgetObject? _widget;

    protected void Render(WidgetObject widget)
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

    public DeclarativeView()
    {
        Loaded += OnLoaded;
        Unloaded += OnUnloaded;
    }

    private void OnUnloaded(object sender, RoutedEventArgs e)
    {
        _disposable?.Dispose();
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        _disposable = State.Subscribe(state => Render(Render(state)));
    }

    protected abstract WidgetObject Render(T state);
}