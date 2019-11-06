package moe.tlaster.weipo.activity

import android.annotation.SuppressLint
import android.os.Bundle
import androidx.recyclerview.widget.StaggeredGridLayoutManager
import kotlinx.android.synthetic.main.activity_timeline.*
import moe.tlaster.weipo.R
import moe.tlaster.weipo.common.AutoStaggeredGridLayoutManager
import moe.tlaster.weipo.common.adapter.IncrementalLoadingAdapter
import moe.tlaster.weipo.common.adapter.ItemSelector
import moe.tlaster.weipo.common.extensions.bindLoadingCollection
import moe.tlaster.weipo.common.extensions.dp
import moe.tlaster.weipo.common.extensions.openActivity
import moe.tlaster.weipo.common.extensions.viewModel
import moe.tlaster.weipo.controls.StatusView
import moe.tlaster.weipo.services.models.Status
import moe.tlaster.weipo.viewmodel.ComposeViewModel
import moe.tlaster.weipo.viewmodel.TimelineViewModel

class TimelineActivity : BaseActivity() {
    override val layoutId: Int
        get() = R.layout.activity_timeline

    private val viewModel by lazy {
        viewModel<TimelineViewModel>()
    }

    private val adapter by lazy {
        IncrementalLoadingAdapter<Status>(ItemSelector(R.layout.item_status)).apply {
            items = viewModel.items
            setView<StatusView>(R.id.item_status) { view, item, _, _ ->
                view.data = item
            }
        }
    }
    @SuppressLint("RestrictedApi")
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        recycler_view.layoutManager = AutoStaggeredGridLayoutManager(360.dp.toInt(), StaggeredGridLayoutManager.VERTICAL)
        recycler_view.adapter = adapter
        refresh_layout.bindLoadingCollection(viewModel.items)
        account_button.setOnClickListener {
            viewModel.config?.let {
                openActivity<UserActivity>(
                    "user_id" to it.uid?.toLongOrNull()
                )
            }
        }
        notification_button.setOnClickListener {
            openActivity<NotificationActivity>()
        }
        compose_button.setOnClickListener {
            openActivity<ComposeActivity>(*ComposeActivity.bundle(ComposeViewModel.ComposeType.Create))
        }
    }
}
