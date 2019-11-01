package moe.tlaster.weipo.viewmodel.user

import androidx.lifecycle.ViewModel
import moe.tlaster.weipo.common.collection.IncrementalLoadingCollection
import moe.tlaster.weipo.datasource.UserWeiboDataSource

class WeiboListViewModel(
    userId: Long,
    containerId: String
) : ViewModel() {
    val source = IncrementalLoadingCollection(UserWeiboDataSource(userId, containerId))
}