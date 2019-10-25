package moe.tlaster.weipo.common.adapter

import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.ImageView
import android.widget.TextView
import androidx.annotation.IdRes
import androidx.collection.ArrayMap
import androidx.recyclerview.widget.RecyclerView
import moe.tlaster.weipo.common.extensions.load

class ViewHolder(itemView: View) : RecyclerView.ViewHolder(itemView)

interface IItemSelector<T> {
    fun selectLayout(item: T): Int
}

open class ItemSelector<T>(private val layoutId: Int) : IItemSelector<T> {
    override fun selectLayout(item: T): Int {
        return layoutId
    }
}

open class AutoAdapter<T>(
    private val itemSelector: IItemSelector<T>
) : RecyclerView.Adapter<ViewHolder>() {

    var itemClick: ((view: View, item: T, position: Int, adapter: AutoAdapter<T>) -> Unit)? = null
    private val customActions: ArrayMap<Int, ((view: View, item: T, position: Int, adapter: AutoAdapter<T>) -> Unit)> =
        ArrayMap()

    open var items = listOf<T>()
        set(value) {
            field = value
            notifyDataSetChanged()
        }

    override fun getItemViewType(position: Int): Int {
        val item = getItem(position)
        return itemSelector.selectLayout(item)
    }

    override fun onCreateViewHolder(parent: ViewGroup, viewType: Int): ViewHolder {
        return ViewHolder(
            LayoutInflater.from(parent.context).inflate(
                viewType,
                parent,
                false
            )
        )
    }

    override fun getItemCount(): Int {
        return items.count()
    }

    override fun onBindViewHolder(holder: ViewHolder, position: Int) {
        val item = getItem(position)
        itemClick?.let { clickListener ->
            holder.itemView.setOnClickListener {
                clickListener.invoke(it, item, position, this)
            }
        }
        customActions.forEach { action ->
            holder.itemView.findViewById<View>(action.key)?.let {
                action.value.invoke(it, item, position, this)
            }
        }
    }

    protected open fun getItem(position: Int): T {
        return items[position]
    }

    fun setText(@IdRes id: Int, action: (T) -> String) {
        customActions[id] = { view, item, _, _ ->
            view.let {
                it as? TextView
            }?.let {
                it.text = action.invoke(item)
            }
        }
    }

    fun setImage(@IdRes id: Int, action: (T) -> String) {
        customActions[id] = { view, item, _, _ ->
            view.let {
                it as? ImageView
            }?.load(action.invoke(item))
        }
    }

    fun <K : View> setView(@IdRes id: Int, action: (view: K, item: T, position: Int, adapter: AutoAdapter<T>) -> Unit) {
        customActions[id] = { view, item, index, adapter ->
            view.let {
                it as? K
            }?.let {
                action.invoke(it, item, index, adapter)
            }
        }
    }
}