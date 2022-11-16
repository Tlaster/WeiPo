using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace WeiPoX.DeclarativeUI.WinUI3.Renderer;

internal abstract class RendererObject<TWidget, TControl> : IRenderer
    where TWidget : WidgetObject where TControl : class, new()
{
    public void Update(FrameworkElement control, WidgetObject widget)
    {
        Update(control as TControl ?? throw new InvalidOperationException(), (TWidget)widget);
    }

    public void AddChild(FrameworkElement control, FrameworkElement childControl)
    {
        AddChild(control as TControl ?? throw new InvalidOperationException(), childControl);
    }

    public void RemoveChild(FrameworkElement control, FrameworkElement childControl)
    {
        RemoveChild(control as TControl ?? throw new InvalidOperationException(), childControl);
    }

    public void ReplaceChild(FrameworkElement control, int index, FrameworkElement newChildControl)
    {
        ReplaceChild(control as TControl ?? throw new InvalidOperationException(), index, newChildControl);
    }

    FrameworkElement IRenderer.Create()
    {
        return Create() as Control ?? throw new InvalidOperationException();
    }

    protected internal virtual TControl Create()
    {
        return new TControl();
    }

    protected internal virtual void AddChild(TControl control, FrameworkElement childControl)
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

    protected internal virtual void RemoveChild(TControl control, FrameworkElement childControl)
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

    protected internal virtual void ReplaceChild(TControl control, int index, FrameworkElement newChildControl)
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

    protected internal abstract void Update(TControl control, TWidget widget);
}

internal interface IRenderer
{
    FrameworkElement Create();
    void Update(FrameworkElement control, WidgetObject widget);
    void AddChild(FrameworkElement control, FrameworkElement childControl);
    void RemoveChild(FrameworkElement control, FrameworkElement childControl);
    void ReplaceChild(FrameworkElement control, int index, FrameworkElement newChildControl);
}