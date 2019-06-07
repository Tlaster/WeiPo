package moe.tlaster.weipo.shiba.collection

import android.os.Build
import androidx.annotation.RequiresApi
import moe.tlaster.shiba.Event
import moe.tlaster.shiba.type.CollectionChangedEventArg
import moe.tlaster.shiba.type.CollectionChangedType
import moe.tlaster.shiba.type.ShibaArray
import java.util.function.Predicate

interface INotifyCollectionChanged {
    var collectionChanged: Event<CollectionChangedEventArg>
}

interface ISupportIncrementalLoading {
    suspend fun loadMoreItemsAsync()
    val hasMoreItems: Boolean
}

interface IIncrementalSource<T> {
    suspend fun getPagedItemAsync(page: Int, count: Int): List<T>
}

class IncrementalLoadingCollection<TSource: IIncrementalSource<T>, T>(
    private val source: TSource,
    private val itemsPerPage: Int = 20
): ArrayList<T>(), INotifyCollectionChanged, ISupportIncrementalLoading {
    protected var currentPageIndex = 0
    public var isLoading = false

    suspend fun refresh() {
        currentPageIndex = 0
        hasMoreItems = true
        loadMoreItemsAsync()
    }

    override suspend fun loadMoreItemsAsync() {
        if (isLoading) {
            return
        }
        var result: List<T>? = null
        try {
            isLoading = true
            result = source.getPagedItemAsync(currentPageIndex++, itemsPerPage)
        } catch (e: Throwable) {

        }
        if (result != null && result.any()) {
            addAll(result)
        } else {
            hasMoreItems = false
        }
        isLoading = false
    }

    override var hasMoreItems: Boolean = true


    override var collectionChanged: Event<CollectionChangedEventArg> = Event()

    override fun add(element: T): Boolean {
        val result = super.add(element)
        if (result) {
            collectionChanged.invoke(this, CollectionChangedEventArg(CollectionChangedType.Add))
        }
        return result
    }

    override fun add(index: Int, element: T) {
        super.add(index, element)
        collectionChanged.invoke(this, CollectionChangedEventArg(CollectionChangedType.Add))
    }

    override fun addAll(elements: Collection<T>): Boolean {
        val result = super.addAll(elements)
        if (result) {
            collectionChanged.invoke(this, CollectionChangedEventArg(CollectionChangedType.Add))
        }
        return result
    }

    override fun addAll(index: Int, elements: Collection<T>): Boolean {
        val result = super.addAll(index, elements)
        if (result) {
            collectionChanged.invoke(this, CollectionChangedEventArg(CollectionChangedType.Add))
        }
        return result
    }

    override fun clear() {
        super.clear()
        collectionChanged.invoke(this, CollectionChangedEventArg(CollectionChangedType.Remove))
    }

    override fun remove(element: T): Boolean {
        val result = super.remove(element)
        if (result) {
            collectionChanged.invoke(this, CollectionChangedEventArg(CollectionChangedType.Remove))
        }
        return result
    }

    override fun removeAll(elements: Collection<T>): Boolean {
        val result = super.removeAll(elements)
        if (result) {
            collectionChanged.invoke(this, CollectionChangedEventArg(CollectionChangedType.Remove))
        }
        return result
    }

    override fun removeAt(index: Int): T {
        val result = super.removeAt(index)
        if (result != null) {
            collectionChanged.invoke(this, CollectionChangedEventArg(CollectionChangedType.Remove))
        }
        return result
    }

    @RequiresApi(Build.VERSION_CODES.N)
    override fun removeIf(filter: Predicate<in T>): Boolean {
        val result = super.removeIf(filter)
        if (result) {
            collectionChanged.invoke(this, CollectionChangedEventArg(CollectionChangedType.Remove))
        }
        return result
    }

    override fun removeRange(fromIndex: Int, toIndex: Int) {
        super.removeRange(fromIndex, toIndex)
        collectionChanged.invoke(this, CollectionChangedEventArg(CollectionChangedType.Remove))
    }
}