package moe.tlaster.weipo.viewmodel

import androidx.lifecycle.ViewModel
import moe.tlaster.weipo.common.collection.IncrementalLoadingCollection
import moe.tlaster.weipo.datasource.TimelineDataSource

class TimelineViewModel : ViewModel() {
    val items = IncrementalLoadingCollection(TimelineDataSource())
}