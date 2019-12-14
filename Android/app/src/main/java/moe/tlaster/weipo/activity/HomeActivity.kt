package moe.tlaster.weipo.activity

import android.app.PendingIntent
import android.content.Intent
import android.os.Bundle
import androidx.activity.viewModels
import androidx.core.app.NotificationCompat
import androidx.core.app.NotificationManagerCompat
import androidx.lifecycle.Observer
import androidx.navigation.findNavController
import androidx.navigation.ui.NavigationUI
import androidx.navigation.ui.setupWithNavController
import kotlinx.android.synthetic.main.activity_home.*
import moe.tlaster.weipo.R
import moe.tlaster.weipo.common.extensions.openActivity
import moe.tlaster.weipo.services.models.UnreadData
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
                val total =
                    (it.mentionStatus ?: 0) + (it.mentionCmt ?: 0) + (it.cmt ?: 0) + (it.attitude
                        ?: 0) + (it.dm ?: 0)
                if (total > 0) {
                    bottomNavigationView.getOrCreateBadge(R.id.navigation_notification).number =
                        total.toInt()
                } else {
                    bottomNavigationView.removeBadge(R.id.navigation_notification)
                }
            }
            navigation_view?.let { navigationView ->
                //TODO: badge
            }
            sendNotification(it)
        })
    }

    private fun sendNotification(it: UnreadData) {
        if ((it.mentionStatus ?: 0) > 0) {
            buildAndSendNotification(
                getString(R.string.notification_mention_status).format((it.mentionStatus ?: 0)),
                R.integer.notification_mention_status
            )
        } else {
            cancelNotification(R.integer.notification_mention_status)
        }
        if ((it.mentionCmt ?: 0) > 0) {
            buildAndSendNotification(
                getString(R.string.notification_mention_cmt).format((it.mentionCmt ?: 0)),
                R.integer.notification_mention_cmt
            )
        } else {
            cancelNotification(R.integer.notification_mention_cmt)
        }
        if ((it.cmt ?: 0) > 0) {
            buildAndSendNotification(
                getString(R.string.notification_cmt).format((it.cmt ?: 0)),
                R.integer.notification_cmt
            )
        } else {
            cancelNotification(R.integer.notification_cmt)
        }
        if ((it.dm ?: 0) > 0) {
            buildAndSendNotification(
                getString(R.string.notification_dm).format((it.dm ?: 0)),
                R.integer.notification_dm
            )
        } else {
            cancelNotification(R.integer.notification_dm)
        }
        if ((it.follower ?: 0) > 0) {
            buildAndSendNotification(
                getString(R.string.notification_follower).format((it.follower ?: 0)),
                R.integer.notification_follower
            )
        } else {
            cancelNotification(R.integer.notification_follower)
        }
    }

    private fun cancelNotification(notificationId: Int) {
        with(NotificationManagerCompat.from(this)) {
            cancel(notificationId)
        }
    }

    private fun buildAndSendNotification(title: String, notificationId: Int) {
        val builder = NotificationCompat.Builder(this, getString(R.string.channel_id))
            .setAutoCancel(true)
            .setSmallIcon(R.drawable.ic_notification_icon)
            .setContentIntent(
                PendingIntent.getActivity(
                    this,
                    0,
                    Intent(this, HomeActivity::class.java),
                    0
                )
            )
            .setContentTitle(title)
        with(NotificationManagerCompat.from(this)) {
            notify(notificationId, builder.build())
        }
    }
}
