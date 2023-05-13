using System.Collections.Immutable;
using WeiPoX.Core.DeclarativeUI.Widgets;

namespace WeiPoX.Core.DeclarativeUI.Internal;

public partial class WidgetBuilder<T>
{
    private Task<T> CreateAsync(Widget widget, BuildContext context, IBuildOwner owner)
    {
        return widget switch
        {
            MappingWidget mappingWidget => CreateMappingWidgetAsync(mappingWidget, context, owner),
            StateWidget stateWidget => CreateStateWidgetAsync(stateWidget, context, owner),
            _ => throw new NotSupportedException()
        };
    }

    private Task<T> CreateStateWidgetAsync(StateWidget widget, BuildContext context, IBuildOwner owner)
    {
        switch (widget)
        {
            case ContextProvider contextProvider:
                return CreateAsync(contextProvider.Child,
                    context.Merge((contextProvider as IContextProvider).Providers), owner);
            case StatefulWidget statefulWidget:
                statefulWidget.State.BuildOwner = owner;
                var result = BuildStatefulWidget(statefulWidget, context);
                return CreateAsync(result, context, owner);
            case StatelessWidget statelessWidget:
                return CreateAsync(statelessWidget.Content, context, owner);
            default:
                throw new NotImplementedException();
        }
    }

    private async Task<T> CreateMappingWidgetAsync(MappingWidget widget, BuildContext context, IBuildOwner owner)
    {
        var renderer = GetRenderer(widget.GetType());
        var control = renderer.Create(new RendererContext<T>(this, owner));
        renderer.Update(control, widget);

        if (widget is not IPanelWidget panel || !renderer.IsPanel(control))
        {
            return control;
        }

        var child = await Task.WhenAll(panel.Children.Select(it => CreateAsync(it, context, owner)));
        foreach (var c in child)
        {
            renderer.AddChild(control, c);
        }

        return control;
    }


    public Task<T> BuildIfNeededAsync(Widget? oldValue, Widget newValue, T? control, IBuildOwner owner)
    {
        return BuildIfNeededAsync(oldValue, newValue, control,
            new BuildContext(ImmutableDictionary<Type, object>.Empty), owner);
    }

    private Task<T> BuildIfNeededAsync(Widget? oldValue, Widget newValue, T? control, BuildContext context, IBuildOwner owner)
    {
        if (oldValue == null || control == null)
        {
            return CreateAsync(newValue, context, owner);
        }

        if (oldValue.GetType() != newValue.GetType()) // tree changed
        {
            return CreateAsync(newValue, context, owner);
        }

        return newValue switch
        {
            StateWidget stateWidget => BuildStateWidgetAsync(oldValue as StateWidget, stateWidget, control, context, owner),
            MappingWidget mappingWidget => BuildMappingWidgetAsync(oldValue, mappingWidget, control, context, owner),
            _ => throw new NotImplementedException()
        };
    }

    private Task<T> BuildStateWidgetAsync(StateWidget? oldValue, StateWidget newValue, T control, BuildContext context, IBuildOwner owner)
    {
        switch (newValue)
        {
            case ContextProvider contextProvider:
                return BuildIfNeededAsync((oldValue as ContextProvider)?.Child, contextProvider.Child, control,
                    context.Merge((contextProvider as IContextProvider).Providers), owner);
            case StatefulWidget statefulWidget:
                if (oldValue is StatefulWidget oldStatefulWidget && !ReferenceEquals(oldValue, newValue))
                {
                    statefulWidget.Merge(oldStatefulWidget);
                }

                var oldBuild = (oldValue as StatefulWidget)?.State.CachedBuild;
                var forceRebuild = oldBuild != newValue;
                var newBuild = BuildStatefulWidget(statefulWidget, context, forceRebuild);
                return BuildIfNeededAsync(oldBuild, newBuild, control, context, owner);
            case StatelessWidget statelessWidget:
                return BuildIfNeededAsync((oldValue as StatelessWidget)?.Content, statelessWidget.Content, control,
                    context, owner);
            default:
                throw new NotImplementedException();
        }
    }

    private async Task<T> BuildMappingWidgetAsync(
        Widget oldValue,
        MappingWidget newValue,
        T control,
        BuildContext context,
        IBuildOwner owner
        )
    {
        if (!IsChanged(oldValue, newValue, owner))
        {
            return control;
        }

        var renderer = GetRenderer(newValue.GetType());
        renderer.Update(control, newValue);
        if (newValue is IPanelWidget newPanel && oldValue is IPanelWidget oldPanel &&
            renderer.IsPanel(control))
        {
            await BuildPanel(control, context, oldPanel, newPanel, renderer, owner);
        }

        if (newValue is ILazyWidget newLazyWidget && oldValue is ILazyWidget oldLazyWidget &&
            renderer is ILazyRenderer<T> lazyRenderer)
        {
            await BuildLazyWidget(control, context, oldLazyWidget, newLazyWidget, lazyRenderer, owner);
        }

        return control;
    }

    private async Task BuildLazyWidget(
        T control,
        BuildContext context,
        ILazyWidget oldLazyWidget,
        ILazyWidget newLazyWidget,
        ILazyRenderer<T> lazyRenderer,
        IBuildOwner owner
    )
    {
        var oldItemsCount = oldLazyWidget.Count;
        var newItemsCount = newLazyWidget.Count;
        var range = lazyRenderer.GetVisibleRange(control);
        if (range.Start.Value == -1 || range.End.Value <= 0)
        {
            return;
        }
        var actualEnd = Math.Min(newItemsCount, range.End.Value);
        var actualStart = Math.Max(0, range.Start.Value);
        var count = actualEnd - actualStart;
        
        await Task.WhenAll(Enumerable.Range(actualStart, count).Select(async i =>
        {
            if (lazyRenderer.IsVisible(control, i))
            {
                var oldItem = oldLazyWidget.GetBuilder(i);
                var newItem = newLazyWidget.GetBuilder(i);
                if (oldItem != null && newItem != null)
                {
                    var oldItemControl = lazyRenderer.GetVisibleChild(control, i);
                    var newItemControl = await BuildIfNeededAsync(
                        oldItem.Invoke(),
                        newItem.Invoke(),
                        oldItemControl, 
                        context, 
                        owner);
                    lazyRenderer.UpdateChild(control, i, newItemControl);
                }
            }
        }));
    }

    private async Task BuildPanel(
        T control,
        BuildContext context,
        IPanelWidget oldPanel,
        IPanelWidget newPanel,
        IRenderer<T> renderer,
        IBuildOwner owner
    )
    {
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
                var newChildControl = await CreateAsync(newChild, context, owner);
                renderer.AddChild(control, newChildControl);
                OnChildAdded(newChild);
            }
            else if (oldChildControl != null && newChild == null)
            {
                renderer.RemoveChild(control, oldChildControl);
                OnChildRemoved(oldChild);
            }
            else if (oldChildControl != null && newChild != null)
            {
                var newChildControl = await BuildIfNeededAsync(oldChild, newChild, oldChildControl, context, owner);
                if (!ReferenceEquals(newChildControl, oldChildControl))
                {
                    renderer.ReplaceChild(control, i, newChildControl);
                    OnChildRemoved(oldChild);
                    OnChildAdded(newChild);
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
    }
}