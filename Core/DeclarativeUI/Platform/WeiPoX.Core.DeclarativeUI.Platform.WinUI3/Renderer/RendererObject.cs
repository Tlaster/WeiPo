using System;
using System.Linq;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using WeiPoX.Core.DeclarativeUI.Internal;
using WeiPoX.Core.DeclarativeUI.Platform.WinUI3.Internal;
using WeiPoX.Core.DeclarativeUI.Widgets;

namespace WeiPoX.Core.DeclarativeUI.Platform.WinUI3.Renderer;

internal abstract class RendererObject<TWidget, TControl> : IRenderer<UIElement>
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

    public bool IsPanel(UIElement value)
    {
        return value is Panel;
    }

    UIElement IRenderer<UIElement>.Create(WidgetBuilder<UIElement> renderer)
    {
        return Create(renderer as WidgetBuilder ?? throw new InvalidOperationException()) as UIElement ?? throw new InvalidOperationException();
    }

    protected virtual TControl Create(WidgetBuilder renderer)
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


internal abstract class LazyRendererObject<TWidget, TControl> : RendererObject<TWidget, TControl>, ILazyRenderer<Control>
    where TWidget : MappingWidget where TControl : class, new()
{
    public bool IsVisible(Control control, int index)
    {
        return IsVisible(control as TControl ?? throw new InvalidOperationException(), index);
    }
    
    public Control? GetVisibleChild(Control control, int index)
    {
        return GetVisibleChild(control as TControl ?? throw new InvalidOperationException(), index);
    }

    public void UpdateChild(Control control, int index, Control childControl)
    {
        UpdateChild(control as TControl ?? throw new InvalidOperationException(), index, childControl);
    }

    protected abstract bool IsVisible(TControl control, int index);

    protected abstract Control? GetVisibleChild(TControl control, int index);

    protected abstract void UpdateChild(TControl control, int index, Control childControl);
}