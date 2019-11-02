package moe.tlaster.weipo.activity

import android.os.Bundle
import android.view.View
import androidx.core.os.bundleOf
import androidx.lifecycle.Observer
import com.bumptech.glide.request.RequestOptions
import com.google.android.material.tabs.TabLayoutMediator
import kotlinx.android.synthetic.main.activity_user.*
import moe.tlaster.weipo.R
import moe.tlaster.weipo.common.adapter.FragmentAdapter
import moe.tlaster.weipo.common.extensions.factory
import moe.tlaster.weipo.common.extensions.load
import moe.tlaster.weipo.common.extensions.viewModel
import moe.tlaster.weipo.fragment.user.*
import moe.tlaster.weipo.services.models.ProfileData
import moe.tlaster.weipo.viewmodel.UserViewModel

class UserActivity : BaseActivity() {
    private val viewModel by lazy {
        viewModel<UserViewModel>(factory {
            val name = intent.getStringExtra("user_name")
            val id = intent.getLongExtra("user_id", 0)
            if (name != null) {
                return@factory UserViewModel(name)
            }
            if (id != null && id != 0L) {
                return@factory UserViewModel(id)
            }
            throw Error("Name or Id should not be null")
        })
    }

    private val fragmentMapping by lazy {
        mapOf(
            "weibo" to { uid: Long, containerId: String ->
                WeiboTabFragment().apply {
                    arguments = bundleOf(
                        "containerId" to containerId,
                        "userId" to uid
                    )
                }
            }
        )
    }

    private var tabItems = emptyList<TabFragment>()
        set(value) {
            field = value
            pagerAdapter.items = value
        }

    private val pagerAdapter by lazy {
        FragmentAdapter(supportFragmentManager, lifecycle)
    }

    override val layoutId: Int
        get() = R.layout.activity_user

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        view_pager.adapter = pagerAdapter
        viewModel.profile.observe(this, Observer<ProfileData> { profile ->
            profile.userInfo?.coverImagePhone?.let {
                user_background.load(it)
            }
            profile.userInfo?.profileImageURL?.let {
                user_avatar.load(it) {
                    apply(RequestOptions.circleCropTransform())
                }
            }
            profile.userInfo?.screenName?.let {
                user_name.text = it
            }
            profile.userInfo?.verifiedReason?.let {
                user_verify.text = it
            }
            user_verify.visibility = if (profile.userInfo?.verified == true) {
                View.VISIBLE
            } else {
                View.GONE
            }
            profile.userInfo?.description?.let {
                user_desc.text = it
            }
            profile?.userInfo?.followCount?.let {
                user_follow_count.text = it.toString()
            }
            profile.userInfo?.statusesCount?.let {
                user_status_count.text = it.toString()
            }
            profile.userInfo?.followersCount?.let {
                user_follower_count.text = it.toString()
            }
            profile.userInfo?.id?.let { userId ->
                profile.tabsInfo?.tabs?.let { tabs ->
                    tabItems = tabs.map { tab ->
                        if (fragmentMapping.containsKey(tab.tabType) && tab.containerid != null) {
                            return@map fragmentMapping[tab.tabType]!!.invoke(userId, tab.containerid).apply {
                                title = tab.title.toString()
                            }
                        }
                        return@map EmptyTabFragment().apply {
                            title = tab.title.toString()
                        }
                    } + FollowFragment().apply {
                        arguments = bundleOf(
                            "containerId" to containerId,
                            "userId" to userId
                        )
                        title = "Follow"
                    } + FansFragment().apply {
                        arguments = bundleOf(
                            "containerId" to containerId,
                            "userId" to userId
                        )
                        title = "Fans"
                    }
                }
            }
            profile.tabsInfo?.selectedTab?.let {
                view_pager.post {
                    view_pager.setCurrentItem(it.toInt(), false)
                }
            }
        })

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
