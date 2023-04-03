using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;
using WeiPoX.Core.DeclarativeUI.Platform.Avalonia.Internal;
using WeiPoX.Core.DeclarativeUI.Widgets.Layout;

namespace WeiPoX.Core.DeclarativeUI.Platform.Avalonia.Renderer;

internal class BoxRenderer : RendererObject<Box, WeiPoXBox>
{
    protected override void Update(WeiPoXBox control, Box widget)
    {
        control.ContentGrid.HorizontalAlignment = widget.Horizontal.ToHorizontalAlignment();
        control.ContentGrid.VerticalAlignment = widget.Vertical.ToVerticalAlignment();
        control.Background = new SolidColorBrush(widget.BackgroundColor.ToColor());
        if (!double.IsNaN(widget.Height) && !double.IsInfinity(widget.Height))
        {
            control.Height = widget.Height;
        }
        if (!double.IsNaN(widget.Width) && !double.IsInfinity(widget.Width))
        {
            control.Width = widget.Width;
        }
    }

    protected override void AddChild(WeiPoXBox control, Control childControl)
    {
        control.ContentGrid.Children.Add(childControl);
    }
    
    protected override void RemoveChild(WeiPoXBox control, Control childControl)
    {
        control.ContentGrid.Children.Remove(childControl);
    }

    protected override void ReplaceChild(WeiPoXBox control, int index, Control newChildControl)
    {
        control.ContentGrid.Children[index] = newChildControl;
    }

    public override Control? GetChildAt(Control control, int index)
    {
        if (control is WeiPoXBox box)
        {
            return box.ContentGrid.Children.ElementAtOrDefault(index);
        }
        return base.GetChildAt(control, index);
    }

    public override bool IsPanel(Control value)
    {
        return true;
    }
}

internal class WeiPoXBox : UserControl
{
    private readonly Grid _rootGrid;
    private readonly Grid _contentGrid;
    public WeiPoXBox()
    {
        _contentGrid = new Grid();
        _rootGrid = new Grid
        {
            Children = { _contentGrid },
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Stretch,
        };
        Content = _rootGrid;
    }
    
    public Grid ContentGrid => _contentGrid;
}