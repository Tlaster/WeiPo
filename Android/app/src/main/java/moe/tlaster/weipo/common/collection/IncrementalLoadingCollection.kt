package moe.tlaster.weipo.common.collection

import kotlinx.coroutines.GlobalScope
import kotlinx.coroutines.launch
import moe.tlaster.weipo.common.Event

class IncrementalLoadingCollection<TSource: IIncrementalSource<T>, T>(
    private val source: TSource,
    private val itemsPerPage: Int = 20
): ObservableCollection<T>(), ISupportIncrementalLoading {
    enum class CollectionState {
        Loading,
        Completed
    }

    protected var currentPageIndex = 0
    val stateChanged = Event<CollectionState>()
    var isLoading = false

    fun refresh() {
        GlobalScope.launch {
            refreshAsync()
        }
    }

    suspend fun refreshAsync() {
        currentPageIndex = 0
        hasMoreItems = true
        clear()
        loadMoreItemsAsync()
    }

    override suspend fun loadMoreItemsAsync() {
        if (isLoading) {
            return
        }
        isLoading = true
        stateChanged.invoke(this, CollectionState.Loading)
        var result: List<T>? = null
        try {
            result = source.getPagedItemAsync(currentPageIndex++, itemsPerPage)
        } catch (e: Throwable) {

        }
        if (result != null && result.any()) {
            addAll(result)
        } else {
            hasMoreItems = false
        }
        stateChanged.invoke(this, CollectionState.Completed)
        isLoading = false
    }

    override var hasMoreItems: Boolean = true
}