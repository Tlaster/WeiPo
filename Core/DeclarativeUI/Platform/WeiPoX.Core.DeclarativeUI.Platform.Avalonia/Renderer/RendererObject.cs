using Avalonia.Controls;
using WeiPoX.Core.DeclarativeUI.Internal;
using WeiPoX.Core.DeclarativeUI.Widgets;

namespace WeiPoX.Core.DeclarativeUI.Platform.Avalonia.Renderer;

internal abstract class RendererObject<TWidget, TControl> : IRenderer<Control>
    where TWidget : MappingWidget where TControl : class, new()
{
    public void Update(Control control, MappingWidget widget)
    {
        Update(control as TControl ?? throw new InvalidOperationException(), (TWidget)widget);
    }

    public void AddChild(Control control, Control childControl)
    {
        AddChild(control as TControl ?? throw new InvalidOperationException(), childControl);
    }

    public void RemoveChild(Control control, Control childControl)
    {
        RemoveChild(control as TControl ?? throw new InvalidOperationException(), childControl);
    }

    public void ReplaceChild(Control control, int index, Control newChildControl)
    {
        ReplaceChild(control as TControl ?? throw new InvalidOperationException(), index, newChildControl);
    }

    public bool IsPanel(Control value)
    {
        return value is Panel;
    }

    public virtual Control? GetChildAt(Control control, int index)
    {
        if (control is Panel panel)
        {
            return panel.Children.ElementAtOrDefault(index);
        }

        return null;
    }

    Control IRenderer<Control>.Create()
    {
        return Create() as Control ?? throw new InvalidOperationException();
    }

    protected virtual TControl Create()
    {
        return new TControl();
    }

    protected virtual void AddChild(TControl control, Control childControl)
    {
        switch (control)
        {
            case Panel panel:
                panel.Children.Add(childControl);
                break;
            case ContentControl contentControl:
            {
                contentControl.Content ??= new Grid();
                var panel = (Panel)contentControl.Content;
                panel.Children.Add(childControl);
                break;
            }
        }
    }

    protected virtual void RemoveChild(TControl control, Control childControl)
    {
        switch (control)
        {
            case Panel panel:
                panel.Children.Remove(childControl);
                break;
            case ContentControl contentControl:
            {
                contentControl.Content ??= new Grid();
                var panel = (Panel)contentControl.Content;
                panel.Children.Remove(childControl);
                break;
            }
        }
    }

    protected virtual void ReplaceChild(TControl control, int index, Control newChildControl)
    {
        switch (control)
        {
            case Panel panel:
                panel.Children[index] = newChildControl;
                break;
            case ContentControl contentControl:
            {
                contentControl.Content ??= new Grid();
                var panel = (Panel)contentControl.Content;
                panel.Children[index] = newChildControl;
                break;
            }
        }
    }

    protected abstract void Update(TControl control, TWidget widget);
}