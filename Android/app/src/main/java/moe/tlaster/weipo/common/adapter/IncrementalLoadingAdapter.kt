package moe.tlaster.weipo.common.adapter

import kotlinx.coroutines.launch
import moe.tlaster.weipo.common.collection.*

class IncrementalLoadingAdapter<T>(layout: IItemSelector<T>) : AutoAdapter<T>(layout) {

    private val onCollectionChanged: (Any, CollectionChangedEventArg) -> Unit = { _, args ->
        when (args.type) {
            CollectionChangedType.Add -> notifyItemRangeInserted(args.index, args.count)
            CollectionChangedType.Remove -> notifyItemRangeRemoved(args.index, args.count)
            CollectionChangedType.Update -> notifyItemRangeChanged(args.index, args.count)
            CollectionChangedType.Reset -> notifyDataSetChanged()
        }
    }

    var autoRefresh = true

    override var items = listOf<T>()
        set(value) {
            field = value

            val current = field
            if (current is INotifyCollectionChanged) {
                current.collectionChanged -= onCollectionChanged
            }
            field = value
            if (value is INotifyCollectionChanged) {
                value.collectionChanged += onCollectionChanged
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
}