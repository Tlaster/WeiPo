package moe.tlaster.weipo.viewmodel.notification

import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import kotlinx.coroutines.launch
import moe.tlaster.weipo.common.collection.IIncrementalSource
import moe.tlaster.weipo.common.collection.IncrementalLoadingCollection
import moe.tlaster.weipo.datasource.CachedFuncDataSource
import moe.tlaster.weipo.datasource.FuncDataSource
import moe.tlaster.weipo.services.Api
import moe.tlaster.weipo.services.models.Attitude
import moe.tlaster.weipo.services.models.Comment
import moe.tlaster.weipo.services.models.Config
import moe.tlaster.weipo.services.models.MessageList

class MentionViewModel : ViewModel() {
    var isCmt = false
    val source = IncrementalLoadingCollection(FuncDataSource {
        if (isCmt) {
            Api.mentionsCmt(it + 1)
        } else {
            Api.mentionsAt(it + 1)
        }
    }, scope = viewModelScope)
}

abstract class NotificationItemViewModel<T>(dataSource: IIncrementalSource<T>) : ViewModel() {
    val source = IncrementalLoadingCollection(dataSource, scope = viewModelScope)
}

class CommentViewModel :
    NotificationItemViewModel<Comment>(CachedFuncDataSource("comment_list", Comment.serializer()) {
        Api.comment(it + 1)
    })

class AttitudeViewModel : NotificationItemViewModel<Attitude>(
    CachedFuncDataSource(
        "attitude_list",
        Attitude.serializer()
    ) {
        Api.attitude(it + 1)
    })

class DirectMessageViewModel : NotificationItemViewModel<MessageList>(
    CachedFuncDataSource(
        "direct_message_list",
        MessageList.serializer()
    ) {
        Api.messageList(it + 1)
    }) {
    var config: Config? = null
    init {
        viewModelScope.launch {
            config = Api.config()
        }
    }
}