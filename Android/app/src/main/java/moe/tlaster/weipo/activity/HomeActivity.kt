package moe.tlaster.weipo.activity

import android.os.Bundle
import androidx.activity.viewModels
import androidx.lifecycle.Observer
import androidx.navigation.findNavController
import androidx.navigation.ui.setupWithNavController
import kotlinx.android.synthetic.main.activity_home.*
import moe.tlaster.weipo.R
import moe.tlaster.weipo.common.extensions.openActivity
import moe.tlaster.weipo.viewmodel.ComposeViewModel
import moe.tlaster.weipo.viewmodel.NotificationViewModel

class HomeActivity : BaseActivity() {

    private val notificationViewModel by viewModels<NotificationViewModel>()

    override val layoutId: Int
        get() = R.layout.activity_home

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        val navController = findNavController(R.id.nav_host_fragment)
        bottom_navigation_view?.setupWithNavController(navController)
        navigation_view?.setupWithNavController(navController)
        floating_button.setOnClickListener {
            it?.context?.openActivity<ComposeActivity>("compose_type" to ComposeViewModel.ComposeType.Create)
        }
        navController.addOnDestinationChangedListener { _, destination, _ ->
            if (destination.id == R.id.navigation_timeline) {
                floating_button.show()
            } else {
                floating_button.hide()
            }
        }
        notificationViewModel.unread.observe(this, Observer {
            bottom_navigation_view?.let { bottomNavigationView ->
                val total = (it.mentionStatus ?: 0) + (it.mentionCmt ?: 0) + (it.cmt ?: 0) + (it.attitude ?: 0) + (it.dm ?: 0)
                if (total > 0) {
                    bottomNavigationView.getOrCreateBadge(R.id.navigation_notification).number = total.toInt()
                } else {
                    bottomNavigationView.removeBadge(R.id.navigation_notification)
                }
            }
            navigation_view?.let { navigationView ->
                //TODO: badge
            }
        })
    }
}
