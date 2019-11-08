package moe.tlaster.weipo.common

import android.content.Context
import android.net.Uri
import moe.tlaster.weipo.activity.ImageActivity
import moe.tlaster.weipo.activity.ImageData
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
            val uri = Uri.parse(value)
            if (uri.host?.contains("sinaimg.cn") == true) {
                context.openActivity<ImageActivity>(*ImageActivity.bundle(listOf(
                    ImageData(value, value)
                )))
            } else {
                context.openBrowser(value)
            }
        }
    }
}