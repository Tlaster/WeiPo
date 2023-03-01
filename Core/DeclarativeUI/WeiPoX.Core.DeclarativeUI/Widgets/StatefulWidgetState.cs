using System.Collections.ObjectModel;
using System.Collections.Specialized;
using WeiPoX.Core.DeclarativeUI.Internal;

namespace WeiPoX.Core.DeclarativeUI.Widgets;

internal class StatefulWidgetState : IDisposable
{
    public StatefulWidgetState()
    {
        Hooks.CollectionChanged += (sender, args) =>
        {
            if (args.Action != NotifyCollectionChangedAction.Replace)
            {
                return;
            }

            if (Dirty)
            {
                return;
            }

            Dirty = true;
#if DEBUG
            if (BuildOwner == null)
            {
                throw new Exception("BuildOwner is null");
            }
#endif
            if (Widget != null)
            {
                BuildOwner?.MarkNeedsBuild(Widget);
            }
        };
    }

    public ObservableCollection<object> Hooks { get; } = new();
    public int HookId { get; set; }
    public bool Dirty { get; set; }
    public IBuildOwner? BuildOwner {
        get;
        set;
    }
    public BuildContext? BuildContext { get; set; }
    public Dictionary<Type, object> UsedProviders { get; } = new();
    public Widget? Widget { get; set; }
    internal Widget? CachedBuild { get; set; }

    public void Dispose()
    {
        foreach (var hook in Hooks)
        {
            if (hook is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }
    }
}