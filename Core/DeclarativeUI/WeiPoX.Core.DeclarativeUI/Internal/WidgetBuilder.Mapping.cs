using System.Collections.Immutable;
using WeiPoX.Core.DeclarativeUI.Widgets;

namespace WeiPoX.Core.DeclarativeUI.Internal;

public partial class WidgetBuilder<T>
{
    // widget -> element -> mapping
    public async Task<T> Build(Widget? oldValue, Widget newValue, T? control, IBuildOwner owner)
    {
        var element = await BuildElement(oldValue, newValue, owner);
        return ApplyElement(element, control, owner);
    }

    internal Task<Element> BuildElement(Widget? oldValue, Widget newValue, IBuildOwner owner)
    {
        return BuildElement(oldValue, newValue,
            new BuildContext(ImmutableDictionary<Type, object>.Empty), owner);
    }

    private async Task<Element> BuildElement(Widget? oldValue, Widget newValue, BuildContext context,
        IBuildOwner owner)
    {
        if (oldValue == null)
        {
            return await CreateElementAsync(newValue, context, owner);
        }

        if (oldValue.GetType() != newValue.GetType()) // tree changed
        {
            return await CreateElementAsync(newValue, context, owner) with { ShouldReCreate = true };
        }

        return newValue switch
        {
            StateWidget stateWidget => await BuildStateWidgetElementAsync(oldValue as StateWidget, stateWidget, context,
                owner),
            MappingWidget mappingWidget => await BuildMappingWidgetElementAsync(oldValue, mappingWidget, context, owner),
            _ => throw new NotImplementedException()
        };
    }

    internal T ApplyElement(Element element, T? control, IBuildOwner owner)
    {
        var renderer = GetRenderer(element.Widget.GetType());
        if (control == null || element.ShouldReCreate)
        {
            var actualControl = renderer.Create(new RendererContext<T>(this, owner));
            renderer.Update(actualControl, element.Widget);
            if (renderer.IsPanel(actualControl))
            {
                foreach (var (key, value) in element.Children)
                {
                    renderer.AddChild(actualControl, ApplyElement(value, default, owner));
                }
            }

            return actualControl;
        }

        if (!element.IsDirty)
        {
            return control;
        }
        renderer.Update(control, element.Widget);
        if (renderer.IsPanel(control))
        {
            element.ChildrenToRemove.ForEach(index =>
            {
                var child = renderer.GetChildAt(control, index);
                if (child != null)
                {
                    renderer.RemoveChild(control, child);
                }
            });
            foreach (var (childIndex, childElement) in element.Children)
            {
                var child = renderer.GetChildAt(control, childIndex);
                if (child == null)
                {
                    renderer.AddChild(control, ApplyElement(childElement, default, owner));
                }
                else if (childElement.ShouldReCreate)
                {
                    renderer.ReplaceChild(control, childIndex, ApplyElement(childElement, default, owner));
                }
                else
                {
                    ApplyElement(childElement, child, owner);
                }
            }
        }

        if (renderer is ILazyRenderer<T> lazyRenderer && element.Widget is ILazyWidget lazyWidget)
        {
            lazyWidget.GetVisibleRange = () => lazyRenderer.GetVisibleRange(control);
            foreach (var (key, value) in element.Children)
            {
                if (!lazyRenderer.IsVisible(control, key))
                {
                    continue;
                }

                var child = lazyRenderer.GetVisibleChild(control, key);
                if (child == null)
                {
                    lazyRenderer.UpdateChild(control, key, ApplyElement(value, default, owner));
                }
                else
                {
                    ApplyElement(value, child, owner);
                }
            }
        }

        return control;
    }

    private Task<Element> CreateElementAsync(Widget widget, BuildContext context, IBuildOwner owner)
    {
        return widget switch
        {
            MappingWidget mappingWidget => CreateMappingWidgetElementAsync(mappingWidget, context, owner),
            StateWidget stateWidget => CreateStateWidgetElementAsync(stateWidget, context, owner),
            _ => throw new NotSupportedException()
        };
    }

    private Task<Element> CreateStateWidgetElementAsync(StateWidget widget, BuildContext context, IBuildOwner owner)
    {
        switch (widget)
        {
            case ContextProvider contextProvider:
                return CreateElementAsync(contextProvider.Child,
                    context.Merge((contextProvider as IContextProvider).Providers), owner);
            case StatefulWidget statefulWidget:
                statefulWidget.State.BuildOwner = owner;
                var result = BuildStatefulWidget(statefulWidget, context);
                return CreateElementAsync(result, context, owner);
            case StatelessWidget statelessWidget:
                return CreateElementAsync(statelessWidget.Content, context, owner);
            default:
                throw new NotImplementedException();
        }
    }

    private async Task<Element> CreateMappingWidgetElementAsync(MappingWidget widget, BuildContext context,
        IBuildOwner owner)
    {
        if (widget is not IPanelWidget panel)
        {
            return new Element(widget);
        }

        var child = await Task.WhenAll(panel.Children.Select(it => CreateElementAsync(it, context, owner)));
        return new Element(widget,
            child.Select((it, index) => (index, it)).ToImmutableDictionary(it => it.index, it => it.it));
    }


