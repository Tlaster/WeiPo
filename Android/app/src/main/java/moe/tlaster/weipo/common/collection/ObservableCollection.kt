package moe.tlaster.weipo.common.collection

import android.os.Build
import androidx.annotation.RequiresApi
import moe.tlaster.weipo.common.Event
import java.util.function.Predicate

interface INotifyCollectionChanged {
    var collectionChanged: Event<CollectionChangedEventArg>
}

open class ObservableCollection<T> : ArrayList<T>(), INotifyCollectionChanged {

    override var collectionChanged: Event<CollectionChangedEventArg> =
        Event()

    override fun add(element: T): Boolean {
        val result = super.add(element)
        if (result) {
            collectionChanged.invoke(this,
                CollectionChangedEventArg(CollectionChangedType.Add, this.count() - 1, 1)
            )
        }
        return result
    }

    override fun add(index: Int, element: T) {
        super.add(index, element)
        collectionChanged.invoke(this,
            CollectionChangedEventArg(CollectionChangedType.Add, index, 1)
        )
    }

    override fun addAll(elements: Collection<T>): Boolean {
        val result = super.addAll(elements)
        if (result) {
            collectionChanged.invoke(this,
                CollectionChangedEventArg(CollectionChangedType.Add, this.count() - elements.count(), elements.count())
            )
        }
        return result
    }

    override fun addAll(index: Int, elements: Collection<T>): Boolean {
        val result = super.addAll(index, elements)
        if (result) {
            collectionChanged.invoke(this,
                CollectionChangedEventArg(CollectionChangedType.Add, index, elements.count())
            )
        }
        return result
    }

    override fun clear() {
        val size = this.size
        super.clear()
        collectionChanged.invoke(this,
            CollectionChangedEventArg(CollectionChangedType.Reset, 0, size)
        )
    }

    override fun remove(element: T): Boolean {
        val index = indexOf(element)
        val result = super.remove(element)
        if (result) {
            collectionChanged.invoke(this,
                CollectionChangedEventArg(CollectionChangedType.Remove, index, 1)
            )
        }
        return result
    }

    override fun removeAll(elements: Collection<T>): Boolean {
        val result = super.removeAll(elements)
        if (result) {
            collectionChanged.invoke(this,
                CollectionChangedEventArg(CollectionChangedType.Remove)
            )
        }
        return result
    }

    override fun removeAt(index: Int): T {
        val result = super.removeAt(index)
        if (result != null) {
            collectionChanged.invoke(this,
                CollectionChangedEventArg(CollectionChangedType.Remove, index, 1)
            )
        }
        return result
    }

    @RequiresApi(Build.VERSION_CODES.N)
    override fun removeIf(filter: Predicate<in T>): Boolean {
        val index = indexOfFirst { filter.test(it) }
        val result = super.removeIf(filter)
        if (result) {
            collectionChanged.invoke(this,
                CollectionChangedEventArg(CollectionChangedType.Remove, index, 1)
            )
        }
        return result
    }

    override fun removeRange(fromIndex: Int, toIndex: Int) {
        super.removeRange(fromIndex, toIndex)
        collectionChanged.invoke(this,
            CollectionChangedEventArg(CollectionChangedType.Remove, fromIndex, toIndex - fromIndex)
        )
    }
}