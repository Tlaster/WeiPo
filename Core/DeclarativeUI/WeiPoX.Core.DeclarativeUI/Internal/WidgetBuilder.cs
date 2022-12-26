using System.Collections.Immutable;
using System.Diagnostics;
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

    private T Create(Widget widget, BuildContext context)
    {
        return widget switch
        {
            MappingWidget mappingWidget => CreateMappingWidget(mappingWidget, context),
            StateWidget stateWidget => CreateStateWidget(stateWidget, context),
            _ => throw new NotSupportedException()
        };
    }

    private T CreateStateWidget(StateWidget widget, BuildContext context)
    {
        switch (widget)
        {
            case ContextProvider contextProvider:
                return Create(contextProvider.Child, context + contextProvider.Providers);
            case StatefulWidget statefulWidget:
                statefulWidget.BuildOwner = _owner;
                statefulWidget.BuildContext = context;
                var result = BuildStatefulWidget(statefulWidget);
                return Create(result, context);
            case StatelessWidget statelessWidget:
                return Create(statelessWidget.Content, context);
            default:
                throw new NotImplementedException();
        }
    }

    private T CreateMappingWidget(MappingWidget widget, BuildContext context)
    {
        var renderer = GetRenderer(widget.GetType());
        var control = renderer.Create();
        renderer.Update(control, widget);

        if (widget is not IPanelWidget panel)
        {
            return control;
        }

        foreach (var childControl in panel.Children.Select(it => Create(it, context)))
        {
            renderer.AddChild(control, childControl);
        }

        return control;
    }
    
    public T BuildIfNeeded(Widget? oldValue, Widget newValue, T? control, BuildContext context)
    {
        if (oldValue == null || control == null)
        {
            return Create(newValue, context);
        }

        if (!IsChanged(oldValue, newValue))
        {
            return control;
        }

        if (oldValue.GetType() != newValue.GetType()) // tree changed
        {
            return Create(newValue, context);
        }

        return newValue switch
        {
            StateWidget stateWidget => BuildStateWidget(oldValue as StateWidget, stateWidget, control, context),
            MappingWidget mappingWidget => BuildMappingWidget(oldValue, mappingWidget, control, context),
            _ => throw new NotImplementedException()
        };
    }
    
    public T BuildIfNeeded(Widget? oldValue, Widget newValue, T? control)
    {
        return BuildIfNeeded(oldValue, newValue, control, new BuildContext(ImmutableDictionary<Type, object>.Empty));
    }

    private T BuildStateWidget(StateWidget? oldValue, StateWidget newValue, T control, BuildContext context)
    {
        switch (newValue)
        {
            case ContextProvider contextProvider:
                return BuildIfNeeded((oldValue as ContextProvider)?.Child, contextProvider.Child, control, context + contextProvider.Providers);
            case StatefulWidget statefulWidget:
                // check if the new value is not created from CreateStateWidget
                // it only occurs when the parent widget is rebuilt
                // Test case: TestHooksUseContextWidget
                // TODO: optimize: only rebuild the parent widget and the sub tree that use the context, not the whole tree
                if (oldValue != null && !ReferenceEquals(oldValue, newValue))
                {
                    // go through the create process
                    return CreateStateWidget(statefulWidget, context);
                }
                else
                {
                    var oldBuild = (oldValue as StatefulWidget)?.CachedBuild;
                    var newBuild = BuildStatefulWidget(statefulWidget);
                    return BuildIfNeeded(oldBuild, newBuild, control, context);
                }
            case StatelessWidget statelessWidget:
                return BuildIfNeeded((oldValue as StatelessWidget)?.Content, statelessWidget.Content, control, context);
            default:
                throw new NotImplementedException();
        }
    }

    private T BuildMappingWidget(Widget oldValue, MappingWidget newValue, T control, BuildContext context)
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
                var newChildControl = Create(newChild, context);
                renderer.AddChild(control, newChildControl);
                OnChildAdded(newChild, newChildControl);
            }
            else if (oldChildControl != null && newChild == null)
            {
                renderer.RemoveChild(control, oldChildControl);
                OnChildRemoved(oldChild, oldChildControl);
            }
            else if (oldChildControl != null && newChild != null)
            {
                var newChildControl = BuildIfNeeded(oldChild, newChild, oldChildControl, context);
                if (!ReferenceEquals(newChildControl, oldChildControl))
                {
                    renderer.ReplaceChild(control, i, newChildControl);
                    OnChildRemoved(oldChild, oldChildControl);
                    OnChildAdded(newChild, newChildControl);
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
    
    private void OnChildAdded(Widget child, T childControl)
    {
    }

    private void OnChildRemoved(Widget? oldChild, T oldChildControl)
    {
        if (oldChild is IDisposable disposable)
        {
            disposable.Dispose();
        }
    }
    
    private static Widget BuildStatefulWidget(StatefulWidget statefulWidget)
    {
        var result = statefulWidget.BuildInternal();
        statefulWidget.CachedBuild = result;
        return result;
    }

    protected abstract IRenderer<T> GetRenderer(Type widgetType);
    protected abstract bool IsPanel(T value);
    protected abstract T? GetChildAt(T control, int index);
}