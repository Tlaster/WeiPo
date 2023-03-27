using WeiPoX.Core.DeclarativeUI.Platform.UIKit.Internal;
using WeiPoX.Core.DeclarativeUI.Widgets;
using WeiPoX.Core.DeclarativeUI.Widgets.Layout;

namespace WeiPoX.Core.DeclarativeUI.Platform.UIKit.Renderer;

internal class LazyColumnRenderer : LazyRendererObject<LazyColumn, WeiPoXUICollectionView>
{
    protected override WeiPoXUICollectionView Create(WidgetBuilder renderer)
    {
        var layoutConfiguration = new UICollectionLayoutListConfiguration(UICollectionLayoutListAppearance.Plain)
        {
            ShowsSeparators = false,
            BackgroundColor = null,
        };
        var layout = UICollectionViewCompositionalLayout.GetLayout(layoutConfiguration);
        return new WeiPoXUICollectionView(CGRect.Empty, layout, renderer);
    }

    protected override void Update(WeiPoXUICollectionView control, LazyColumn widget)
    {
        control.SetItems(widget.GenerateActualLazyItems());
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
}

internal class WeiPoXUICollectionView : UICollectionView, IUICollectionViewDelegate, IUICollectionViewDataSource
{
    private readonly WidgetBuilder? _renderer;
    private const string _cellIdentifier = "cell";
    private List<ActualLazyItem> _actualLazyItems = new();
    public WeiPoXUICollectionView() : base(CGRect.Empty, UICollectionViewCompositionalLayout.GetLayout(new UICollectionLayoutListConfiguration(UICollectionLayoutListAppearance.Plain)))
    {
        
    }

    public WeiPoXUICollectionView(CGRect frame, UICollectionViewLayout layout, WidgetBuilder renderer) : base(frame, layout)
    {
        _renderer = renderer;
        RegisterClassForCell(typeof(WeiPoXUICollectionViewCell), _cellIdentifier);
        Delegate = this;
        DataSource = this;
    }
    
    public nint GetItemsCount(UICollectionView collectionView, nint section)
    {
        return _actualLazyItems.Count;
    }

    public UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
    {
        var cell = collectionView.DequeueReusableCell(_cellIdentifier, indexPath) as WeiPoXUICollectionViewCell;
        cell!.View.Renderer = _renderer;
        cell!.View.Builder = (index) => _actualLazyItems[index];
        cell!.View.SetIndex(indexPath.Row);
        return cell;
    }
    
    public void SetItems(List<ActualLazyItem> generateActualLazyItems)
    {
        if (generateActualLazyItems.Count != _actualLazyItems.Count)
        {
            _actualLazyItems = generateActualLazyItems;
            ReloadData();
        }
        else
        {
            _actualLazyItems = generateActualLazyItems;
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

    public SubDeclarativeView View { get; } = new();
}