package moe.tlaster.weipo.fragment

import android.os.Bundle
import android.view.View
import android.widget.RadioButton
import android.widget.RadioGroup
import androidx.fragment.app.Fragment
import androidx.fragment.app.activityViewModels
import androidx.lifecycle.MutableLiveData
import androidx.lifecycle.Observer
import androidx.recyclerview.widget.RecyclerView
import androidx.recyclerview.widget.StaggeredGridLayoutManager
import androidx.swiperefreshlayout.widget.SwipeRefreshLayout
import androidx.viewpager2.widget.ViewPager2
import com.google.android.material.tabs.TabLayoutMediator
import kotlinx.android.synthetic.main.fragment_notification.*
import moe.tlaster.weipo.R
import moe.tlaster.weipo.common.AutoStaggeredGridLayoutManager
import moe.tlaster.weipo.common.adapter.AutoAdapter
import moe.tlaster.weipo.common.adapter.IItemSelector
import moe.tlaster.weipo.common.collection.IncrementalLoadingCollection
import moe.tlaster.weipo.common.extensions.bindLoadingCollection
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

class NotificationFragment : Fragment(R.layout.fragment_notification), ITabFragment {
    private lateinit var requestRefresh: () -> Unit
    val totalNotificationCount = MutableLiveData<Int>()

    private val viewModel by activityViewModels<NotificationViewModel>()

    override fun onViewCreated(view: View, savedInstanceState: Bundle?) {
        super.onViewCreated(view, savedInstanceState)
        viewModel.unread.observe(this.viewLifecycleOwner, Observer { args ->
            var result = 0
            val mention = ((args.mentionCmt ?: 0) + (args.mentionStatus ?: 0)).toInt()
            result += mention
            if (mention > 0) {
                tab_layout.getTabAt(0)?.orCreateBadge?.number = mention
            } else {
                tab_layout.getTabAt(0)?.removeBadge()
            }
            val cmt = args.cmt?.toInt() ?: 0
            result += cmt
            if (cmt > 0) {
                tab_layout.getTabAt(1)?.orCreateBadge?.number = cmt
            } else {
                tab_layout.getTabAt(1)?.removeBadge()
            }
            val attitude = args.attitude?.toInt() ?: 0
            result += attitude
            if (attitude > 0) {
                tab_layout.getTabAt(2)?.orCreateBadge?.number = attitude
            } else {
                tab_layout.getTabAt(2)?.removeBadge()
            }
            val dm = args.dm?.toInt() ?: 0
            result += dm
            if (dm > 0) {
                tab_layout.getTabAt(3)?.orCreateBadge?.number = dm
            } else {
                tab_layout.getTabAt(3)?.removeBadge()
            }
            totalNotificationCount.value = result
        })
        view_pager.adapter =
            AutoAdapter(NotificationSelector()).apply {
                items = viewModel.sources
                setView<SwipeRefreshLayout>(R.id.refresh_layout) { view, item, position, _ ->
                    item.adapter.items.let {
                        it as? IncrementalLoadingCollection<*, *>
                    }?.let {
                        view.bindLoadingCollection(it)
                        it.stateChanged += { _, args ->
                            when (args) {
                                IncrementalLoadingCollection.CollectionState.Loading -> {
                                    tab_layout.getTabAt(position)?.let { tab ->
                                        totalNotificationCount.value = totalNotificationCount.value?.minus(
                                            (tab.badge?.number ?: 0)
                                        ) ?: 0
                                        tab.removeBadge()
                                    }
                                }
                                IncrementalLoadingCollection.CollectionState.Completed -> {

                                }
                            }
                        }
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
//                    tab.setIcon(it.icon)
                }
            }).attach()
        view_pager.registerOnPageChangeCallback(object: ViewPager2.OnPageChangeCallback() {
            override fun onPageSelected(position: Int) {
                super.onPageSelected(position)
                tab_layout.getTabAt(position)?.badge?.takeIf {
                    it.hasNumber() && it.number > 0
                }?.let {
                    viewModel.sources[position]
                }?.let {
                    it.adapter.items as? IncrementalLoadingCollection<*, *>
                }?.refresh()
            }
        })

        requestRefresh = {
            viewModel.sources[view_pager.currentItem].adapter.items.let {
                it as? IncrementalLoadingCollection<*, *>
            }?.refresh()
        }
    }

    override fun onTabReselected() {
        requestRefresh.invoke()
    }
}
