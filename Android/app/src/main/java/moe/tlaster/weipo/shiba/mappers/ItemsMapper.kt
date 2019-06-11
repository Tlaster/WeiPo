package moe.tlaster.weipo.shiba.mappers

import android.view.View
import android.view.ViewGroup
import androidx.recyclerview.widget.LinearLayoutManager
import androidx.recyclerview.widget.RecyclerView
import kotlinx.coroutines.*
import moe.tlaster.shiba.IShibaContext
import moe.tlaster.shiba.Shiba
import moe.tlaster.shiba.ShibaHost
import moe.tlaster.shiba.mapper.PropertyMap
import moe.tlaster.shiba.mapper.ViewMapper
import moe.tlaster.shiba.type.CollectionChangedEventArg
import moe.tlaster.weipo.shiba.collection.IIncrementalSource
import moe.tlaster.weipo.shiba.collection.INotifyCollectionChanged
import moe.tlaster.weipo.shiba.collection.ISupportIncrementalLoading
import moe.tlaster.weipo.shiba.collection.IncrementalLoadingCollection

class NineGridLayoutManager : RecyclerView.LayoutManager() {
    private var itemSize: Double = 0.0

    override fun generateDefaultLayoutParams(): RecyclerView.LayoutParams {
        return RecyclerView.LayoutParams(ViewGroup.LayoutParams.MATCH_PARENT, ViewGroup.LayoutParams.WRAP_CONTENT)
    }

    override fun isAutoMeasureEnabled(): Boolean {
        return false
    }

    override fun onMeasure(
        recycler: RecyclerView.Recycler,
        state: RecyclerView.State,
        widthSpec: Int,
        heightSpec: Int
    ) {
        super.onMeasure(recycler, state, widthSpec, heightSpec)
        if (itemCount == 0 || state.isPreLayout) {
            return
        }
        val width = View.MeasureSpec.getSize(widthSpec)

        itemSize = width.toDouble() / 3.toDouble()
        val totalHeight = Math.ceil(itemCount.toDouble() / 3.toDouble()) * itemSize

        val actualHeightSpec = View.MeasureSpec.makeMeasureSpec(totalHeight.toInt(), View.MeasureSpec.EXACTLY)

        super.onMeasure(recycler, state, widthSpec, actualHeightSpec)
    }

    override fun onLayoutChildren(recycler: RecyclerView.Recycler?, state: RecyclerView.State?) {
        if (recycler == null || state == null || itemCount <= 0 || state.isPreLayout) {
            return
        }

        if (state.itemCount == 0) {
            removeAndRecycleAllViews(recycler)
            return
        }
        detachAndScrapAttachedViews(recycler)
        var currentY = 0.toDouble()
        var currentX = 0.toDouble()
        val size = itemSize
        val itemWidth = View.MeasureSpec.makeMeasureSpec(size.toInt(), View.MeasureSpec.EXACTLY)
        for (i in 0 until itemCount) {

            val view = recycler.getViewForPosition(i)
            addView(view)
            view.measure(itemWidth, itemWidth)
            layoutDecorated(view, currentX.toInt(), currentY.toInt(), currentX.toInt() + size.toInt(), currentY.toInt() + size.toInt())
            currentX += size
            if (currentX >= size * 3) {
                currentX = 0.0
                currentY += size
            }
        }
    }

}

class ItemsMapper : ViewMapper<RecyclerView>() {
    override fun getViewLayoutParams(): ViewGroup.LayoutParams {
        return ViewGroup.MarginLayoutParams(ViewGroup.LayoutParams.MATCH_PARENT, ViewGroup.LayoutParams.WRAP_CONTENT)
    }

    override fun createNativeView(context: IShibaContext): RecyclerView {
        return RecyclerView(context.getContext()).apply {
            layoutManager = LinearLayoutManager(context.getContext(), RecyclerView.VERTICAL, false)
            adapter = ShibaRecyclerAdapter<Any?>()
        }
    }

    override fun propertyMaps(): ArrayList<PropertyMap> {
        return super.propertyMaps().apply {
            add(PropertyMap("source", { view, value ->
                if (view is RecyclerView) {
                    when (value) {
                        is String -> {
                            view.adapter?.let {
                                it as? ShibaRecyclerAdapter<Any?>
                            }?.let { adapter ->
                                Shiba.configuration.scriptRuntime.callFunction(name = value)?.let {
                                    adapter.items = IncrementalLoadingCollection(ShibaDataSource(it)).also {
                                        GlobalScope.launch {
                                            it.refresh()
                                        }
                                    }
                                }
                            }
                        }
                        is List<*> -> {
                            view.adapter?.let {
                                it as? ShibaRecyclerAdapter<Any?>
                            }?.let {
                                it.items = value
                            }
                        }
                    }
                }
            }))
            add(PropertyMap("layout", { view, value ->
                if (view is RecyclerView && value is String) {
                    when (value) {
                        "nineGrid" -> view.layoutManager = NineGridLayoutManager()
                    }
                }
            }))
            add(PropertyMap("creator", { view, value ->
                if (view is RecyclerView && value is String) {
                    view.adapter?.let {
                        it as? ShibaRecyclerAdapter<Any?>
                    }?.let {
                        it.creator = value
                    }
                }
            }))
        }
    }
}

private class ShibaDataSource(
    val creatorInstance: Any
) : IIncrementalSource<Any> {
    private val functionName = "getPagedItemsAsync"
    override suspend fun getPagedItemAsync(page: Int, count: Int): List<Any> {
        val scriptResult = Shiba.configuration.scriptRuntime.callFunction(creatorInstance, functionName, page)
        if (scriptResult is Deferred<*>) {
            val result = scriptResult.await()
            if (Shiba.configuration.scriptRuntime.isArray(result)) {
                return Shiba.configuration.scriptRuntime.toArray(result)
            }
        }
        return emptyList()
    }

}

private class ShibaViewHolder(itemView: ShibaHost) : RecyclerView.ViewHolder(itemView)

private class ShibaRecyclerAdapter<T> : RecyclerView.Adapter<ShibaViewHolder>() {
    var creator: String? = null
        set(value) {
            field = value
            notifyDataSetChanged()//relayout
        }

    private val onItemsChanged: (Any, CollectionChangedEventArg) -> Unit = { _, _ ->
        GlobalScope.launch {
            withContext(Dispatchers.Main) {
                notifyDataSetChanged()
            }
        }
    }
    var items: List<T> = emptyList()
        set(value) {
            field = value
            if (value is INotifyCollectionChanged) {
                value.collectionChanged += onItemsChanged
            }
            notifyDataSetChanged()
        }

    override fun onCreateViewHolder(parent: ViewGroup, position: Int): ShibaViewHolder {
        return ShibaViewHolder(ShibaHost(parent.context).apply {
            this.creator = this@ShibaRecyclerAdapter.creator
        })
    }

    override fun getItemCount(): Int {
        return items.count()
    }

    override fun onBindViewHolder(viewHolder: ShibaViewHolder, position: Int) {
        (viewHolder.itemView as? ShibaHost)?.dataContext = getItemAt(position)
    }

    private fun getItemAt(position: Int): Any? {
        val currentItems = items
        if (isNearEnd(position, currentItems.size) &&
            currentItems is ISupportIncrementalLoading &&
                currentItems.hasMoreItems) {
            GlobalScope.launch {
                currentItems.loadMoreItemsAsync()
            }
        }
        return currentItems[position]
    }

    protected open fun isNearEnd(index: Int, count: Int): Boolean {
        return index == count - 1
    }
}