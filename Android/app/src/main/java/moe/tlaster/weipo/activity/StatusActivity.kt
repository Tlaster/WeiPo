package moe.tlaster.weipo.activity

import android.os.Bundle
import androidx.core.os.bundleOf
import com.google.android.material.tabs.TabLayoutMediator
import kotlinx.android.synthetic.main.activity_status.*
import moe.tlaster.weipo.R
import moe.tlaster.weipo.common.adapter.FragmentAdapter
import moe.tlaster.weipo.fragment.status.RepostTimelineFragment
import moe.tlaster.weipo.services.models.Status

class StatusActivity : BaseActivity() {
    companion object {
        fun bundle(status: Status): Array<Pair<String, Any?>> {
            return arrayOf(
                "status" to status
            )
        }
    }

    private val status by lazy {
        intent.getParcelableExtra<Status>("status")
    }
    override val layoutId: Int
        get() = R.layout.activity_status

    private val tabItems by lazy {
        listOf(
            RepostTimelineFragment().apply {
                arguments = bundleOf(
                    "id" to status?.id?.toLongOrNull()
                )
            }
        )
    }

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        item_status.data = status
        view_pager.adapter = FragmentAdapter(supportFragmentManager, lifecycle).apply {
            items = tabItems
        }
        TabLayoutMediator(
            tab_layout,
            view_pager,
            TabLayoutMediator.TabConfigurationStrategy { tab, position ->
                tabItems[position].let {
                    if (it.icon != -1) {
                        tab.setIcon(it.icon)
                    }
                    when {
                        it.titleRes != -1 -> tab.setText(it.titleRes)
                        it.title.isNotEmpty() -> tab.text = it.title
                        else -> {
                        }
                    }
                }
            }).attach()
    }

}
