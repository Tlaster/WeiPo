using Android.Content;
using Android.Views;
using AndroidX.RecyclerView.Widget;
using WeiPoX.Core.DeclarativeUI.Internal;
using WeiPoX.Core.DeclarativeUI.Platform.Android.Internal;
using WeiPoX.Core.DeclarativeUI.Widgets;
using WeiPoX.Core.DeclarativeUI.Widgets.Layout;

namespace WeiPoX.Core.DeclarativeUI.Platform.Android.Renderer;

internal class LazyColumnRenderer : LazyRendererObject<LazyColumn, RecyclerView>
{
    public LazyColumnRenderer(Context context) : base(context)
    {
    }

    protected override RecyclerView Create(Context context, RendererContext<View> rendererContext)
    {
        var view = new RecyclerView(context);
        view.SetLayoutManager(new LinearLayoutManager(context));
        view.SetAdapter(new WeiPoXRecyclerViewAdapter(rendererContext));
        return view;
    }

    protected override void Update(RecyclerView control, LazyColumn widget)
    {
        if (control.GetAdapter() is WeiPoXRecyclerViewAdapter adapter)
        {
            adapter.SetItems(widget);
        }
    }

    protected override bool IsVisible(RecyclerView control, int index)
    {
        var layoutManager = control.GetLayoutManager();
        if (layoutManager is LinearLayoutManager linearLayoutManager)
        {
            var firstVisibleItemPosition = linearLayoutManager.FindFirstVisibleItemPosition();
            var lastVisibleItemPosition = linearLayoutManager.FindLastVisibleItemPosition();
            return index >= firstVisibleItemPosition && index <= lastVisibleItemPosition;
        }
        return false;
    }

    protected override View? GetVisibleChild(RecyclerView control, int index)
    {
        var layoutManager = control.GetLayoutManager();
        if (layoutManager is LinearLayoutManager linearLayoutManager)
        {
            var firstVisibleItemPosition = linearLayoutManager.FindFirstVisibleItemPosition();
            var lastVisibleItemPosition = linearLayoutManager.FindLastVisibleItemPosition();
            if (index >= firstVisibleItemPosition && index <= lastVisibleItemPosition)
            {
                var childAt = linearLayoutManager.FindViewByPosition(index);
                if (childAt is DeclarativeView view)
                {
                    return view.GetChildAt(0);
                }
            }
        }
        return null;
    }

    protected override void UpdateChild(RecyclerView control, int index, View childControl)
    {
        var layoutManager = control.GetLayoutManager();
        if (layoutManager is LinearLayoutManager linearLayoutManager)
        {
            var firstVisibleItemPosition = linearLayoutManager.FindFirstVisibleItemPosition();
            var lastVisibleItemPosition = linearLayoutManager.FindLastVisibleItemPosition();
            if (index >= firstVisibleItemPosition && index <= lastVisibleItemPosition)
            {
                var childAt = linearLayoutManager.FindViewByPosition(index);
                if (childAt is DeclarativeView view)
                {
                    view.UpdateChild(childControl);
                }
            }
        }
    }

    protected override Range GetVisibleRange(RecyclerView control)
    {
        // return if recycler view is empty
        if (control.GetAdapter()?.ItemCount == 0)
        {
            return new Range(0, 0);
        }

        // workaround for recycler view not showing items when first load
        if (control.ChildCount == 0)
        {
            return new Range(0, 0);
        }
        var layoutManager = control.GetLayoutManager();
        if (layoutManager is LinearLayoutManager linearLayoutManager)
        {
            var firstVisibleItemPosition = linearLayoutManager.FindFirstVisibleItemPosition();
            var lastVisibleItemPosition = linearLayoutManager.FindLastVisibleItemPosition();
            return new Range(Math.Max(0, firstVisibleItemPosition), Math.Max(0, lastVisibleItemPosition));
        }
        return new Range(0, 0);
    }
}

internal class WeiPoXRecyclerViewAdapter : RecyclerView.Adapter
{
    private readonly RendererContext<View> _rendererContext;
    private ILazyWidget? _lazyWidget;
    
    public WeiPoXRecyclerViewAdapter(RendererContext<View> rendererContext)
    {
        _rendererContext = rendererContext;
    }

    public override int ItemCount => _lazyWidget?.Count ?? 0;

    public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
    {
        if (holder is WeiPoXViewHolder { ItemView: DeclarativeView view } && _lazyWidget != null)
        {
            view.Widget = _lazyWidget.GetBuilder(position)?.Invoke();
        }
    }

    public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
    {
        return new WeiPoXViewHolder(
            new DeclarativeView(
                parent.Context!,
                _rendererContext.BuildOwner
            )
        );
    }
    
    
    public void SetItems(ILazyWidget widget)
    {
        if (_lazyWidget == null || widget.Count != _lazyWidget.Count)
        {
            _lazyWidget = widget;
            NotifyDataSetChanged();
        }
        else
        {
            _lazyWidget = widget;
        }
    }
}

internal class WeiPoXViewHolder : RecyclerView.ViewHolder
{
    public WeiPoXViewHolder(View itemView) : base(itemView)
    {
    }
}