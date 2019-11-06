package moe.tlaster.weipo.common

import kotlinx.coroutines.GlobalScope
import kotlinx.coroutines.launch
import moe.tlaster.weipo.appContext
import moe.tlaster.weipo.common.extensions.runOnMainThread
import moe.tlaster.weipo.common.extensions.toast
import moe.tlaster.weipo.services.Api
import moe.tlaster.weipo.services.models.Comment
import moe.tlaster.weipo.services.models.ICanReply
import moe.tlaster.weipo.viewmodel.ComposeViewModel
import java.io.File

object ComposeQueue {
    fun commit(
        content: String,
        composeType: ComposeViewModel.ComposeType,
        reply: ICanReply?,
        images: Array<File>
    ) {
        GlobalScope.launch {
            val picIds = images.mapNotNull { Api.uploadPic(it).picId }
            val result = when (composeType) {
                ComposeViewModel.ComposeType.Create -> {
                    Api.update(content, *picIds.toTypedArray())
                }
                ComposeViewModel.ComposeType.Repost -> {
                    Api.repost(content, reply!!, picIds.firstOrNull())
                }
                ComposeViewModel.ComposeType.Comment -> {
                    if (reply is Comment) {
                        Api.reply(content, reply, picIds.firstOrNull())
                    } else {
                        Api.comment(content, reply!!, picIds.firstOrNull())
                    }
                }
            }
            runOnMainThread {
                appContext.toast("Send success")
            }
        }
    }

}