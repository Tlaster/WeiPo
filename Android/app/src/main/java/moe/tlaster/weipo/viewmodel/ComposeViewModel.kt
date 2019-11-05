package moe.tlaster.weipo.viewmodel

import androidx.lifecycle.ViewModel
import kotlinx.coroutines.GlobalScope
import kotlinx.coroutines.launch
import moe.tlaster.weipo.common.collection.ObservableCollection
import moe.tlaster.weipo.services.Api
import java.io.File

class ComposeViewModel(
    private val composeType: ComposeType
) : ViewModel() {

    enum class ComposeType {
        Create,
        Repost,
        Comment
    }

    val maxLength: Int
    get() {
        return 1000
    }
    val images = ObservableCollection<File>()
    val content: String = ""

    fun commit() {
        GlobalScope.launch {
            val picIds = images.mapNotNull { Api.uploadPic(it).picId }
            val result = when (composeType) {
                ComposeType.Create -> {
                    Api.update(content, *picIds.toTypedArray())
                }
                ComposeType.Repost -> TODO()
                ComposeType.Comment -> TODO()
            }

        }
    }
}