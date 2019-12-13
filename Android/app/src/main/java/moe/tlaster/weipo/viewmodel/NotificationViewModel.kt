package moe.tlaster.weipo.viewmodel

import androidx.lifecycle.MutableLiveData
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import kotlinx.coroutines.CoroutineStart
import kotlinx.coroutines.delay
import kotlinx.coroutines.launch
import moe.tlaster.weipo.common.extensions.runOnMainThread
import moe.tlaster.weipo.services.Api
import moe.tlaster.weipo.services.models.UnreadData


class NotificationViewModel : ViewModel() {

    val unread = MutableLiveData<UnreadData>()

    private val task = viewModelScope.launch(start = CoroutineStart.LAZY) {
        while (true) {
            fetchUnread()
            delay(60 * 1000)
        }
    }

    fun update(action: (UnreadData) -> Unit) {
        unread.value = unread.value?.let {
            action.invoke(it)
            it
        }
    }

    private suspend fun fetchUnread() {
        kotlin.runCatching {
            Api.unread()
        }.onFailure {
        }.onSuccess { newValue ->
            runOnMainThread {
                unread.value = newValue
            }
        }
    }
    init {
        task.start()
    }
}
