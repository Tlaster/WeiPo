package moe.tlaster.weipo.viewmodel

import androidx.annotation.DrawableRes
import androidx.lifecycle.ViewModel
import kotlinx.coroutines.CoroutineStart
import kotlinx.coroutines.GlobalScope
import kotlinx.coroutines.delay
import kotlinx.coroutines.launch
import moe.tlaster.weipo.R
import moe.tlaster.weipo.common.Event
import moe.tlaster.weipo.common.adapter.IncrementalLoadingAdapter
import moe.tlaster.weipo.common.adapter.ItemSelector
import moe.tlaster.weipo.common.collection.IncrementalLoadingCollection
import moe.tlaster.weipo.controls.PersonCard
import moe.tlaster.weipo.controls.StatusView
import moe.tlaster.weipo.datasource.FuncDataSource
import moe.tlaster.weipo.services.Api
import moe.tlaster.weipo.services.models.Attitude
import moe.tlaster.weipo.services.models.Comment
import moe.tlaster.weipo.services.models.MessageList
import moe.tlaster.weipo.services.models.UnreadData

interface INotificationTabItem<T> {
    val title: Int
    val icon: Int
    val adapter: IncrementalLoadingAdapter<T>
}

data class NotificationItemViewModel<T>(
    override val title: Int,
    @DrawableRes override val icon: Int,
    override val adapter: IncrementalLoadingAdapter<T>
): INotificationTabItem<T>

class MentionViewModel: INotificationTabItem<Any> {
    override val title: Int
        get() = R.string.mention
    override val icon: Int
        get() = R.drawable.ic_at_black_24dp

    var isCmt = false

    override val adapter = IncrementalLoadingAdapter<Any>(ItemSelector(R.layout.item_status)).apply {
        autoRefresh = false
        items = IncrementalLoadingCollection(FuncDataSource {
            if (isCmt) {
                Api.mentionsCmt(it + 1)
            } else {
                Api.mentionsAt(it + 1)
            }
        })
        setView<StatusView>(R.id.item_status) { view, item, _, _ ->
            view.data = item
        }
    }
}

class NotificationViewModel : ViewModel() {
    val sources = listOf(
        MentionViewModel(),
        NotificationItemViewModel(
            R.string.comment,
            R.drawable.ic_comment_black_24dp,
            IncrementalLoadingAdapter<Comment>(ItemSelector(R.layout.item_status)).apply {
                autoRefresh = false
                items = IncrementalLoadingCollection(FuncDataSource {
                    Api.comment(it + 1)
                })
                setView<StatusView>(R.id.item_status) { view, item, _, _ ->
                    view.data = item
                }
            }
        ),
        NotificationItemViewModel(
            R.string.attitude,
            R.drawable.ic_thumb_up_black_24dp,
            IncrementalLoadingAdapter<Attitude>(ItemSelector(R.layout.item_status)).apply {
                autoRefresh = false
                items = IncrementalLoadingCollection(FuncDataSource {
                    Api.attitude(it + 1)
                })
                setView<StatusView>(R.id.item_status) { view, item, _, _ ->
                    view.data = item
                }
            }
        ),
        NotificationItemViewModel(
            R.string.direct_message,
            R.drawable.ic_message_black_24dp,
            IncrementalLoadingAdapter<MessageList>(ItemSelector(R.layout.item_person)).apply {
                autoRefresh = false
                items = IncrementalLoadingCollection(FuncDataSource {
                    Api.messageList(it + 1)
                })
                setView<PersonCard>(R.id.item_person) { view, item, _, _ ->
                    item.user?.avatarLarge?.let {
                        view.avatar = it
                    }
                    item.user?.screenName?.let {
                        view.title = it
                    }
                    item.text?.let {
                        view.subTitle = it
                    }
                }
            }
        )
    )

    val unreadChanged = Event<UnreadData>()

    private var unread: UnreadData? = null

    private val task = GlobalScope.launch(start = CoroutineStart.LAZY) {
        while (true) {
            fetchUnread()
            delay(60 * 1000)
        }
    }

    private suspend fun fetchUnread() {
        kotlin.runCatching {
            Api.unread()
        }.onFailure {

        }.onSuccess { newValue ->
            if (notificationCheck(newValue.follower, unread?.follower)) {
                // TODO: send notification
            }

            if (notificationCheck(newValue.mentionStatus, unread?.mentionStatus)) {
                // TODO: send notification
            }

            if (notificationCheck(newValue.mentionCmt, unread?.mentionCmt)) {
                // TODO: send notification
            }

            if (notificationCheck(newValue.cmt, unread?.cmt)) {
                // TODO: send notification
            }

            if (notificationCheck(newValue.dm, unread?.dm)) {
                // TODO: send notification
            }
            unread = newValue
            unreadChanged.invoke(this, newValue)
        }
    }

    private fun notificationCheck(newValue: Long?, oldValue: Long?): Boolean {
        return newValue != null && newValue != 0L && newValue != oldValue
    }

    init {
        task.start()
    }
}