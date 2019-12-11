package moe.tlaster.weipo.common.collection

import kotlinx.coroutines.CoroutineScope
import kotlinx.coroutines.GlobalScope
import kotlinx.coroutines.launch
import moe.tlaster.weipo.common.Event

open class IncrementalLoadingCollection<TSource: IIncrementalSource<T>, T>(
    private val source: TSource,
    private val itemsPerPage: Int = 20,
    override val scope: CoroutineScope = GlobalScope
): ObservableCollection<T>(), ISupportIncrementalLoading, ISupportCacheLoading {

    val onError = Event<Throwable>()

    enum class CollectionState {
        Loading,
        Completed
    }

    protected var currentPageIndex = 0
    val stateChanged = Event<CollectionState>()
    var isLoading = false

    fun refresh() {
        scope.launch {
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
            onError.invoke(this, e)
            e.printStackTrace()
        }
        if (result != null && result.any()) {
            addAll(result)
        } else {
            hasMoreItems = false
        }
        stateChanged.invoke(this, CollectionState.Completed)
        isLoading = false
    }

    override suspend fun loadCachedAsync() {
        if (source !is ICachedIncrementalSource<*>) {
            return
        }
        if (isLoading) {
            return
        }
        isLoading = true
        stateChanged.invoke(this, CollectionState.Loading)
        kotlin.runCatching {
            source.getCachedItemsAsync()
        }.onFailure {
            onError.invoke(this, it)
            it.printStackTrace()
        }.onSuccess {
            addAll(it.map { it as T })
        }
        stateChanged.invoke(this, CollectionState.Completed)
        isLoading = false
    }

    override var hasMoreItems: Boolean = true
}
