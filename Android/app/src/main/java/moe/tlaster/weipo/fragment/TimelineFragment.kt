package moe.tlaster.weipo.fragment

import android.os.Bundle
import android.view.View
import kotlinx.android.synthetic.main.fragment_timeline.*
import moe.tlaster.weipo.R
import moe.tlaster.weipo.common.adapter.IncrementalLoadingAdapter
import moe.tlaster.weipo.common.adapter.ItemSelector
import moe.tlaster.weipo.common.extensions.viewModel
import moe.tlaster.weipo.controls.StatusView
import moe.tlaster.weipo.services.models.Status
import moe.tlaster.weipo.viewmodel.TimelineViewModel

class TimelineFragment : BaseFragment(R.layout.fragment_timeline) {

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
        recycler_view.adapter = adapter
    }
}
