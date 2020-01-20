package moe.tlaster.weipo.viewmodel

import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import moe.tlaster.weipo.common.collection.IIncrementalSource
import moe.tlaster.weipo.common.collection.IncrementalLoadingCollection
import moe.tlaster.weipo.services.Api
import moe.tlaster.weipo.services.models.Msg
import moe.tlaster.weipo.services.models.User


class ChatDataSource(private val uid: Long) : IIncrementalSource<Msg> {
    private var maxId = 0L
    override suspend fun getPagedItemAsync(page: Int, count: Int): List<Msg> {
        if (uid == 0L) {
            return emptyList()
        }
        val result = Api.chatList(uid, max_id = maxId)
        result.msgs.forEach {
            it.user = result.users[it.senderID.toString()]
        }
        maxId = result.msgs.last().id.toLong() - 1
        return result.msgs
    }
}

class ChatViewModel(
    data: User?
) : ViewModel() {
    val source = IncrementalLoadingCollection(ChatDataSource(data?.id ?: 0), scope = viewModelScope)

    fun init() {

    }


    override fun onCleared() {
        super.onCleared()
    }
}