package moe.tlaster.weipo.common

import android.view.View
import android.view.ViewGroup
import androidx.recyclerview.widget.RecyclerView
import kotlin.math.ceil

class NineGridLayoutManager : RecyclerView.LayoutManager() {
    private var itemSize: Double = 0.0
    private val gridSpacing = 8.dp

    override fun generateDefaultLayoutParams(): RecyclerView.LayoutParams {
        return RecyclerView.LayoutParams(
            ViewGroup.LayoutParams.MATCH_PARENT,
            ViewGroup.LayoutParams.WRAP_CONTENT
        )
    }

    override fun isAutoMeasureEnabled(): Boolean {
        return false
    }

    override fun onMeasure(
        recycler: RecyclerView.Recycler,
        state: RecyclerView.State,
        widthSpec: Int,
        heightSpec: Int
    ) {
        super.onMeasure(recycler, state, widthSpec, heightSpec)
        if (itemCount == 0 || state.isPreLayout) {
            return
        }
        val width = View.MeasureSpec.getSize(widthSpec)

        itemSize = width.toDouble() / 3.toDouble() - gridSpacing
        val rowCount = ceil(itemCount.toDouble() / 3.toDouble())
        val totalHeight = rowCount * itemSize + gridSpacing * (rowCount - 1)

        val actualHeightSpec = View.MeasureSpec.makeMeasureSpec(
            totalHeight.toInt(),
            View.MeasureSpec.EXACTLY
        )

        super.onMeasure(recycler, state, widthSpec, actualHeightSpec)
    }

    override fun onLayoutChildren(recycler: RecyclerView.Recycler?, state: RecyclerView.State?) {
        if (recycler == null || state == null || itemCount <= 0 || state.isPreLayout) {
            return
        }

        if (state.itemCount == 0) {
            removeAndRecycleAllViews(recycler)
            return
        }
        detachAndScrapAttachedViews(recycler)
        var currentY = 0.toDouble()
        var currentX = 0.toDouble()
        val size = itemSize
        val itemWidth = View.MeasureSpec.makeMeasureSpec(
            size.toInt(),
            View.MeasureSpec.EXACTLY
        )
        for (i in 0 until itemCount) {

            val view = recycler.getViewForPosition(i)
            addView(view)
            view.measure(itemWidth, itemWidth)
            layoutDecorated(
                view,
                currentX.toInt(),
                currentY.toInt(),
                currentX.toInt() + size.toInt(),
                currentY.toInt() + size.toInt()
            )
            currentX += size + gridSpacing
            if (currentX >= size * 3) {
                currentX = 0.0
                currentY += size + gridSpacing
            }
        }
    }

}