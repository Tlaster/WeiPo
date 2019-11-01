package moe.tlaster.weipo.datasource

import moe.tlaster.weipo.common.collection.IIncrementalSource


class FuncDataSource<T>(
    private val func: suspend (page: Int) -> List<T>
) : IIncrementalSource<T> {
    override suspend fun getPagedItemAsync(page: Int, count: Int): List<T> {
        return func.invoke(page)
    }
}