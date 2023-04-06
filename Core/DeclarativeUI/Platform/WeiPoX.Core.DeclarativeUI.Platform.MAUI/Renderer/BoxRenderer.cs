using WeiPoX.Core.DeclarativeUI.Platform.MAUI.Internal;
using WeiPoX.Core.DeclarativeUI.Widgets.Layout;
using MAUIThickness = Microsoft.Maui.Thickness;

namespace WeiPoX.Core.DeclarativeUI.Platform.MAUI.Renderer;

internal class BoxRenderer : RendererObject<Box, WeiPoXBox>
{
    protected override void Update(WeiPoXBox control, Box widget)
    {
        control.ContentGrid.HorizontalOptions = widget.Horizontal.ToLayoutOptions();
        control.ContentGrid.VerticalOptions = widget.Vertical.ToLayoutOptions();
        control.Background = new SolidColorBrush(widget.BackgroundColor.ToColor());
        if (!double.IsNaN(widget.Height) && !double.IsInfinity(widget.Height))
        {
            control.HeightRequest = widget.Height;
        }
        if (!double.IsNaN(widget.Width) && !double.IsInfinity(widget.Width))
        {
            control.WidthRequest = widget.Width;
        }
        control.Padding = new MAUIThickness(widget.Padding.Start, widget.Padding.Top, widget.Padding.End,
            widget.Padding.Bottom);
    }

    protected override void AddChild(WeiPoXBox control, View childControl)
    {
        control.ContentGrid.Children.Add(childControl);
    }
    
    protected override void RemoveChild(WeiPoXBox control, View childControl)
    {
        control.ContentGrid.Children.Remove(childControl);
    }

    protected override void ReplaceChild(WeiPoXBox control, int index, View newChildControl)
    {
        control.ContentGrid.Children[index] = newChildControl;
    }

    public override View? GetChildAt(View control, int index)
    {
        if (control is WeiPoXBox box && box.ContentGrid.Children.ElementAtOrDefault(index) is View child)
        {
            return child;
        }
        return base.GetChildAt(control, index);
    }

    public override bool IsPanel(View value)
    {
        return true;
    }
}

internal class WeiPoXBox : ContentView
{
    private readonly Grid _rootGrid;
    private readonly Grid _contentGrid;
    public WeiPoXBox()
    {
        _contentGrid = new Grid();
        _rootGrid = new Grid
        {
            Children = { _contentGrid },
            HorizontalOptions = LayoutOptions.Fill,
            VerticalOptions = LayoutOptions.Fill,
        };
        Content = _rootGrid;
    }
    
    public Grid ContentGrid => _contentGrid;
}