package moe.tlaster.weipo.activity

import android.os.Bundle
import androidx.core.os.bundleOf
import androidx.core.view.isVisible
import androidx.lifecycle.Observer
import com.bumptech.glide.request.RequestOptions
import com.google.android.material.tabs.TabLayoutMediator
import kotlinx.android.synthetic.main.activity_user.*
import moe.tlaster.weipo.R
import moe.tlaster.weipo.common.adapter.FragmentAdapter
import moe.tlaster.weipo.common.extensions.factory
import moe.tlaster.weipo.common.extensions.load
import moe.tlaster.weipo.common.extensions.viewModel
import moe.tlaster.weipo.fragment.TabFragment
import moe.tlaster.weipo.fragment.user.EmptyTabFragment
import moe.tlaster.weipo.fragment.user.FansFragment
import moe.tlaster.weipo.fragment.user.FollowFragment
import moe.tlaster.weipo.fragment.user.WeiboTabFragment
import moe.tlaster.weipo.services.models.ProfileData
import moe.tlaster.weipo.viewmodel.UserViewModel

class UserActivity : BaseActivity() {

    companion object {
        fun bundle(name: String? = null, id: Long? = null): Array<Pair<String, Any?>> {
            return arrayOf(
                "user_name" to name,
                "user_id" to id
            )
        }
    }

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
        follow_button.setOnClickListener {
            viewModel.updateFollow()
        }
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
            user_verify.isVisible = profile.userInfo?.verified == true
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
            if (tabItems.isEmpty()) {
                profile.userInfo?.id?.let { userId ->
                    profile.tabsInfo?.tabs?.let { tabs ->
                        tabItems = tabs.map { tab ->
                            if (fragmentMapping.containsKey(tab.tabType) && tab.containerid != null) {
                                return@map fragmentMapping[tab.tabType]!!.invoke(
                                    userId,
                                    tab.containerid
                                ).apply {
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
                        } + FansFragment().apply {
                            arguments = bundleOf(
                                "containerId" to containerId,
                                "userId" to userId
                            )
                        }
                    }
                }
                profile.tabsInfo?.selectedTab?.let {
                    view_pager.post {
                        view_pager.setCurrentItem(it.toInt(), false)
                    }
                }
            }
            if (profile?.userInfo?.id == viewModel.config.uid?.toLongOrNull()) {
                follow_button.isVisible = false
            } else {
                follow_button.isVisible = true
                follow_button.text = when {
                    profile.userInfo?.followMe == true && profile.userInfo.following == true -> {
                        getString(R.string.follow_state_twoway)
                    }
                    profile.userInfo?.followMe == true -> {
                        getString(R.string.follow_state_fans_only)
                    }
                    profile.userInfo?.following == true -> {
                        getString(R.string.follow_state_follow_only)
                    }
                    else -> {
                        getString(R.string.follow)
                    }
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
