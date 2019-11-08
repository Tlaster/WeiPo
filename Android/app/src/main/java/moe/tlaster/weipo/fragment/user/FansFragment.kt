package moe.tlaster.weipo.fragment.user

import android.os.Bundle
import android.view.View
import kotlinx.android.synthetic.main.layout_list.*
import moe.tlaster.weipo.R
import moe.tlaster.weipo.common.AutoStaggeredGridLayoutManager
import moe.tlaster.weipo.common.adapter.IncrementalLoadingAdapter
import moe.tlaster.weipo.common.adapter.ItemSelector
import moe.tlaster.weipo.common.extensions.bindLoadingCollection
import moe.tlaster.weipo.common.extensions.factory
import moe.tlaster.weipo.common.extensions.viewModel
import moe.tlaster.weipo.common.userWidth
import moe.tlaster.weipo.controls.UserCard
import moe.tlaster.weipo.services.models.User
import moe.tlaster.weipo.viewmodel.user.FansViewModel

class FansFragment : UserTabFragment() {
    override val contentLayoutId: Int
        get() = R.layout.layout_list

    override val titleRes: Int
        get() = R.string.fans

    val viewModel by lazy {
        viewModel<FansViewModel>(factory {
            FansViewModel(userId)
        })
    }

    val adapter by lazy {
        IncrementalLoadingAdapter<User>(ItemSelector(R.layout.item_user)).apply {
            items = viewModel.source
            setView<UserCard>(R.id.user_card) { view, item, _, _ ->
                view.user = item
            }
        }
    }

    override fun onViewCreated(view: View, savedInstanceState: Bundle?) {
        super.onViewCreated(view, savedInstanceState)
        recycler_view.layoutManager = AutoStaggeredGridLayoutManager(userWidth)
        recycler_view.adapter = adapter
        refresh_layout.bindLoadingCollection(viewModel.source)
    }
}