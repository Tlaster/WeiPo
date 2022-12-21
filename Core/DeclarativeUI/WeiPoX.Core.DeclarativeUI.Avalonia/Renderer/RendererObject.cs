using Avalonia.Controls;
using WeiPoX.Core.DeclarativeUI.Internal;
using WeiPoX.Core.DeclarativeUI.Widgets;

namespace WeiPoX.Core.DeclarativeUI.Avalonia.Renderer;

internal abstract class RendererObject<TWidget, TControl> : IRenderer<IControl>
    where TWidget : MappingWidget where TControl : class, new()
{
    public void Update(IControl control, MappingWidget widget)
    {
        Update(control as TControl ?? throw new InvalidOperationException(), (TWidget)widget);
    }

    public void AddChild(IControl control, IControl childControl)
    {
        AddChild(control as TControl ?? throw new InvalidOperationException(), childControl);
    }

    public void RemoveChild(IControl control, IControl childControl)
    {
        RemoveChild(control as TControl ?? throw new InvalidOperationException(), childControl);
    }

    public void ReplaceChild(IControl control, int index, IControl newChildControl)
    {
        ReplaceChild(control as TControl ?? throw new InvalidOperationException(), index, newChildControl);
    }

    IControl IRenderer<IControl>.Create()
    {
        return Create() as Control ?? throw new InvalidOperationException();
    }

    protected virtual TControl Create()
    {
        return new TControl();
    }

    protected virtual void AddChild(TControl control, IControl childControl)
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

    protected virtual void RemoveChild(TControl control, IControl childControl)
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

    protected virtual void ReplaceChild(TControl control, int index, IControl newChildControl)
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