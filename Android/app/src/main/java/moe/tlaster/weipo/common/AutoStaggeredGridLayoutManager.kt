package moe.tlaster.weipo.common

import android.view.View
import androidx.recyclerview.widget.RecyclerView
import androidx.recyclerview.widget.StaggeredGridLayoutManager
import java.util.*
import kotlin.math.abs

class AutoStaggeredGridLayoutManager(columnSize: Int, orientation: Int) :
    StaggeredGridLayoutManager(1, orientation) {

    enum class Strategy {
        MinSize,
        SuitableSize
    }

    var columnSize = columnSize
        set(value) {
            field = value
            columnSizeChanged = true
        }
    private var columnSizeChanged = true
    var strategy: Strategy = Strategy.MinSize
        set(value) {
            field = value
            columnSizeChanged = true
        }

    private var listeners: MutableList<OnUpdateSpanCountListener>? = null

    override fun onMeasure(
        recycler: RecyclerView.Recycler,
        state: RecyclerView.State,
        widthSpec: Int,
        heightSpec: Int
    ) {
        if (columnSizeChanged && columnSize > 0) {
            val totalSpace: Int = if (orientation == StaggeredGridLayoutManager.VERTICAL) {
                check(View.MeasureSpec.EXACTLY == View.MeasureSpec.getMode(widthSpec)) { "RecyclerView need a fixed width for AutoStaggeredGridLayoutManager" }
                View.MeasureSpec.getSize(widthSpec) - paddingRight - paddingLeft
            } else {
                check(View.MeasureSpec.EXACTLY == View.MeasureSpec.getMode(heightSpec)) { "RecyclerView need a fixed height for AutoStaggeredGridLayoutManager" }
                View.MeasureSpec.getSize(heightSpec) - paddingTop - paddingBottom
            }

            val spanCount: Int = when (strategy) {
                Strategy.MinSize -> getSpanCountForMinSize(totalSpace, columnSize)
                Strategy.SuitableSize -> getSpanCountForSuitableSize(totalSpace, columnSize)
            }
            setSpanCount(spanCount)
            columnSizeChanged = false

            if (null != listeners) {
                var i = 0
                val n = listeners!!.size
                while (i < n) {
                    listeners!![i].onUpdateSpanCount(spanCount)
                    i++
                }
            }
        }
        super.onMeasure(recycler, state, widthSpec, heightSpec)
    }

    fun addOnUpdateSpanCountListener(listener: OnUpdateSpanCountListener) {
        if (null == listeners) {
            listeners = ArrayList()
        }
        listeners!!.add(listener)
    }

    fun removeOnUpdateSpanCountListener(listener: OnUpdateSpanCountListener) {
        if (null != listeners) {
            listeners!!.remove(listener)
        }
    }

    interface OnUpdateSpanCountListener {
        fun onUpdateSpanCount(spanCount: Int)
    }

    private fun getSpanCountForSuitableSize(total: Int, single: Int): Int {
        val span = total / single
        if (span <= 0) {
            return 1
        }
        val span2 = span + 1
        val deviation = abs(1 - total.toFloat() / span.toFloat() / single.toFloat())
        val deviation2 = abs(1 - total.toFloat() / span2.toFloat() / single.toFloat())
        return if (deviation < deviation2) span else span2
    }

    private fun getSpanCountForMinSize(total: Int, single: Int): Int {
        return 1.coerceAtLeast(total / single)
    }
}