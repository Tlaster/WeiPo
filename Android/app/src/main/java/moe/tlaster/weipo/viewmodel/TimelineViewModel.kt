package moe.tlaster.weipo.viewmodel

import androidx.lifecycle.ViewModel
import kotlinx.coroutines.GlobalScope
import kotlinx.coroutines.launch
import moe.tlaster.weipo.common.collection.IncrementalLoadingCollection
import moe.tlaster.weipo.datasource.TimelineDataSource
import moe.tlaster.weipo.services.Api
import moe.tlaster.weipo.services.models.Config

class TimelineViewModel : ViewModel() {
    val items = IncrementalLoadingCollection(TimelineDataSource())
    var config: Config? = null
    init {
        GlobalScope.launch {
            config = Api.config()
        }
    }
}