package moe.tlaster.weipo.fragment.notification

import android.os.Bundle
import android.view.View
import androidx.fragment.app.Fragment
import androidx.fragment.app.activityViewModels
import kotlinx.android.synthetic.main.fragment_mention.*
import moe.tlaster.weipo.R
import moe.tlaster.weipo.common.AutoStaggeredGridLayoutManager
import moe.tlaster.weipo.common.adapter.IncrementalLoadingAdapter
import moe.tlaster.weipo.common.adapter.ItemSelector
import moe.tlaster.weipo.common.extensions.bindLoadingCollection
import moe.tlaster.weipo.common.statusWidth
import moe.tlaster.weipo.controls.StatusView
import moe.tlaster.weipo.viewmodel.notification.MentionViewModel

class MentionFragment : Fragment(R.layout.fragment_mention) {
    private val viewModel by activityViewModels<MentionViewModel>()
    override fun onViewCreated(view: View, savedInstanceState: Bundle?) {
        super.onViewCreated(view, savedInstanceState)
        radio_selector.setOnCheckedChangeListener { _, checkedId ->
            when (checkedId) {
                R.id.radio_mention -> {
                    viewModel.isCmt = false
                }
                R.id.radio_mention_cmt -> {
                    viewModel.isCmt = true
                }
            }
            viewModel.source.refresh()
        }
        refresh_layout.bindLoadingCollection(viewModel.source)
        recycler_view.adapter = IncrementalLoadingAdapter(
            ItemSelector<Any>(R.layout.item_status)
        ).apply {
            items = viewModel.source
            setView<StatusView>(R.id.item_status) { view, item, _, _ ->
                view.data = item
            }
        }
        recycler_view.layoutManager = AutoStaggeredGridLayoutManager(statusWidth)
    }
}


