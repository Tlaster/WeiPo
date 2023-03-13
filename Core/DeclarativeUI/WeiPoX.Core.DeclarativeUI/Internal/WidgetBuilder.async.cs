using System.Collections.Immutable;
using WeiPoX.Core.DeclarativeUI.Widgets;

namespace WeiPoX.Core.DeclarativeUI.Internal;

internal partial class WidgetBuilder<T>
{
    

    private Task<T> CreateAsync(Widget widget, BuildContext context)
    {
        return widget switch
        {
            MappingWidget mappingWidget => CreateMappingWidgetAsync(mappingWidget, context),
            StateWidget stateWidget => CreateStateWidgetAsync(stateWidget, context),
            _ => throw new NotSupportedException()
        };
    }

    private Task<T> CreateStateWidgetAsync(StateWidget widget, BuildContext context)
    {
        switch (widget)
        {
            case ContextProvider contextProvider:
                return CreateAsync(contextProvider.Child, context.Merge((contextProvider as IContextProvider).Providers));
            case StatefulWidget statefulWidget:
                statefulWidget.State.BuildOwner = _owner;
                var result = BuildStatefulWidget(statefulWidget, context);
                return CreateAsync(result, context);
            case StatelessWidget statelessWidget:
                return CreateAsync(statelessWidget.Content, context);
            default:
                throw new NotImplementedException();
        }
    }

    private async Task<T> CreateMappingWidgetAsync(MappingWidget widget, BuildContext context)
    {
        var renderer = GetRenderer(widget.GetType());
        var control = renderer.Create();
        renderer.Update(control, widget);

        if (widget is not IPanelWidget panel)
        {
            return control;
        }
        var child = await Task.WhenAll(panel.Children.Select(it => CreateAsync(it, context)));
        foreach (var c in child)
        {
            renderer.AddChild(control, c);
        }
        return control;
    }

    
    public Task<T> BuildIfNeededAsync(Widget? oldValue, Widget newValue, T? control)
    {
        return BuildIfNeededAsync(oldValue, newValue, control, new BuildContext(ImmutableDictionary<Type, object>.Empty));
    }

    private Task<T> BuildIfNeededAsync(Widget? oldValue, Widget newValue, T? control, BuildContext context)
    {
        if (oldValue == null || control == null)
        {
            return Task.FromResult(Create(newValue, context));
        }

        if (oldValue.GetType() != newValue.GetType()) // tree changed
        {
            return Task.FromResult(Create(newValue, context));
        }

        return newValue switch
        {
            StateWidget stateWidget => BuildStateWidgetAsync(oldValue as StateWidget, stateWidget, control, context),
            MappingWidget mappingWidget => BuildMappingWidgetAsync(oldValue, mappingWidget, control, context),
            _ => throw new NotImplementedException()
        };
    }

    private Task<T> BuildStateWidgetAsync(StateWidget? oldValue, StateWidget newValue, T control, BuildContext context)
    {
        switch (newValue)
        {
            case ContextProvider contextProvider:
                return BuildIfNeededAsync((oldValue as ContextProvider)?.Child, contextProvider.Child, control,
                    context.Merge((contextProvider as IContextProvider).Providers));
            case StatefulWidget statefulWidget:
                if (oldValue is StatefulWidget oldStatefulWidget && !ReferenceEquals(oldValue, newValue))
                {
                    statefulWidget.Merge(oldStatefulWidget);
                }

                var oldBuild = (oldValue as StatefulWidget)?.State.CachedBuild;
                var forceRebuild = oldBuild != newValue;
                var newBuild = BuildStatefulWidget(statefulWidget, context, forceRebuild);
                return BuildIfNeededAsync(oldBuild, newBuild, control, context);
            case StatelessWidget statelessWidget:
                return BuildIfNeededAsync((oldValue as StatelessWidget)?.Content, statelessWidget.Content, control, context);
            default:
                throw new NotImplementedException();
        }
    }

    private async Task<T> BuildMappingWidgetAsync(Widget oldValue, MappingWidget newValue, T control, BuildContext context)
    {
        if (!IsChanged(oldValue, newValue))
        {
            return control;
        }

        var renderer = GetRenderer(newValue.GetType());
        renderer.Update(control, newValue);
        if (newValue is not IPanelWidget newPanel || oldValue is not IPanelWidget oldPanel || !renderer.IsPanel(control))
        {
            return control;
        }   

        var oldChildren = oldPanel.Children;
        var newChildren = newPanel.Children;
        var oldCount = oldChildren.Count;
        var newCount = newChildren.Count;
        var count = Math.Max(oldCount, newCount);
        await Task.WhenAll(Enumerable.Range(0, count).Select(async i =>
        {
            var oldChild = oldChildren.ElementAtOrDefault(i);
            var newChild = newChildren.ElementAtOrDefault(i);
            var oldChildControl = renderer.GetChildAt(control, i);
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
                var newChildControl = await BuildIfNeededAsync(oldChild, newChild, oldChildControl, context);
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
        }));

        return control;
    }
}