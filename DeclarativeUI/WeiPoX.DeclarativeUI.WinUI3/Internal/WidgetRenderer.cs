using System;
using System.Linq;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace WeiPoX.DeclarativeUI.WinUI3.Internal;

internal class WidgetRenderer
{
    private bool IsChanged(WidgetObject oldValue, WidgetObject newValue)
    {
        return oldValue != newValue;
    }

    private FrameworkElement Render(WidgetObject widget)
    {
        var renderer = RendererPool.GetRenderer(widget.GetType());
        var control = renderer.Create();
        renderer.Update(control, widget);
        if (widget is IPanelWidget panel)
        {
            foreach (var childControl in panel.Children.Select(Render))
            {
                renderer.AddChild(control, childControl);
            }
        }

        return control;
    }

    public FrameworkElement RenderIfNeeded(WidgetObject? oldValue, WidgetObject newValue, FrameworkElement? control)
    {
        if (oldValue == null || control == null)
        {
            return Render(newValue);
        }

        if (!IsChanged(oldValue, newValue))
        {
            return control;
        }

        var oldRenderer = RendererPool.GetRenderer(oldValue.GetType());
        var newRenderer = RendererPool.GetRenderer(newValue.GetType());
        if (oldRenderer != newRenderer) // tree changed
        {
            return Render(newValue);
        }

        newRenderer.Update(control, newValue);
        if (newValue is IPanelWidget newPanel && oldValue is IPanelWidget oldPanel && control is Grid controlPanel)
        {
            var oldChildren = oldPanel.Children;
            var newChildren = newPanel.Children;
            var oldCount = oldChildren.Count;
            var newCount = newChildren.Count;
            var count = Math.Max(oldCount, newCount);
            for (var i = 0; i < count; i++)
            {
                var oldChild = oldChildren.ElementAtOrDefault(i);
                var newChild = newChildren.ElementAtOrDefault(i);
                var childControl = controlPanel.Children.ElementAtOrDefault(i);
                if (oldChild == null && newChild != null)
                {
                    newRenderer.AddChild(control, Render(newChild));
                }
                else if (newChild == null && childControl is Control childControl1)
                {
                    newRenderer.RemoveChild(control, childControl1);
                }
                else if (newChild != null && childControl is Control childControl2)
                {
                    var renderedChild = RenderIfNeeded(oldChild, newChild, childControl2);
                    if (!ReferenceEquals(renderedChild, childControl))
                    {
                        newRenderer.ReplaceChild(control, i, renderedChild);
                    }
                }
            }
        }

        return control;
    }
}