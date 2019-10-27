package moe.tlaster.weipo.fragment

import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.recyclerview.widget.StaggeredGridLayoutManager
import kotlinx.android.synthetic.main.fragment_timeline.*
import kotlinx.android.synthetic.main.fragment_timeline.recycler_view
import kotlinx.coroutines.GlobalScope
import kotlinx.coroutines.launch
import moe.tlaster.weipo.R
import moe.tlaster.weipo.common.adapter.IncrementalLoadingAdapter
import moe.tlaster.weipo.common.adapter.ItemSelector
import moe.tlaster.weipo.common.extensions.dp
import moe.tlaster.weipo.common.extensions.viewModel
import moe.tlaster.weipo.controls.StatusView
import moe.tlaster.weipo.services.models.Status
import moe.tlaster.weipo.viewmodel.TimelineViewModel
import kotlin.math.round

class TimelineFragment : BaseFragment(R.layout.fragment_timeline) {

    private var spanCount: Int = -1
    private val LIST_STATE_KEY = "LIST_STATE_KEY"
    private val viewModel by lazy {
        viewModel<TimelineViewModel>()
    }

    private val adapter by lazy {
        IncrementalLoadingAdapter<Status>(ItemSelector(R.layout.item_status)).apply {
            items = viewModel.items
            setView<StatusView>(R.id.item_status) { view, item, _, _ ->
                view.status = item
            }
        }
    }

    override fun onViewCreated(view: View, savedInstanceState: Bundle?) {
        super.onViewCreated(view, savedInstanceState)
        if (spanCount == -1) {
            recycler_view.post {
                spanCount = round(recycler_view.width.toDouble() / 360.dp.toDouble()).toInt()
                recycler_view.layoutManager = StaggeredGridLayoutManager(spanCount, StaggeredGridLayoutManager.VERTICAL)
            }
        } else {
            recycler_view.layoutManager = StaggeredGridLayoutManager(spanCount, StaggeredGridLayoutManager.VERTICAL)
        }
        recycler_view.adapter = adapter
        refresh_layout.setOnRefreshListener {
            GlobalScope.launch {
                viewModel.items.refreshAsync()
                refresh_layout.isRefreshing = false
            }
        }
    }
}
