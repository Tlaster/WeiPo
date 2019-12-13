package moe.tlaster.weipo.common.adapter

import androidx.lifecycle.Observer
import androidx.recyclerview.widget.RecyclerView
import kotlinx.coroutines.launch
import moe.tlaster.weipo.common.collection.*

class IncrementalLoadingAdapter<T>(
    layout: IItemSelector<T>
) : AutoAdapter<T>(layout), Observer<CollectionChangedEventArg> {
    var autoRefresh = true

    override var items = listOf<T>()
        set(value) {
            field = value

            val current = field
            if (current is INotifyCollectionChanged) {
                current.collectionChanged.removeObserver(this)
            }
            field = value
            if (value is INotifyCollectionChanged) {
                value.collectionChanged.observeForever(this)
            }
            if (!value.any()) {
                if (value is ISupportIncrementalLoading && autoRefresh) {
                    value.scope.launch {
                        value.loadMoreItemsAsync()
                    }
                } else if (value is ISupportCacheLoading) {
                    value.scope.launch {
                        value.loadCachedAsync()
                    }
                }
            }
            notifyDataSetChanged()
        }

    override fun onDetachedFromRecyclerView(recyclerView: RecyclerView) {
        super.onDetachedFromRecyclerView(recyclerView)
        items.let {
            it as? INotifyCollectionChanged
        }?.let {
            it.collectionChanged.removeObserver(this)
        }
    }

    private var isLoading = false
    override fun getItem(position: Int): T {
        val currentItems = items
        if (isNearEnd(position, currentItems.size) &&
            currentItems is ISupportIncrementalLoading &&
            currentItems.hasMoreItems &&
            !isLoading
        ) {
            currentItems.scope.launch {
                isLoading = true
                currentItems.loadMoreItemsAsync()
                isLoading = false
            }
        }
        return currentItems[position]
    }

    private fun isNearEnd(index: Int, count: Int): Boolean {
        return index == count - 1
    }

    override fun onChanged(args: CollectionChangedEventArg) {
        when (args.type) {
            CollectionChangedType.Add -> notifyItemRangeInserted(args.index, args.count)
            CollectionChangedType.Remove -> notifyItemRangeRemoved(args.index, args.count)
            CollectionChangedType.Update -> notifyItemRangeChanged(args.index, args.count)
            CollectionChangedType.Reset -> notifyDataSetChanged()
        }
    }
}