using System.Collections.Immutable;
using WeiPoX.Core.DeclarativeUI.Widgets;

namespace WeiPoX.Core.DeclarativeUI.Internal;

public abstract partial class WidgetBuilder<T>
{
    private bool IsChanged(Widget oldValue, Widget newValue, IBuildOwner owner)
    {
        return oldValue != newValue || owner.IsBuildScheduled(newValue);
    }

    private bool IsContextChanged(StatefulWidget widget, BuildContext context)
    {
        foreach (var (key, value) in widget.State.UsedProviders)
        {
            if (!context.ContextMap.ContainsKey(key) || context.ContextMap[key] != value)
            {
                return true;
            }
        }

        return false;
    }

    private Widget BuildStatefulWidget(StatefulWidget statefulWidget, BuildContext context, bool forceRebuild = false)
    {
        statefulWidget.State.BuildContext = context;
        if (!forceRebuild && statefulWidget.State is { Dirty: false, CachedBuild: { } } &&
            !IsContextChanged(statefulWidget, context))
        {
            return statefulWidget.State.CachedBuild;
        }

        var result = statefulWidget.BuildInternal();
        statefulWidget.State.CachedBuild = result;
        return result;
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

    protected abstract IRenderer<T> GetRenderer(Type widgetType);
}