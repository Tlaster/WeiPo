package moe.tlaster.weipo.fragment

import android.os.Bundle
import android.view.View
import androidx.fragment.app.activityViewModels
import kotlinx.android.synthetic.main.layout_list.*
import moe.tlaster.weipo.R
import moe.tlaster.weipo.common.AutoStaggeredGridLayoutManager
import moe.tlaster.weipo.common.adapter.IncrementalLoadingAdapter
import moe.tlaster.weipo.common.adapter.ItemSelector
import moe.tlaster.weipo.common.extensions.bindLoadingCollection
import moe.tlaster.weipo.common.statusWidth
import moe.tlaster.weipo.controls.StatusView
import moe.tlaster.weipo.services.models.Status
import moe.tlaster.weipo.viewmodel.TimelineViewModel

interface ITabFragment {
    fun onTabReselected()
}

class TimelineFragment : BaseFragment(R.layout.layout_list), ITabFragment {

    private lateinit var requestRefresh: () -> Unit
    private val viewModel by activityViewModels<TimelineViewModel>()

    private val adapter by lazy {
        IncrementalLoadingAdapter<Status>(ItemSelector(R.layout.item_status)).apply {
            items = viewModel.items
            setView<StatusView>(R.id.item_status) { view, item, _, _ ->
                view.data = item
            }
        }
    }

    override fun onViewCreated(view: View, savedInstanceState: Bundle?) {
        super.onViewCreated(view, savedInstanceState)
        recycler_view.layoutManager = AutoStaggeredGridLayoutManager(statusWidth)
        recycler_view.adapter = adapter
        refresh_layout.bindLoadingCollection(viewModel.items, viewLifecycleOwner)
        requestRefresh = {
            viewModel.items.refresh()
        }
    }

    override fun onTabReselected() {
        requestRefresh.invoke()
    }
}