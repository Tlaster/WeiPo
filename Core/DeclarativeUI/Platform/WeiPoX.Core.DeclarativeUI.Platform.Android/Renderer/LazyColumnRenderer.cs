﻿using Android.Content;
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
        var view = new RecyclerView(context)
        {
        };
        view.SetLayoutManager(new LinearLayoutManager(context));
        view.SetAdapter(new WeiPoXRecyclerViewAdapter(rendererContext));
        return view;
    }

    protected override void Update(RecyclerView control, LazyColumn widget)
    {
        if (control.GetAdapter() is WeiPoXRecyclerViewAdapter adapter)
        {
            adapter.SetItems(widget.GenerateActualLazyItems());
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
}

internal class WeiPoXRecyclerViewAdapter : RecyclerView.Adapter
{
    private readonly RendererContext<View> _rendererContext;
    private List<ActualLazyItem> _actualLazyItems = new();
    
    public WeiPoXRecyclerViewAdapter(RendererContext<View> rendererContext)
    {
        _rendererContext = rendererContext;
    }

    public override int ItemCount => _actualLazyItems.Count;

    public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
    {
        if (holder is WeiPoXViewHolder { ItemView: DeclarativeView view })
        {
            view.Widget = _actualLazyItems[position].Builder();
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
    
    
    public void SetItems(List<ActualLazyItem> generateActualLazyItems)
    {
        if (generateActualLazyItems.Count != _actualLazyItems.Count)
        {
            _actualLazyItems = generateActualLazyItems;
            NotifyDataSetChanged();
        }
        else
        {
            _actualLazyItems = generateActualLazyItems;
        }
    }
}

internal class WeiPoXViewHolder : RecyclerView.ViewHolder
{
    public WeiPoXViewHolder(DeclarativeView itemView) : base(itemView)
    {
    }
}