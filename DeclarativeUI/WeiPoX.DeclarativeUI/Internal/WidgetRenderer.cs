namespace WeiPoX.DeclarativeUI.Internal;

internal abstract class WidgetRenderer<T>
{
    private bool IsChanged(WidgetObject oldValue, WidgetObject newValue)
    {
        return oldValue != newValue;
    }

    private T Render(WidgetObject widget)
    {
        var renderer = GetRenderer(widget.GetType());
        var control = renderer.Create();
        renderer.Update(control, widget);
        if (widget is not IPanelWidget panel)
        {
            return control;
        }

        foreach (var childControl in panel.Children.Select(Render))
        {
            renderer.AddChild(control, childControl);
        }

        return control;
    }

    public T RenderIfNeeded(WidgetObject? oldValue, WidgetObject newValue, T? control)
    {
        if (oldValue == null || control == null)
        {
            return Render(newValue);
        }

        if (!IsChanged(oldValue, newValue))
        {
            return control;
        }

        if (oldValue.GetType() != newValue.GetType()) // tree changed
        {
            return Render(newValue);
        }

        var renderer = GetRenderer(newValue.GetType());
        renderer.Update(control, newValue);
        if (newValue is not IPanelWidget newPanel || oldValue is not IPanelWidget oldPanel || !IsPanel(control))
        {
            return control;
        }

        var oldChildren = oldPanel.Children;
        var newChildren = newPanel.Children;
        var oldCount = oldChildren.Count;
        var newCount = newChildren.Count;
        var count = Math.Max(oldCount, newCount);
        for (var i = 0; i < count; i++)
        {
            var oldChild = oldChildren.ElementAtOrDefault(i);
            var newChild = newChildren.ElementAtOrDefault(i);
            var oldChildControl = GetChildAt(control, i);
            if (oldChildControl == null && newChild != null)
            {
                renderer.AddChild(control, Render(newChild));
            }
            else if (oldChildControl != null && newChild ==  null)
            {
                renderer.RemoveChild(control, oldChildControl);
            }
            else if (oldChildControl != null && newChild != null)
            {
                var newChildControl = RenderIfNeeded(oldChild, newChild, oldChildControl);
                if (!ReferenceEquals(newChildControl, oldChildControl))
                {
                    renderer.ReplaceChild(control, i, newChildControl);
                }
            }
            else if (oldChildControl == null && newChild == null)
            {
                // WTF?
            }
        }

        return control;
    }

    protected abstract IRenderer<T> GetRenderer(Type widgetType);
    protected abstract bool IsPanel(T value);
    protected abstract T? GetChildAt(T control, int index);
}