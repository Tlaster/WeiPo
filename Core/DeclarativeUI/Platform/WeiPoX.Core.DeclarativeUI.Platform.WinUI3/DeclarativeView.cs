using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using WeiPoX.Core.DeclarativeUI.Internal;
using WeiPoX.Core.DeclarativeUI.Platform.WinUI3.Internal;
using WeiPoX.Core.DeclarativeUI.Widgets;

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