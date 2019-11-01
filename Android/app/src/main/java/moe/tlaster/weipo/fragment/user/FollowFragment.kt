package moe.tlaster.weipo.fragment.user

import android.os.Bundle
import android.view.View
import kotlinx.android.synthetic.main.layout_list.*
import moe.tlaster.weipo.R
import moe.tlaster.weipo.common.AutoStaggeredGridLayoutManager
import moe.tlaster.weipo.common.adapter.IncrementalLoadingAdapter
import moe.tlaster.weipo.common.adapter.ItemSelector
import moe.tlaster.weipo.common.extensions.bindLoadingCollection
import moe.tlaster.weipo.common.extensions.dp
import moe.tlaster.weipo.common.extensions.factory
import moe.tlaster.weipo.common.extensions.viewModel
import moe.tlaster.weipo.services.models.User
import moe.tlaster.weipo.viewmodel.user.FollowViewModel


class FollowFragment(
    userId: Long
) : UserTabFragment(R.layout.layout_list, userId, containerId = "") {
    val viewModel by lazy {
        viewModel<FollowViewModel>(factory {
            FollowViewModel(userId)
        })
    }

    val adapter by lazy {
        IncrementalLoadingAdapter<User>(ItemSelector(R.layout.control_user)).apply {
            items = viewModel.source
        }
    }

    override fun onViewCreated(view: View, savedInstanceState: Bundle?) {
        super.onViewCreated(view, savedInstanceState)
        recycler_view.layoutManager = AutoStaggeredGridLayoutManager(100.dp.toInt())
        recycler_view.adapter = adapter
        refresh_layout.bindLoadingCollection(viewModel.source)
    }
}