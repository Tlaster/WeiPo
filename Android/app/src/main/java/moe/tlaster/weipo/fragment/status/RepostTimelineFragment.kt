package moe.tlaster.weipo.fragment.status

import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.fragment.app.viewModels
import kotlinx.android.synthetic.main.layout_list.*
import moe.tlaster.weipo.R
import moe.tlaster.weipo.common.AutoStaggeredGridLayoutManager
import moe.tlaster.weipo.common.adapter.IncrementalLoadingAdapter
import moe.tlaster.weipo.common.adapter.ItemSelector
import moe.tlaster.weipo.common.extensions.bindLoadingCollection
import moe.tlaster.weipo.common.extensions.factory
import moe.tlaster.weipo.common.statusWidth
import moe.tlaster.weipo.controls.StatusView
import moe.tlaster.weipo.fragment.TabFragment
import moe.tlaster.weipo.services.models.Status
import moe.tlaster.weipo.viewmodel.status.RepostTimelineViewModel

class RepostTimelineFragment : TabFragment() {
    private var statusId: Long = 0

    override val titleRes: Int
        get() = R.string.repost

    private val viewModel by viewModels<RepostTimelineViewModel> {
        factory {
            RepostTimelineViewModel(statusId)
        }
    }

    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        (savedInstanceState ?: arguments)?.let { bundle ->
            bundle.getLong("id").takeIf {
                it != 0L
            }?.let {
                statusId = it
            }
        }
        return inflater.inflate(R.layout.layout_list, container)
    }

    override fun onViewCreated(view: View, savedInstanceState: Bundle?) {
        super.onViewCreated(view, savedInstanceState)
        recycler_view.layoutManager = AutoStaggeredGridLayoutManager(statusWidth)
        recycler_view.adapter = IncrementalLoadingAdapter<Status>(ItemSelector(R.layout.item_status)).apply {
            setView<StatusView>(R.id.item_status) { view, item, _, _ ->
                view.isTextSelectionEnabled = false
                view.showRepost = false
                view.data = item
            }
            items = viewModel.source
        }
        refresh_layout.bindLoadingCollection(viewModel.source, this)
    }

    override fun onSaveInstanceState(outState: Bundle) {
        super.onSaveInstanceState(outState)
        outState.putLong("id", statusId)
    }
}