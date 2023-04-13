using System;
using System.Windows;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Markup;
using WeiPoX.Core.DeclarativeUI.Internal;
using WeiPoX.Core.DeclarativeUI.Platform.WinUI3.Internal;
using WeiPoX.Core.DeclarativeUI.Platform.WinUI3.Renderer;
using WeiPoX.Core.DeclarativeUI.Widgets;
using DataTemplate = Microsoft.UI.Xaml.DataTemplate;
using DependencyObject = Microsoft.UI.Xaml.DependencyObject;
using DependencyProperty = Microsoft.UI.Xaml.DependencyProperty;
using DependencyPropertyChangedEventArgs = Microsoft.UI.Xaml.DependencyPropertyChangedEventArgs;
using PropertyMetadata = Microsoft.UI.Xaml.PropertyMetadata;
using RoutedEventArgs = Microsoft.UI.Xaml.RoutedEventArgs;
using UIElement = Microsoft.UI.Xaml.UIElement;

namespace WeiPoX.Core.DeclarativeUI.Platform.WinUI3;

public class DeclarativeView : UserControl
{
    private readonly DeclarativeCore<UIElement> _core;

    public DeclarativeView(IBuildOwner? buildOwner = null)
    {
        _core = new DeclarativeCore<UIElement>(new WidgetBuilder(), UpdateChild, buildOwner);
    }

    public Widget? Widget
    {
        get => _core.Widget;
        set => _core.Widget = value;
    }

    internal void UpdateChild(UIElement control)
    {
        if (!Equals(Content, control))
        {
            Content = control;
        }
    }
}
