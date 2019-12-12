package moe.tlaster.weipo.fragment

import android.os.Bundle
import android.view.View
import androidx.fragment.app.Fragment
import androidx.fragment.app.activityViewModels
import androidx.lifecycle.Observer
import com.google.android.material.tabs.TabLayoutMediator
import kotlinx.android.synthetic.main.fragment_notification.*
import moe.tlaster.weipo.R
import moe.tlaster.weipo.common.adapter.FragmentAdapter
import moe.tlaster.weipo.fragment.notification.AttitudeFragment
import moe.tlaster.weipo.fragment.notification.CommentFragment
import moe.tlaster.weipo.fragment.notification.DirectMessageFragment
import moe.tlaster.weipo.fragment.notification.MentionFragment
import moe.tlaster.weipo.viewmodel.NotificationViewModel


class NotificationFragment : Fragment(R.layout.fragment_notification), ITabFragment {
    private lateinit var requestRefresh: () -> Unit

    private val viewModel by activityViewModels<NotificationViewModel>()

    override fun onViewCreated(view: View, savedInstanceState: Bundle?) {
        super.onViewCreated(view, savedInstanceState)
        viewModel.unread.observe(viewLifecycleOwner, Observer {
            val mention = (it.mentionStatus ?: 0) + (it.mentionCmt ?: 0)
            if (mention > 0L) {
                tab_layout.getTabAt(0)?.orCreateBadge?.number = mention.toInt()
            } else {
                tab_layout.getTabAt(0)?.removeBadge()
            }
            val comment = it.cmt ?: 0
            if (comment > 0L) {
                tab_layout.getTabAt(1)?.orCreateBadge?.number = comment.toInt()
            } else {
                tab_layout.getTabAt(1)?.removeBadge()
            }
            val attitude = it.attitude ?: 0
            if (attitude > 0L) {
                tab_layout.getTabAt(2)?.orCreateBadge?.number = attitude.toInt()
            } else {
                tab_layout?.getTabAt(2)?.removeBadge()
            }
            val dm = it.dm ?: 0
            if (dm > 0L) {
                tab_layout.getTabAt(3)?.orCreateBadge?.number = dm.toInt()
            } else {
                tab_layout.getTabAt(3)?.removeBadge()
            }
        })
        view_pager.adapter = FragmentAdapter(this, listOf(
            MentionFragment(),
            CommentFragment(),
            AttitudeFragment(),
            DirectMessageFragment()
        ))

        TabLayoutMediator(
            tab_layout,
            view_pager,
            TabLayoutMediator.TabConfigurationStrategy { tab, position ->
                tab.setText(when (position) {
                    0 -> R.string.mention
                    1 -> R.string.comment
                    2 -> R.string.attitude
                    3 -> R.string.direct_message
                    else -> throw IllegalArgumentException()
                })
            }).attach()
    }

    override fun onTabReselected() {
        requestRefresh.invoke()
    }
}
