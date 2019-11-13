package moe.tlaster.weipo.viewmodel.user

import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import moe.tlaster.weipo.common.collection.IncrementalLoadingCollection
import moe.tlaster.weipo.datasource.UserWeiboDataSource

class WeiboListViewModel(
    userId: Long,
    containerId: String
) : ViewModel() {
    val source = IncrementalLoadingCollection(UserWeiboDataSource(userId, containerId), scope = viewModelScope)
}