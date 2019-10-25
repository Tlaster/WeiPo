package moe.tlaster.weipo.common.collection

interface ISupportIncrementalLoading {
    suspend fun loadMoreItemsAsync()
    val hasMoreItems: Boolean
}


