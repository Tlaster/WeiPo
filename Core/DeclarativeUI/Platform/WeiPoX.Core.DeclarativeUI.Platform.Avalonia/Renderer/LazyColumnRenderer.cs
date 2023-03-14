using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Styling;
using WeiPoX.Core.DeclarativeUI.Widgets.Layout;

namespace WeiPoX.Core.DeclarativeUI.Platform.Avalonia.Renderer;

internal class LazyColumnRenderer : RendererObject<LazyColumn, WeiPoXItemsRepeater>
{
    
    protected override void Update(WeiPoXItemsRepeater control, LazyColumn widget)
    {
        if (!control.CachedItems.SequenceEqual(widget.Items))
        {
            control.SetItems(widget.GenerateActualLazyItems());
            control.CachedItems = widget.Items;
        }
    }
}

internal class WeiPoXItemsRepeater : ScrollViewer, IStyleable
{
    Type IStyleable.StyleKey => typeof(ScrollViewer);
    private readonly ItemsRepeater _repeater;

    public List<ILazyItem> CachedItems { get; set; } = new();

    public WeiPoXItemsRepeater()
    {
        _repeater = new ItemsRepeater
        {
            ItemTemplate = new FuncDataTemplate<ActualLazyItem>(GenerateLayout)
        };
        Content = _repeater;
    }

    private Control GenerateLayout(ActualLazyItem arg1, INameScope arg2)
    {
        // TODO: nested DeclarativeView won't work with nested set state
        return new DeclarativeView(arg1.Builder.Invoke());
    }

    public void SetItems(List<ActualLazyItem> generateActualLazyItems)
    {
        _repeater.Items = generateActualLazyItems;
    }
}