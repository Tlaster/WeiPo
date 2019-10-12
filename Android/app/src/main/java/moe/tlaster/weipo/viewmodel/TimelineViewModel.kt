package moe.tlaster.weipo.viewmodel

import android.util.Log
import moe.tlaster.mvvmdroid.collection.IIncrementalSource
import moe.tlaster.mvvmdroid.collection.IncrementalLoadingCollection
import moe.tlaster.weipo.services.Api
import moe.tlaster.weipo.services.models.Status


class TimelineDataSource : IIncrementalSource<Status> {
    private var _maxId = 0L

    override suspend fun getPagedItemAsync(page: Int, count: Int): List<Status> {
        if (page == 0) {
            _maxId = 0
        }

        val result = Api.timeline(_maxId)
        _maxId = result.data.nextCursor ?: 0
        return result.data.statuses ?: listOf()
    }

}

class TimelineViewModel : ViewModelBase() {
    val timeline = IncrementalLoadingCollection(TimelineDataSource())
    init {
        timeline.refresh()
        timeline.collectionChanged += { sender, args ->
            Log.i("", "")
        }
    }
}