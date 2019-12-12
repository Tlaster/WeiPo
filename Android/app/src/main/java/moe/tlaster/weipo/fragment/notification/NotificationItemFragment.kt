package moe.tlaster.weipo.fragment.notification

import android.os.Bundle
import android.view.View
import androidx.fragment.app.Fragment
import androidx.fragment.app.activityViewModels
import kotlinx.android.synthetic.main.layout_list.*
import moe.tlaster.weipo.R
import moe.tlaster.weipo.common.AutoStaggeredGridLayoutManager
import moe.tlaster.weipo.common.adapter.IncrementalLoadingAdapter
import moe.tlaster.weipo.common.adapter.ItemSelector
import moe.tlaster.weipo.common.extensions.bindLoadingCollection
import moe.tlaster.weipo.common.statusWidth
import moe.tlaster.weipo.controls.PersonCard
import moe.tlaster.weipo.controls.StatusView
import moe.tlaster.weipo.services.models.Attitude
import moe.tlaster.weipo.services.models.Comment
import moe.tlaster.weipo.services.models.MessageList
import moe.tlaster.weipo.viewmodel.notification.AttitudeViewModel
import moe.tlaster.weipo.viewmodel.notification.CommentViewModel
import moe.tlaster.weipo.viewmodel.notification.DirectMessageViewModel
import moe.tlaster.weipo.viewmodel.notification.NotificationItemViewModel

abstract class NotificationItemFragment<T> : Fragment(R.layout.layout_list) {
    protected abstract val viewModel: NotificationItemViewModel<T>
    override fun onViewCreated(view: View, savedInstanceState: Bundle?) {
        super.onViewCreated(view, savedInstanceState)
        refresh_layout.bindLoadingCollection(viewModel.source)
        recycler_view.adapter = ceateAdapter()
        recycler_view.layoutManager = AutoStaggeredGridLayoutManager(statusWidth)
    }

    protected open fun ceateAdapter(): IncrementalLoadingAdapter<T> {
        return IncrementalLoadingAdapter(
            ItemSelector<T>(R.layout.item_status)
        ).apply {
            items = viewModel.source
            setView<StatusView>(R.id.item_status) { view, item, _, _ ->
                view.data = item
            }
        }
    }
}

class CommentFragment : NotificationItemFragment<Comment>() {
    override val viewModel by activityViewModels<CommentViewModel>()
}
class AttitudeFragment : NotificationItemFragment<Attitude>() {
    override val viewModel by activityViewModels<AttitudeViewModel>()
}
class DirectMessageFragment : NotificationItemFragment<MessageList>() {
    override val viewModel by activityViewModels<DirectMessageViewModel>()
    override fun ceateAdapter(): IncrementalLoadingAdapter<MessageList> {
        return IncrementalLoadingAdapter<MessageList>(ItemSelector(R.layout.item_person)).apply {
            autoRefresh = false
            items = viewModel.source
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
    }
}