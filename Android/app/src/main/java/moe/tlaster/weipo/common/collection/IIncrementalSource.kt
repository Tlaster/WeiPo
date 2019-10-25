package moe.tlaster.weipo.common.collection

interface IIncrementalSource<T> {
    suspend fun getPagedItemAsync(page: Int, count: Int): List<T>
}