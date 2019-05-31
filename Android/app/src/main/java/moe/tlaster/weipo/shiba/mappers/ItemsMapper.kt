package moe.tlaster.weipo.shiba.mappers

import android.view.ViewGroup
import androidx.recyclerview.widget.GridLayoutManager
import androidx.recyclerview.widget.LinearLayoutManager
import androidx.recyclerview.widget.RecyclerView
import moe.tlaster.shiba.IShibaContext
import moe.tlaster.shiba.ShibaHost
import moe.tlaster.shiba.mapper.PropertyMap
import moe.tlaster.shiba.mapper.ViewMapper
import moe.tlaster.shiba.type.CollectionChangedEventArg
import moe.tlaster.shiba.type.ShibaArray

class ItemsMapper : ViewMapper<RecyclerView>() {
    override fun getViewLayoutParams(): ViewGroup.LayoutParams {
        return ViewGroup.MarginLayoutParams(ViewGroup.LayoutParams.MATCH_PARENT, ViewGroup.LayoutParams.WRAP_CONTENT)
    }

    override fun createNativeView(context: IShibaContext): RecyclerView {
        return RecyclerView(context.getContext()).apply {
            layoutManager = LinearLayoutManager(context.getContext(), RecyclerView.VERTICAL, false)
            adapter = ShibaRecyclerAdapter()
        }
    }

    override fun propertyMaps(): ArrayList<PropertyMap> {
        return super.propertyMaps().apply {
            add(PropertyMap("source", { view, value ->
                if (view is RecyclerView && value is List<*>) {
                    val adapter = view.adapter as? ShibaRecyclerAdapter
                    adapter?.items?.addAll(value)
                }
            }))
            add(PropertyMap("layout", { view, value ->
                if (view is RecyclerView && value is String) {
                    when (value) {
                        "nineGrid" -> view.layoutManager = GridLayoutManager(view.context, 3)
                    }
                }
            }))
            add(PropertyMap("creator", { view, value ->
                if (view is RecyclerView && value is String) {
                    view.adapter?.let { it as? ShibaRecyclerAdapter }?.creator = value
                }
            }))
        }
    }

}


private class ShibaViewHolder(itemView: ShibaHost) : RecyclerView.ViewHolder(itemView)

private class ShibaRecyclerAdapter : RecyclerView.Adapter<ShibaViewHolder>() {
    var creator: String? = null
        set(value) {
            field = value
            notifyDataSetChanged()//relayout
        }

    private val onItemsChanged: (Any, CollectionChangedEventArg) -> Unit = { _, _ ->
        notifyDataSetChanged()
    }
    var items = ShibaArray().apply {
        collectionChanged += this@ShibaRecyclerAdapter.onItemsChanged
    }
        set(value) {
            field = value
            notifyDataSetChanged()
        }

    override fun onCreateViewHolder(parent: ViewGroup, position: Int): ShibaViewHolder {
        return ShibaViewHolder(ShibaHost(parent.context).apply {
            this.creator = this@ShibaRecyclerAdapter.creator
//            component = layout
        })
    }

    override fun getItemCount() = items.count()

    override fun onBindViewHolder(viewHolder: ShibaViewHolder, position: Int) {
        (viewHolder.itemView as? ShibaHost)?.dataContext = items[position]
    }
}