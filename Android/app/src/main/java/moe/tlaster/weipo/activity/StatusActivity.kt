package moe.tlaster.weipo.activity

import android.os.Bundle
import androidx.activity.viewModels
import androidx.core.os.bundleOf
import com.google.android.material.tabs.TabLayoutMediator
import kotlinx.android.synthetic.main.activity_status.*
import moe.tlaster.weipo.R
import moe.tlaster.weipo.common.adapter.FragmentAdapter
import moe.tlaster.weipo.common.extensions.factory
import moe.tlaster.weipo.fragment.status.HotflowFratgment
import moe.tlaster.weipo.fragment.status.RepostTimelineFragment
import moe.tlaster.weipo.services.models.Status
import moe.tlaster.weipo.viewmodel.status.StatusViewModel

class StatusActivity : BaseActivity() {
    companion object {
        fun bundle(status: Status): Array<Pair<String, Any?>> {
            return arrayOf(
                "status" to status
            )
        }
    }

    private val viewModel: StatusViewModel by viewModels {
        factory {
            StatusViewModel(status) {
                item_status.updateContent(it)
            }
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
            },
            HotflowFratgment().apply {
                arguments = bundleOf(
                    "id" to status?.id?.toLongOrNull(),
                    "mid" to status?.mid?.toLongOrNull()
                )
            }
        )
    }

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        item_status.isTextSelectionEnabled = true
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
        viewModel.init()
    }
}