    private Task<Element> BuildStateWidgetElementAsync(StateWidget? oldValue, StateWidget newValue,
        BuildContext context, IBuildOwner owner)
    {
        switch (newValue)
        {
            case ContextProvider contextProvider:
                return BuildElement((oldValue as ContextProvider)?.Child, contextProvider.Child,
                    context.Merge((contextProvider as IContextProvider).Providers), owner);
            case StatefulWidget statefulWidget:
                if (oldValue is StatefulWidget oldStatefulWidget && !ReferenceEquals(oldValue, newValue))
                {
                    statefulWidget.Merge(oldStatefulWidget);
                }

                var oldBuild = (oldValue as StatefulWidget)?.State.CachedBuild;
                var forceRebuild = oldBuild != newValue;
                var newBuild = BuildStatefulWidget(statefulWidget, context, forceRebuild);
                return BuildElement(oldBuild, newBuild, context, owner);
            case StatelessWidget statelessWidget:
                return BuildElement((oldValue as StatelessWidget)?.Content, statelessWidget.Content,
                    context, owner);
            default:
                throw new NotImplementedException();
        }
    }

    private async Task<Element> BuildMappingWidgetElementAsync(
        Widget oldValue,
        MappingWidget newValue,
        BuildContext context,
        IBuildOwner owner
    )
    {
        var isDirty = IsChanged(oldValue, newValue, owner);
        return newValue switch
        {
            IPanelWidget newPanel when oldValue is IPanelWidget oldPanel => await BuildPanelElement(context, oldPanel,
                newPanel, owner) with { IsDirty = isDirty },
            ILazyWidget newLazyWidget when oldValue is ILazyWidget oldLazyWidget => await BuildLazyWidgetElement(
                context, oldLazyWidget, newLazyWidget, owner) with { IsDirty = isDirty },
            _ => new Element(newValue, IsDirty: isDirty),
        };
    }


    private async Task<Element> BuildLazyWidgetElement(
        BuildContext context,
        ILazyWidget oldLazyWidget,
        ILazyWidget newLazyWidget,
        IBuildOwner owner
    )
    {
        var oldItemsCount = oldLazyWidget.Count;
        var newItemsCount = newLazyWidget.Count;
        var nullableRange = oldLazyWidget.GetVisibleRange?.Invoke();
        if (newLazyWidget is not MappingWidget newMappingWidget)
        {
            throw new InvalidOperationException();
        }
        if (nullableRange is null)
        {
            // initial rendering, just skip it.
            return new Element(newMappingWidget);
        }

        var range = nullableRange.Value;
        // avoid null if widget is being build multiple times before into mapping stage
        newLazyWidget.GetVisibleRange = oldLazyWidget.GetVisibleRange;

        if (range.Start.Value == -1 || range.End.Value <= 0)
        {
            return new Element(newMappingWidget);
        }

        var actualEnd = Math.Min(newItemsCount, range.End.Value + 1);
        var actualStart = Math.Max(0, range.Start.Value);
        var count = actualEnd - actualStart;

        var children = await Task.WhenAll(Enumerable.Range(actualStart, count).Select(async i =>
        {
            var oldItem = oldLazyWidget.GetBuilder(i);
            var newItem = newLazyWidget.GetBuilder(i);
            Element? newItemElement;
            if (oldItem != null && newItem != null)
            {
                newItemElement = await BuildElement(
                    oldItem.Invoke(),
                    newItem.Invoke(),
                    context,
                    owner);
            }
            else
            {
                newItemElement = null;
            }

            return newItemElement == null ? null : new Tuple<int, Element>(i, newItemElement);
        }));
        return new Element(newMappingWidget,
            children.Where(it => it != null).ToImmutableDictionary(it => it!.Item1, it => it!.Item2));
    }


    private async Task<Element> BuildPanelElement(
        BuildContext context,
        IPanelWidget oldPanel,
        IPanelWidget newPanel,
        IBuildOwner owner
    )
    {
        if (newPanel is not MappingWidget newMappingPanel)
        {
            throw new NotSupportedException();
        }

        var oldChildren = oldPanel.Children;
        var newChildren = newPanel.Children;
        var oldCount = oldChildren.Count;
        var newCount = newChildren.Count;
        var count = Math.Max(oldCount, newCount);
        var children = await Task.WhenAll(Enumerable.Range(0, count).Select(async i =>
        {
            var oldChild = oldChildren.ElementAtOrDefault(i);
            var newChild = newChildren.ElementAtOrDefault(i);
            Element? newChildElement;
            if (oldChild == null && newChild != null)
            {
                newChildElement = await CreateElementAsync(newChild, context, owner);
                OnChildAdded(newChild);
            }
            else if (oldChild != null && newChild == null)
            {
                newChildElement = null;
                OnChildRemoved(oldChild);
            }
            else if (oldChild != null && newChild != null)
            {
                newChildElement = await BuildElement(oldChild, newChild, context, owner);
                if (oldChild.GetType() != newChild.GetType()) // tree changed
                {
                    OnChildRemoved(oldChild);
                    OnChildAdded(newChild);
                }
            }
            else
            {
                newChildElement = null;
                // WTF?
#if DEBUG
                throw new InvalidOperationException();
#endif
            }

            return newChildElement;
        }));
        return new Element(newMappingPanel,
            children.Where(it => it != null).Select(it => it!).Select((it, index) => (index, it))
                .ToImmutableDictionary(it => it.index, it => it.it),
            ChildrenToRemove: children.Where(it => it == null).Select((it, index) => index).ToImmutableList());
    }

    internal record Element(MappingWidget Widget, ImmutableDictionary<int, Element> Children,
        ImmutableList<int> ChildrenToRemove, bool IsDirty = true, bool ShouldReCreate = false)
    {
        public Element(MappingWidget widget, ImmutableDictionary<int, Element> children, bool IsDirty = true,
            bool ShouldReCreate = false) : this(widget, children,
            ImmutableList<int>.Empty, IsDirty, ShouldReCreate)
        {
        }

        public Element(MappingWidget widget, bool IsDirty = true, bool ShouldReCreate = false) : this(widget,
            ImmutableDictionary<int, Element>.Empty,
            ImmutableList<int>.Empty, IsDirty, ShouldReCreate)
        {
        }
    }
}