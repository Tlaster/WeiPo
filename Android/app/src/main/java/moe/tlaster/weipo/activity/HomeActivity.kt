package moe.tlaster.weipo.activity

import android.os.Bundle
import com.google.android.material.tabs.TabLayout
import com.google.android.material.tabs.TabLayoutMediator
import kotlinx.android.synthetic.main.activity_home.*
import moe.tlaster.weipo.R
import moe.tlaster.weipo.common.adapter.FragmentAdapter
import moe.tlaster.weipo.common.extensions.openActivity
import moe.tlaster.weipo.fragment.NotificationFragment
import moe.tlaster.weipo.fragment.TimelineFragment
import moe.tlaster.weipo.fragment.UserFragment
import moe.tlaster.weipo.viewmodel.ComposeViewModel

class HomeActivity : BaseActivity() {
    private val timelineFragment by lazy {
        TimelineFragment()
    }
    private val notificationFragment by lazy {
        NotificationFragment().apply {
            badgeUpdated += onNotificationBadgeUpdated
        }
    }
    private val userFragment by lazy {
        UserFragment()
    }

    override val layoutId: Int
        get() = R.layout.activity_home


    private val onNotificationBadgeUpdated: (Any, Int) -> Unit = { sender, args ->
        tab_layout.getTabAt(1)?.let {
            if (args == 0) {
                it.removeBadge()
            } else {
                it.orCreateBadge.number = args
            }
        }
    }

    private val tabIcon by lazy {
        listOf(
            R.drawable.ic_home_black_24dp,
            R.drawable.ic_notifications_black_24dp,
            R.drawable.ic_person_black_24dp
        )
    }

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)

        view_pager.offscreenPageLimit = 3
        view_pager.adapter = FragmentAdapter(supportFragmentManager, lifecycle, listOf(
            timelineFragment,
            notificationFragment,
            userFragment
        ))

        view_pager.isUserInputEnabled = false

        compose_button.setOnClickListener {
            openActivity<ComposeActivity>(*ComposeActivity.bundle(ComposeViewModel.ComposeType.Create))
        }

        TabLayoutMediator(
            tab_layout,
            view_pager,
            TabLayoutMediator.TabConfigurationStrategy { tab, position ->
                tab.setIcon(tabIcon[position])
            }).attach()

        tab_layout.addOnTabSelectedListener(object : TabLayout.OnTabSelectedListener {
            override fun onTabReselected(tab: TabLayout.Tab?) {
                if (tab != null) {
                    when (tab.position) {
                        0 -> timelineFragment.requestRefresh()
                        1 -> notificationFragment.requestRefresh()
                    }
                }
            }

            override fun onTabUnselected(tab: TabLayout.Tab?) {
            }

            override fun onTabSelected(tab: TabLayout.Tab?) {
            }

        })
    }
}
