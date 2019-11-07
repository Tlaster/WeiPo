package moe.tlaster.weipo.common

import android.content.Context
import moe.tlaster.weipo.activity.UserActivity
import moe.tlaster.weipo.common.extensions.openActivity
import moe.tlaster.weipo.common.extensions.openBrowser


fun openWeiboLink(context: Context, value: String) {
    when {
        value.startsWith("/n/") -> {
            context.openActivity<UserActivity>(
                "user_name" to value.substring("/n/".length)
            )
        }
        value.startsWith("/status/") -> {

        }
        value.startsWith("http") -> {
            context.openBrowser(value)
        }
    }
}