package moe.tlaster.weipo.fragment.notification

import android.os.Bundle
import android.view.View
import androidx.fragment.app.activityViewModels
import androidx.lifecycle.Observer
import kotlinx.android.synthetic.main.fragment_mention.*
import moe.tlaster.weipo.R
import moe.tlaster.weipo.common.AutoStaggeredGridLayoutManager
import moe.tlaster.weipo.common.adapter.IncrementalLoadingAdapter
import moe.tlaster.weipo.common.adapter.ItemSelector
import moe.tlaster.weipo.common.collection.IncrementalLoadingCollection
import moe.tlaster.weipo.common.extensions.bindLoadingCollection
import moe.tlaster.weipo.common.statusWidth
import moe.tlaster.weipo.controls.StatusView
import moe.tlaster.weipo.fragment.BaseFragment
import moe.tlaster.weipo.viewmodel.NotificationViewModel
import moe.tlaster.weipo.viewmodel.notification.MentionViewModel

class MentionFragment : BaseFragment(R.layout.fragment_mention) {
    private val viewModel by activityViewModels<MentionViewModel>()
    private val notificationViewModel by activityViewModels<NotificationViewModel>()
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
        viewModel.source.stateChanged.observe(viewLifecycleOwner, Observer {
            if (it == IncrementalLoadingCollection.CollectionState.Refreshing) {
                if (viewModel.isCmt) {
                    notificationViewModel.update {
                        it.mentionCmt = 0L
                    }
                } else {
                    notificationViewModel.update {
                        it.mentionStatus = 0L
                    }
                }
            }
        })
        refresh_layout.bindLoadingCollection(viewModel.source, viewLifecycleOwner)
        recycler_view.adapter = IncrementalLoadingAdapter(
            ItemSelector<Any>(R.layout.item_status)
        ).apply {
            items = viewModel.source
            autoRefresh = false
            setView<StatusView>(R.id.item_status) { view, item, _, _ ->
                view.data = item
            }
        }
        recycler_view.layoutManager = AutoStaggeredGridLayoutManager(statusWidth)
    }
}


