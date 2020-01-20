package moe.tlaster.weipo.activity

import android.os.Bundle
import androidx.activity.viewModels
import kotlinx.android.synthetic.main.activity_hotflow_child.*
import moe.tlaster.weipo.R
import moe.tlaster.weipo.common.AutoStaggeredGridLayoutManager
import moe.tlaster.weipo.common.adapter.IncrementalLoadingAdapter
import moe.tlaster.weipo.common.adapter.ItemSelector
import moe.tlaster.weipo.common.extensions.bindLoadingCollection
import moe.tlaster.weipo.common.extensions.factory
import moe.tlaster.weipo.common.statusWidth
import moe.tlaster.weipo.controls.StatusView
import moe.tlaster.weipo.services.models.Comment
import moe.tlaster.weipo.viewmodel.status.HotflowChildViewModel

class HotflowChildActivity : BaseActivity() {

    private val comment by lazy {
        intent.getParcelableExtra<Comment>("item")
    }

    private val viewModel by viewModels<HotflowChildViewModel> {
        factory {
            HotflowChildViewModel(comment)
        }
    }
    override val layoutId: Int
        get() = R.layout.activity_hotflow_child

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setSupportActionBar(toolbar)
        item_status.apply {
            data = comment
            showRepost = false
            showHotflow = false
            isTextSelectionEnabled = true
        }
        recycler_view.layoutManager = AutoStaggeredGridLayoutManager(statusWidth)
        recycler_view.adapter = IncrementalLoadingAdapter<Comment>(ItemSelector(R.layout.item_status)).apply {
            items = viewModel.source
            setView<StatusView>(R.id.item_status) { view, item, _, _ ->
                view.isTextSelectionEnabled = false
                view.showRepost = false
                view.data = item
            }
        }
        refresh_layout.bindLoadingCollection(viewModel.source, this)
    }
}
