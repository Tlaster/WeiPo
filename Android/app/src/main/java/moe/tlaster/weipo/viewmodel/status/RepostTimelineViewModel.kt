package moe.tlaster.weipo.viewmodel.status

import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import moe.tlaster.weipo.common.collection.IncrementalLoadingCollection
import moe.tlaster.weipo.datasource.FuncDataSource
import moe.tlaster.weipo.services.Api

class RepostTimelineViewModel(
    private val id: Long
) : ViewModel() {
    val source = IncrementalLoadingCollection(FuncDataSource{
        Api.repostTimeline(id, it + 1).data ?: emptyList()
    }, scope = viewModelScope)

}