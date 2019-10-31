package moe.tlaster.weipo.common.adapter

import kotlinx.coroutines.GlobalScope
import kotlinx.coroutines.launch
import moe.tlaster.weipo.common.collection.CollectionChangedEventArg
import moe.tlaster.weipo.common.collection.CollectionChangedType
import moe.tlaster.weipo.common.collection.INotifyCollectionChanged
import moe.tlaster.weipo.common.collection.ISupportIncrementalLoading
import moe.tlaster.weipo.common.extensions.runOnMainThread

class IncrementalLoadingAdapter<T>(layout: IItemSelector<T>) : AutoAdapter<T>(layout) {

    private val onCollectionChanged: (Any, CollectionChangedEventArg) -> Unit = { _, args ->
        runOnMainThread {
            when (args.type) {
                CollectionChangedType.Add -> notifyItemRangeInserted(args.index, args.count)
                CollectionChangedType.Remove -> notifyItemRangeRemoved(args.index, args.count)
                CollectionChangedType.Update -> notifyItemRangeChanged(args.index, args.count)
                CollectionChangedType.Reset -> notifyItemRangeRemoved(args.index, args.count)
            }
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
            if (value is ISupportIncrementalLoading && !value.any() && autoRefresh) {
                GlobalScope.launch {
                    value.loadMoreItemsAsync()
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
            GlobalScope.launch {
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