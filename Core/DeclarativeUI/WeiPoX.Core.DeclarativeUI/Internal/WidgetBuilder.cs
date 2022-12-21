using WeiPoX.Core.DeclarativeUI.Widgets;

namespace WeiPoX.Core.DeclarativeUI.Internal;

internal abstract class WidgetBuilder<T>
{
    private readonly IBuildOwner _owner;

    protected WidgetBuilder(IBuildOwner owner)
    {
        _owner = owner;
    }

    private bool IsChanged(Widget oldValue, Widget newValue)
    {
        return oldValue != newValue || _owner.IsBuildScheduled(newValue);
    }

    private T Create(Widget widget)
    {
        return widget switch
        {
            MappingWidget mappingWidget => CreateMappingWidget(mappingWidget),
            StateWidget stateWidget => CreateStateWidget(stateWidget),
            _ => throw new NotSupportedException()
        };
    }

    private T CreateStateWidget(StateWidget widget)
    {
        switch (widget)
        {
            case StatefulWidget statefulWidget:
                statefulWidget.BuildOwner = _owner;
                var result = statefulWidget.BuildInternal();
                statefulWidget.CachedBuild = result;
                return Create(result);
            case StatelessWidget statelessWidget:
                return Create(statelessWidget.Content);
            default:
                throw new ArgumentOutOfRangeException(nameof(widget));
        }
    }

    private T CreateMappingWidget(MappingWidget widget)
    {
        var renderer = GetRenderer(widget.GetType());
        var control = renderer.Create();
        renderer.Update(control, widget);

        if (widget is not IPanelWidget panel)
        {
            return control;
        }

        foreach (var childControl in panel.Children.Select(Create))
        {
            renderer.AddChild(control, childControl);
        }

        return control;
    }

    public T BuildIfNeeded(Widget? oldValue, Widget newValue, T? control)
    {
        if (oldValue == null || control == null)
        {
            return Create(newValue);
        }

        if (!IsChanged(oldValue, newValue))
        {
            return control;
        }

        if (oldValue.GetType() != newValue.GetType()) // tree changed
        {
            return Create(newValue);
        }

        return newValue switch
        {
            StateWidget stateWidget => BuildStateWidget(oldValue as StateWidget, stateWidget, control),
            MappingWidget mappingWidget => BuildMappingWidget(oldValue, mappingWidget, control),
            _ => throw new NotImplementedException()
        };
    }

    private T BuildStateWidget(StateWidget? oldValue, StateWidget newValue, T control)
    {
        switch (newValue)
        {
            case StatefulWidget statefulWidget:
                var result = statefulWidget.BuildInternal();
                statefulWidget.CachedBuild = result;
                return BuildIfNeeded((oldValue as StatefulWidget)?.CachedBuild, result, control);
            case StatelessWidget statelessWidget:
                return BuildIfNeeded((oldValue as StatelessWidget)?.Content, statelessWidget.Content, control);
            default:
                throw new ArgumentOutOfRangeException(nameof(newValue));
        }
    }

    private T BuildMappingWidget(Widget oldValue, MappingWidget newValue, T control)
    {
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
                renderer.AddChild(control, Create(newChild));
            }
            else if (oldChildControl != null && newChild == null)
            {
                renderer.RemoveChild(control, oldChildControl);
            }
            else if (oldChildControl != null && newChild != null)
            {
                var newChildControl = BuildIfNeeded(oldChild, newChild, oldChildControl);
                if (!ReferenceEquals(newChildControl, oldChildControl))
                {
                    renderer.ReplaceChild(control, i, newChildControl);
                }
            }
            else if (oldChildControl == null && newChild == null)
            {
                // WTF?
#if DEBUG
                throw new InvalidOperationException();
#endif
            }
        }

        return control;
    }

    protected abstract IRenderer<T> GetRenderer(Type widgetType);
    protected abstract bool IsPanel(T value);
    protected abstract T? GetChildAt(T control, int index);
}