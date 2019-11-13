package moe.tlaster.weipo.viewmodel

import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import moe.tlaster.weipo.common.collection.IncrementalLoadingCollection
import moe.tlaster.weipo.common.extensions.async
import moe.tlaster.weipo.datasource.TimelineDataSource
import moe.tlaster.weipo.services.Api
import moe.tlaster.weipo.services.models.Config

class TimelineViewModel : ViewModel() {
    val items = IncrementalLoadingCollection(TimelineDataSource(), scope = viewModelScope)
    var config: Config? = null
    init {
        async {
            config = Api.config()
        }
    }
}