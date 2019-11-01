package moe.tlaster.weipo.fragment.user

import android.os.Bundle
import android.view.View
import androidx.recyclerview.widget.StaggeredGridLayoutManager
import kotlinx.android.synthetic.main.layout_list.*
import moe.tlaster.weipo.R
import moe.tlaster.weipo.common.AutoStaggeredGridLayoutManager
import moe.tlaster.weipo.common.adapter.IncrementalLoadingAdapter
import moe.tlaster.weipo.common.adapter.ItemSelector
import moe.tlaster.weipo.common.extensions.bindLoadingCollection
import moe.tlaster.weipo.common.extensions.dp
import moe.tlaster.weipo.common.extensions.factory
import moe.tlaster.weipo.common.extensions.viewModel
import moe.tlaster.weipo.controls.StatusView
import moe.tlaster.weipo.viewmodel.user.WeiboListViewModel

class WeiboTabFragment(
    userId: Long, containerId: String
) : UserTabFragment(R.layout.layout_list, userId, containerId) {

    private val viewModel by lazy {
        viewModel<WeiboListViewModel>(factory {
            WeiboListViewModel(userId, containerId)
        })
    }

    val adapter by lazy {
        IncrementalLoadingAdapter<Any>(ItemSelector(R.layout.item_status)).apply {
            items = viewModel.source
            setView<StatusView>(R.id.item_status) { view, item, _, _ ->
                view.data = item
            }
        }
    }

    override fun onViewCreated(view: View, savedInstanceState: Bundle?) {
        super.onViewCreated(view, savedInstanceState)
        recycler_view.layoutManager = AutoStaggeredGridLayoutManager(360.dp.toInt(), StaggeredGridLayoutManager.VERTICAL)
        recycler_view.adapter = adapter
        refresh_layout.bindLoadingCollection(viewModel.source)
    }
}