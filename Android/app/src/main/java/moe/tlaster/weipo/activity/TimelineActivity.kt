package moe.tlaster.weipo.activity

import android.os.Bundle
import kotlinx.android.synthetic.main.activity_timeline.*
import moe.tlaster.weipo.R
import moe.tlaster.weipo.common.adapter.IncrementalLoadingAdapter
import moe.tlaster.weipo.common.adapter.ItemSelector
import moe.tlaster.weipo.common.collection.IncrementalLoadingCollection
import moe.tlaster.weipo.controls.StatusView
import moe.tlaster.weipo.datasource.TimelineDataSource
import moe.tlaster.weipo.services.models.Status



class TimelineActivity : BaseActivity() {
    private val adapter by lazy {
        IncrementalLoadingAdapter<Status>(ItemSelector(R.layout.item_status)).apply {
            items = IncrementalLoadingCollection(TimelineDataSource())
            setView<StatusView>(R.id.item_status) { view, item, _, _ ->
                view.status = item
            }
        }
    }

    override val layoutId: Int
        get() = R.layout.activity_timeline

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        recycler_view.adapter = adapter
    }
}
