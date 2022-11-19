using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using WeiPoX.DeclarativeUI.Internal;

namespace WeiPoX.DeclarativeUI.WinUI3.Renderer;

internal abstract class RendererObject<TWidget, TControl> : IRenderer<UIElement>
    where TWidget : WidgetObject where TControl : class, new()
{
    public void Update(UIElement control, WidgetObject widget)
    {
        Update(control as TControl ?? throw new InvalidOperationException(), (TWidget)widget);
    }

    public void AddChild(UIElement control, UIElement childControl)
    {
        AddChild(control as TControl ?? throw new InvalidOperationException(), childControl);
    }

    public void RemoveChild(UIElement control, UIElement childControl)
    {
        RemoveChild(control as TControl ?? throw new InvalidOperationException(), childControl);
    }

    public void ReplaceChild(UIElement control, int index, UIElement newChildControl)
    {
        ReplaceChild(control as TControl ?? throw new InvalidOperationException(), index, newChildControl);
    }

    UIElement IRenderer<UIElement>.Create()
    {
        return Create() as Control ?? throw new InvalidOperationException();
    }

    protected virtual TControl Create()
    {
        return new TControl();
    }

    protected virtual void AddChild(TControl control, UIElement childControl)
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

    protected virtual void RemoveChild(TControl control, UIElement childControl)
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

    protected virtual void ReplaceChild(TControl control, int index, UIElement newChildControl)
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