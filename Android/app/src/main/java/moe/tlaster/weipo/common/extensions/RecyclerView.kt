package moe.tlaster.weipo.common.extensions

import androidx.recyclerview.widget.RecyclerView
import moe.tlaster.weipo.common.adapter.AutoAdapter

fun <T> RecyclerView.updateItemsSource(items: List<T>) {
    adapter?.let {
        it as? AutoAdapter<T>
    }?.let {
        it.items = items
    }
}