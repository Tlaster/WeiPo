using System;
using System.Linq;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using WeiPoX.Core.DeclarativeUI.Internal;
using WeiPoX.Core.DeclarativeUI.Platform.WinUI3.Internal;
using WeiPoX.Core.DeclarativeUI.Widgets;

namespace WeiPoX.Core.DeclarativeUI.Platform.WinUI3.Renderer;

public abstract class RendererObject<TWidget, TControl> : IRenderer<UIElement>
    where TWidget : MappingWidget where TControl : class, new()
{
    public void Update(UIElement control, MappingWidget widget)
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

    public virtual UIElement? GetChildAt(UIElement control, int index)
    {
        if (control is Panel panel)
        {
            return panel.Children.ElementAtOrDefault(index);
        }

        return null;
    }

    public virtual bool IsPanel(UIElement value)
    {
        return value is Panel;
    }

    UIElement IRenderer<UIElement>.Create(RendererContext<UIElement> context)
    {
        return Create(context ?? throw new InvalidOperationException()) as UIElement ?? throw new InvalidOperationException();
    }

    protected virtual TControl Create(RendererContext<UIElement> context)
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


public abstract class LazyRendererObject<TWidget, TControl> : RendererObject<TWidget, TControl>, ILazyRenderer<UIElement>
    where TWidget : MappingWidget where TControl : class, new()
{
    public bool IsVisible(UIElement control, int index)
    {
        return IsVisible(control as TControl ?? throw new InvalidOperationException(), index);
    }
    
    public UIElement? GetVisibleChild(UIElement control, int index)
    {
        return GetVisibleChild(control as TControl ?? throw new InvalidOperationException(), index);
    }

    public void UpdateChild(UIElement control, int index, UIElement childControl)
    {
        UpdateChild(control as TControl ?? throw new InvalidOperationException(), index, childControl);
    }

    protected abstract bool IsVisible(TControl control, int index);

    protected abstract UIElement? GetVisibleChild(TControl control, int index);

    protected abstract void UpdateChild(TControl control, int index, UIElement childControl);
}