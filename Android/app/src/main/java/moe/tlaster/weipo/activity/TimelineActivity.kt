package moe.tlaster.weipo.activity

import android.annotation.SuppressLint
import android.os.Bundle
import androidx.recyclerview.widget.StaggeredGridLayoutManager
import com.google.android.material.badge.BadgeDrawable
import com.google.android.material.badge.BadgeUtils
import kotlinx.android.synthetic.main.activity_timeline.*
import kotlinx.coroutines.GlobalScope
import kotlinx.coroutines.launch
import moe.tlaster.weipo.R
import moe.tlaster.weipo.common.adapter.IncrementalLoadingAdapter
import moe.tlaster.weipo.common.adapter.ItemSelector
import moe.tlaster.weipo.common.extensions.dp
import moe.tlaster.weipo.common.extensions.openActivity
import moe.tlaster.weipo.common.extensions.viewModel
import moe.tlaster.weipo.controls.StatusView
import moe.tlaster.weipo.services.models.Status
import moe.tlaster.weipo.viewmodel.TimelineViewModel
import kotlin.math.round

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
                view.status = item
            }
        }
    }
    @SuppressLint("RestrictedApi")
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        val recyclerWidth = savedInstanceState?.getInt("recyclerWidth", -1) ?: -1
        val recyclerSpanCount = savedInstanceState?.getInt("recyclerSpanCount", -1) ?: -1
        if (recyclerSpanCount == -1) {
            recycler_view.post {
                val spanCount = round(recycler_view.width.toDouble() / 480.dp.toDouble()).toInt()
                recycler_view.layoutManager = StaggeredGridLayoutManager(spanCount, StaggeredGridLayoutManager.VERTICAL)
            }
        } else {
            recycler_view.layoutManager = StaggeredGridLayoutManager(recyclerSpanCount, StaggeredGridLayoutManager.VERTICAL)
            recycler_view.post {
                if (recycler_view.width != recyclerWidth) {
                    val spanCount = round(recycler_view.width.toDouble() / 480.dp.toDouble()).toInt()
                    recycler_view.layoutManager = StaggeredGridLayoutManager(spanCount, StaggeredGridLayoutManager.VERTICAL)
                }
            }
        }
        recycler_view.adapter = adapter
        refresh_layout.setOnRefreshListener {
            GlobalScope.launch {
                viewModel.items.refreshAsync()
                refresh_layout.isRefreshing = false
            }
        }
        notification_button.setOnClickListener {
            openActivity<NotificationActivity>()
        }
    }

    override fun onSaveInstanceState(outState: Bundle) {
        super.onSaveInstanceState(outState)
        outState.putInt("recyclerWidth", recycler_view.width)
        recycler_view.layoutManager?.let {
            it as StaggeredGridLayoutManager
        }?.let {
            outState.putInt("recyclerSpanCount", it.spanCount)
        }
    }
}
