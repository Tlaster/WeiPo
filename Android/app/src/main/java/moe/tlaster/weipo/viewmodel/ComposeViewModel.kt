package moe.tlaster.weipo.viewmodel

import androidx.lifecycle.ViewModel
import moe.tlaster.weipo.common.ComposeQueue
import moe.tlaster.weipo.common.collection.ObservableCollection
import moe.tlaster.weipo.services.models.ICanReply
import java.io.File

class ComposeViewModel(
    private val composeType: ComposeType,
    private val reply: ICanReply?
) : ViewModel() {

    enum class ComposeType {
        Create,
        Repost,
        Comment;
        companion object {
            private val map = values().associateBy(ComposeType::ordinal)
            fun fromInt(type: Int) = map[type]
        }

    }

    val maxLength: Int
    get() {
        return when (composeType) {
            ComposeType.Create -> 1000
            ComposeType.Repost -> 140
            ComposeType.Comment -> 140
        }
    }
    val images = ObservableCollection<File>()
    var content: String = ""

    fun commit() {
        ComposeQueue.commit(content, composeType, reply, images.toTypedArray())
    }
}