using WeiPoX.Core.DeclarativeUI.Internal;
using WeiPoX.Core.DeclarativeUI.Platform.UIKit.Internal;
using WeiPoX.Core.DeclarativeUI.Widgets;
using WeiPoX.Core.DeclarativeUI.Widgets.Layout;

namespace WeiPoX.Core.DeclarativeUI.Platform.UIKit.Renderer;

internal class LazyColumnRenderer : LazyRendererObject<LazyColumn, WeiPoXUICollectionView>
{
    protected override WeiPoXUICollectionView Create(RendererContext<UIView> context)
    {
        var layoutConfiguration = new UICollectionLayoutListConfiguration(UICollectionLayoutListAppearance.Plain)
        {
            ShowsSeparators = false,
            BackgroundColor = null,
        };
        var layout = UICollectionViewCompositionalLayout.GetLayout(layoutConfiguration);
        return new WeiPoXUICollectionView(CGRect.Empty, layout, context);
    }

    protected override void Update(WeiPoXUICollectionView control, LazyColumn widget)
    {
        control.SetItems(widget);
    }

    protected override bool IsVisible(WeiPoXUICollectionView control, int index)
    { 
        var firstVisibleItemPosition = control.IndexPathsForVisibleItems.MinBy(x => x.Row)?.Row ?? 0;
        var lastVisibleItemPosition = control.IndexPathsForVisibleItems.MaxBy(x => x.Row)?.Row ?? 0;
        return index >= firstVisibleItemPosition && index <= lastVisibleItemPosition;
    }

    protected override UIView? GetVisibleChild(WeiPoXUICollectionView control, int index)
    {
        var firstVisibleItemPosition = control.IndexPathsForVisibleItems.MinBy(x => x.Row)?.Row ?? 0;
        var lastVisibleItemPosition = control.IndexPathsForVisibleItems.MaxBy(x => x.Row)?.Row ?? 0;
        if (index >= firstVisibleItemPosition && index <= lastVisibleItemPosition)
        {
            var cell = control.CellForItem(NSIndexPath.FromRowSection(index, 0));
            if (cell is WeiPoXUICollectionViewCell view)
            {
                return view.View.Subviews[0];
            }
        }
        return null;
    }

    protected override void UpdateChild(WeiPoXUICollectionView control, int index, UIView childControl)
    {
        var cell = control.CellForItem(NSIndexPath.FromRowSection(index, 0));
        if (cell is WeiPoXUICollectionViewCell view)
        {
            view.View.UpdateChild(childControl);
        }
    }

    protected override Range GetVisibleRange(WeiPoXUICollectionView control)
    {
        var firstVisibleItemPosition = control.IndexPathsForVisibleItems.MinBy(x => x.Row)?.Row ?? 0;
        var lastVisibleItemPosition = control.IndexPathsForVisibleItems.MaxBy(x => x.Row)?.Row ?? 0;
        return new Range(firstVisibleItemPosition, lastVisibleItemPosition);
    }
}

internal class WeiPoXUICollectionView : UICollectionView, IUICollectionViewDelegate, IUICollectionViewDataSource
{
    private readonly RendererContext<UIView>? _context;
    private const string CellIdentifier = "cell";
    private ILazyWidget? _lazyWidget;
    
    public WeiPoXUICollectionView() : base(CGRect.Empty, UICollectionViewCompositionalLayout.GetLayout(new UICollectionLayoutListConfiguration(UICollectionLayoutListAppearance.Plain)))
    {
        
    }

    public WeiPoXUICollectionView(CGRect frame, UICollectionViewLayout layout, RendererContext<UIView> context) : base(frame, layout)
    {
        _context = context;
        RegisterClassForCell(typeof(WeiPoXUICollectionViewCell), CellIdentifier);
        Delegate = this;
        DataSource = this;
    }
    
    public nint GetItemsCount(UICollectionView collectionView, nint section)
    {
        return _lazyWidget?.Count ?? 0;
    }

    public UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
    {
        var cell = collectionView.DequeueReusableCell(CellIdentifier, indexPath) as WeiPoXUICollectionViewCell;
        cell!.View.Init(_context?.BuildOwner);
        cell!.View.Widget = _lazyWidget?.GetBuilder(indexPath.Row)?.Invoke();
        return cell;
    }
    
    public void SetItems(ILazyWidget widget)
    {
        if (_lazyWidget == null || widget.Count != _lazyWidget.Count)
        {
            _lazyWidget = widget;
            ReloadData();
        }
        else
        {
            _lazyWidget = widget;
        }
    }
}

internal class WeiPoXUICollectionViewCell : UICollectionViewCell
{
    
    [Export ("initWithFrame:")]
    public WeiPoXUICollectionViewCell(CGRect frame) : base(frame)
    {
        ContentView.AddSubview(View);
        View.TranslatesAutoresizingMaskIntoConstraints = false;
        View.TopAnchor.ConstraintEqualTo(ContentView.TopAnchor).Active = true;
        View.LeadingAnchor.ConstraintEqualTo(ContentView.LeadingAnchor).Active = true;
        View.TrailingAnchor.ConstraintEqualTo(ContentView.TrailingAnchor).Active = true;
        View.BottomAnchor.ConstraintEqualTo(ContentView.BottomAnchor).Active = true;

    }

    public DeclarativeView View { get; } = new();
}