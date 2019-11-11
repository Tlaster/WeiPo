package moe.tlaster.weipo.activity

import android.annotation.SuppressLint
import android.os.Bundle
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
        NotificationFragment()
    }
    private val userFragment by lazy {
        UserFragment()
    }

    override val layoutId: Int
        get() = R.layout.activity_home
    @SuppressLint("RestrictedApi")
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)

        view_pager.offscreenPageLimit = 3
        view_pager.adapter = FragmentAdapter(supportFragmentManager, lifecycle, listOf(
            timelineFragment,
            notificationFragment,
            userFragment
        ))

        view_pager.isUserInputEnabled = false

        home_button.setOnClickListener {
            if (view_pager.currentItem == 0) {
                timelineFragment.requestRefresh()
            } else {
                view_pager.setCurrentItem(0, false)
            }
        }
        account_button.setOnClickListener {
//            viewModel.config?.let {
//                openActivity<UserActivity>(
//                    "user_id" to it.uid?.toLongOrNull()
//                )
//            }
            view_pager.setCurrentItem(2, false)
        }
        notification_button.setOnClickListener {
//            openActivity<NotificationActivity>()
            view_pager.setCurrentItem(1, false)
        }
        compose_button.setOnClickListener {
            openActivity<ComposeActivity>(*ComposeActivity.bundle(ComposeViewModel.ComposeType.Create))
        }
    }
}
