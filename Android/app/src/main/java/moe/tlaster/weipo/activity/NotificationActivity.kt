package moe.tlaster.weipo.activity

import android.os.Bundle
import android.widget.RadioButton
import android.widget.RadioGroup
import androidx.recyclerview.widget.RecyclerView
import androidx.recyclerview.widget.StaggeredGridLayoutManager
import androidx.swiperefreshlayout.widget.SwipeRefreshLayout
import com.google.android.material.tabs.TabLayoutMediator
import kotlinx.android.synthetic.main.activity_notification.*
import kotlinx.coroutines.GlobalScope
import kotlinx.coroutines.launch
import moe.tlaster.weipo.R
import moe.tlaster.weipo.common.AutoStaggeredGridLayoutManager
import moe.tlaster.weipo.common.adapter.AutoAdapter
import moe.tlaster.weipo.common.adapter.IItemSelector
import moe.tlaster.weipo.common.collection.IncrementalLoadingCollection
import moe.tlaster.weipo.common.extensions.dp
import moe.tlaster.weipo.common.extensions.viewModel
import moe.tlaster.weipo.viewmodel.ITabItem
import moe.tlaster.weipo.viewmodel.MentionViewModel
import moe.tlaster.weipo.viewmodel.NotificationItemViewModel
import moe.tlaster.weipo.viewmodel.NotificationViewModel


class NotificationSelector: IItemSelector<ITabItem> {
    override fun selectLayout(item: ITabItem): Int {
        return when (item) {
            is MentionViewModel -> return R.layout.item_mention
            else -> R.layout.item_notification
        }
    }
}

class NotificationActivity : BaseActivity() {

    private val viewModel by lazy {
        viewModel<NotificationViewModel>()
    }

    override val layoutId: Int
        get() = R.layout.activity_notification

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        view_pager.offscreenPageLimit = 1
        view_pager.adapter =
            AutoAdapter(NotificationSelector()).apply {
                items = viewModel.sources
                setView<SwipeRefreshLayout>(R.id.refresh_layout) { view, item, _, _ ->
                    when (item) {
                        is NotificationItemViewModel<*> -> view.setOnRefreshListener {
                            GlobalScope.launch {
                                item.adapter.items.let {
                                    it as? IncrementalLoadingCollection<*, *>
                                }?.refreshAsync()
                                view.isRefreshing = false
                            }
                        }
                        is MentionViewModel -> {
                            view.setOnRefreshListener {
                                GlobalScope.launch {
                                    item.source.refreshAsync()
                                    view.isRefreshing = false
                                }
                            }
                        }
                    }
                }
                setView<RecyclerView>(R.id.recycler_view) { view, item, _, _ ->
                    view.layoutManager = AutoStaggeredGridLayoutManager(
                        360.dp.toInt(),
                        StaggeredGridLayoutManager.VERTICAL
                    )
                    when (item) {
                        is NotificationItemViewModel<*> -> {
                            view.adapter = item.adapter
                        }
                        is MentionViewModel -> {
                            view.adapter = item.adapter
                        }
                    }
                }
                setView<RadioGroup>(R.id.radio_selector) { view, item, position, adapter ->
                    when (item) {
                        is MentionViewModel -> {
                            view.findViewById<RadioButton>(R.id.radio_mention)?.isChecked = true
                            view.setOnCheckedChangeListener { _, checkedId ->
                                when (checkedId) {
                                    R.id.radio_mention -> {

                                    }
                                    R.id.radio_mention_cmt -> {

                                    }
                                }
                            }
                        }
                    }
                }

            }
        TabLayoutMediator(
            tab_layout,
            view_pager,
            TabLayoutMediator.TabConfigurationStrategy { tab, position ->
                viewModel.sources[position].let {
                    tab.text = it.title
                    tab.setIcon(it.icon)
                }
            }).attach()
    }

}
