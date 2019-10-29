package moe.tlaster.weipo.datasource

import moe.tlaster.weipo.common.collection.IIncrementalSource
import moe.tlaster.weipo.services.Api
import moe.tlaster.weipo.services.models.Status


class TimelineDataSource : IIncrementalSource<Status> {
    private var _maxId = 0L

    override suspend fun getPagedItemAsync(page: Int, count: Int): List<Status> {
        if (page == 0) {
            _maxId = 0
        }

        val result = Api.timeline(_maxId)
        _maxId = result.nextCursor ?: 0
        return result.statuses ?: listOf()
    }
}