package moe.tlaster.weipo.viewmodel.status

import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import moe.tlaster.weipo.common.collection.IncrementalLoadingCollection
import moe.tlaster.weipo.datasource.FuncDataSource
import moe.tlaster.weipo.services.Api
import moe.tlaster.weipo.services.models.Comment

class HotflowChildViewModel(item: Comment?) : ViewModel() {
    private var maxId = 0L
    val source = IncrementalLoadingCollection(FuncDataSource { page ->
        if (page == 0) {
            maxId = 0L
        }
        if (page != 0 && maxId == 0L) {
            return@FuncDataSource emptyList<Comment>()
        }
        return@FuncDataSource item?.mid?.let { id ->
            val result = Api.hotflowChild(id.toLong(), maxId)
            maxId = result.maxID ?: 0
            result.data ?: emptyList()
        } ?: emptyList()
    }, scope = viewModelScope)
}