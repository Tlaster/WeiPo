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

public class RepeaterDeclarativeView : UserControl
{
    private IBuildOwner? _previousBuildOwner;
    private Func<Widget?>? _previousWidgetBuilder;
    private DeclarativeCore<UIElement>? _core;

    public static DataTemplate GenerateDataTemplate()
    {
        // generate DataTemplate from xaml string
        var xaml = $@"
<DataTemplate 
    xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
    xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
    xmlns:local=""using:{typeof(RepeaterDeclarativeView).Namespace}"">
    <local:{nameof(RepeaterDeclarativeView)} {nameof(RepeaterDeclarativeView.RepeaterItem)}=""{{Binding}}"" />
</DataTemplate>".Trim();
        var dataTemplate = (DataTemplate)XamlReader.Load(xaml);
        return dataTemplate;
    }

    public static readonly DependencyProperty RepeaterItemProperty = DependencyProperty.Register(
        nameof(RepeaterItem), typeof(RepeaterItem), typeof(RepeaterDeclarativeView), new PropertyMetadata(
            default(RepeaterItem), (o, args) =>
            {
                if (o is RepeaterDeclarativeView declarativeView && args.NewValue is RepeaterItem item)
                {
                    declarativeView.Update(args.OldValue as RepeaterItem, item);
                }
            }));

    public RepeaterItem RepeaterItem
    {
        get => (RepeaterItem)GetValue(RepeaterItemProperty);
        set => SetValue(RepeaterItemProperty, value);
    }

    private void Update(RepeaterItem? oldValue, RepeaterItem newValue)
    {
        if (_previousBuildOwner != newValue.BuildOwner)
        {
            _core = new DeclarativeCore<UIElement>(new WidgetBuilder(), UpdateChild, newValue.BuildOwner);
        }
        _previousBuildOwner = newValue.BuildOwner;
        if (_previousWidgetBuilder != newValue.WidgetBuilder && _core != null)
        {
            _core.Widget = newValue.WidgetBuilder();
        }

        _previousWidgetBuilder = newValue.WidgetBuilder;
    }
    
    internal void UpdateChild(UIElement control)
    {
        if (!Equals(Content, control))
        {
            Content = control;
        }
    }
}