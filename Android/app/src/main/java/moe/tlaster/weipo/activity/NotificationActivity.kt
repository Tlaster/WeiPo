package moe.tlaster.weipo.activity

import android.os.Bundle
import android.widget.RadioButton
import android.widget.RadioGroup
import androidx.recyclerview.widget.RecyclerView
import androidx.recyclerview.widget.StaggeredGridLayoutManager
import androidx.swiperefreshlayout.widget.SwipeRefreshLayout
import androidx.viewpager2.widget.ViewPager2
import com.google.android.material.tabs.TabLayoutMediator
import kotlinx.android.synthetic.main.activity_notification.*
import moe.tlaster.weipo.R
import moe.tlaster.weipo.common.AutoStaggeredGridLayoutManager
import moe.tlaster.weipo.common.adapter.AutoAdapter
import moe.tlaster.weipo.common.adapter.IItemSelector
import moe.tlaster.weipo.common.collection.IncrementalLoadingCollection
import moe.tlaster.weipo.common.extensions.bindLoadingCollection
import moe.tlaster.weipo.common.extensions.dp
import moe.tlaster.weipo.common.extensions.viewModel
import moe.tlaster.weipo.common.statusWidth
import moe.tlaster.weipo.viewmodel.INotificationTabItem
import moe.tlaster.weipo.viewmodel.MentionViewModel
import moe.tlaster.weipo.viewmodel.NotificationViewModel


class NotificationSelector: IItemSelector<INotificationTabItem<out Any>> {
    override fun selectLayout(item: INotificationTabItem<out Any>): Int {
        return when (item) {
            is MentionViewModel -> return R.layout.item_mention
            else -> R.layout.layout_list
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
        view_pager.adapter =
            AutoAdapter(NotificationSelector()).apply {
                items = viewModel.sources
                setView<SwipeRefreshLayout>(R.id.refresh_layout) { view, item, _, _ ->
                    item.adapter.items.let {
                        it as? IncrementalLoadingCollection<*, *>
                    }?.let {
                        view.bindLoadingCollection(it)
                    }
                }
                setView<RecyclerView>(R.id.recycler_view) { view, item, _, _ ->
                    view.layoutManager = AutoStaggeredGridLayoutManager(
                        statusWidth,
                        StaggeredGridLayoutManager.VERTICAL
                    )
                    view.adapter = item.adapter
                }
                setView<RadioGroup>(R.id.radio_selector) { view, item, _, _ ->
                    when (item) {
                        is MentionViewModel -> {
                            view.findViewById<RadioButton>(R.id.radio_mention)?.isChecked = true
                            view.setOnCheckedChangeListener { _, checkedId ->
                                when (checkedId) {
                                    R.id.radio_mention -> {
                                        item.isCmt = false
                                    }
                                    R.id.radio_mention_cmt -> {
                                        item.isCmt = true
                                    }
                                }
                                item.adapter.items.let {
                                    it as? IncrementalLoadingCollection<*, *>
                                }?.refresh()
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
                    tab.text = getString(it.title)
                    tab.setIcon(it.icon)
                }
            }).attach()
        view_pager.registerOnPageChangeCallback(object: ViewPager2.OnPageChangeCallback() {
            override fun onPageSelected(position: Int) {
                super.onPageSelected(position)
                //TODO:Badge
                viewModel.sources[position].let {
                    if (!it.adapter.items.any()) {
                        it.adapter.items.let {
                            it as? IncrementalLoadingCollection<*, *>
                        }?.refresh()
                    }
                }
            }
        })
    }

}
