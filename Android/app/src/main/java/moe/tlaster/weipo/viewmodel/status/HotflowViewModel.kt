package moe.tlaster.weipo.viewmodel.status

import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import moe.tlaster.weipo.common.collection.IncrementalLoadingCollection
import moe.tlaster.weipo.datasource.FuncDataSource
import moe.tlaster.weipo.services.Api

class HotflowViewModel(
    private val id: Long,
    private val mid: Long
) : ViewModel() {
    private var maxId = 0L

    val source = IncrementalLoadingCollection(FuncDataSource {
        if (it == 0) {
            maxId = 0L
        }
        val result = Api.hotflow(id, mid, maxId)
        maxId = result.maxID ?: 0L
        return@FuncDataSource result.data ?: emptyList()
    }, scope = viewModelScope)
}