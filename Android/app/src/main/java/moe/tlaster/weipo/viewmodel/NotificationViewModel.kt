package moe.tlaster.weipo.viewmodel

import androidx.annotation.DrawableRes
import androidx.lifecycle.ViewModel
import moe.tlaster.weipo.R
import moe.tlaster.weipo.common.adapter.IncrementalLoadingAdapter
import moe.tlaster.weipo.common.adapter.ItemSelector
import moe.tlaster.weipo.common.collection.IIncrementalSource
import moe.tlaster.weipo.common.collection.IncrementalLoadingCollection
import moe.tlaster.weipo.controls.PersonCard
import moe.tlaster.weipo.controls.StatusView
import moe.tlaster.weipo.services.Api
import moe.tlaster.weipo.services.models.Attitude
import moe.tlaster.weipo.services.models.Comment
import moe.tlaster.weipo.services.models.MessageList

class NotificationItemDataSource<T>(
    private val func: suspend (page: Int) -> List<T>
) : IIncrementalSource<T> {

    override suspend fun getPagedItemAsync(page: Int, count: Int): List<T> {
        val result = func.invoke(page + 1)
        return result
    }

}

interface ITabItem {
    val title: String
    val icon: Int
}

data class NotificationItemViewModel<T>(
    override val title: String,
    @DrawableRes override val icon: Int,
    val adapter: IncrementalLoadingAdapter<T>
): ITabItem

class MentionViewModel: ITabItem {
    override val title: String
        get() = "Mention"
    override val icon: Int
        get() = R.drawable.ic_at_black_24dp

    var isCmt = false

    val source = IncrementalLoadingCollection(NotificationItemDataSource {
        if (isCmt) {
            Api.mentionsCmt(it)
        } else {
            Api.mentionsAt(it)
        }
    })

    val adapter = IncrementalLoadingAdapter<Any>(ItemSelector(R.layout.item_status)).apply {
        items = source
        setView<StatusView>(R.id.item_status) { view, item, _, _ ->
            view.data = item
        }
    }
}

class NotificationViewModel : ViewModel() {
    val sources = listOf(
        MentionViewModel(),
//        NotificationItemViewModel(
//            "Mention",
//            R.drawable.ic_at_black_24dp,
//            IncrementalLoadingAdapter<Status>(ItemSelector(R.layout.item_status)).apply {
//                items = IncrementalLoadingCollection(NotificationItemDataSource {
//                    Api.mentionsAt(it)
//                })
//                setView<StatusView>(R.id.item_status) { view, item, _, _ ->
//                    view.data = item
//                }
//            }
//        ),
//        NotificationItemViewModel(
//            "MentionComment",
//            R.drawable.ic_at_black_24dp,
//            IncrementalLoadingAdapter<Comment>(ItemSelector(R.layout.item_status)).apply {
//                items = IncrementalLoadingCollection(NotificationItemDataSource {
//                    Api.mentionsCmt(it)
//                })
//                setView<StatusView>(R.id.item_status) { view, item, _, _ ->
//                    view.data = item
//                }
//            }
//        ),

        NotificationItemViewModel(
            "Comment",
            R.drawable.ic_comment_black_24dp,
            IncrementalLoadingAdapter<Comment>(ItemSelector(R.layout.item_status)).apply {
                items = IncrementalLoadingCollection(NotificationItemDataSource {
                    Api.comment(it)
                })
                setView<StatusView>(R.id.item_status) { view, item, _, _ ->
                    view.data = item
                }
            }
        ),
        NotificationItemViewModel(
            "Attitude",
            R.drawable.ic_thumb_up_black_24dp,
            IncrementalLoadingAdapter<Attitude>(ItemSelector(R.layout.item_status)).apply {
                items = IncrementalLoadingCollection(NotificationItemDataSource {
                    Api.attitude(it)
                })
                setView<StatusView>(R.id.item_status) { view, item, _, _ ->
                    view.data = item
                }
            }
        ),
        NotificationItemViewModel(
            "Message",
            R.drawable.ic_message_black_24dp,
            IncrementalLoadingAdapter<MessageList>(ItemSelector(R.layout.item_person)).apply {
                items = IncrementalLoadingCollection(NotificationItemDataSource {
                    Api.messageList(it)
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
}