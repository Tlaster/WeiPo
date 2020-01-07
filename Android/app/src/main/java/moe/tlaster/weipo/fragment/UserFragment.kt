package moe.tlaster.weipo.fragment

import android.os.Bundle
import android.view.View
import androidx.core.os.bundleOf
import androidx.core.view.isVisible
import androidx.fragment.app.activityViewModels
import androidx.lifecycle.Observer
import com.bumptech.glide.request.RequestOptions
import com.google.android.material.tabs.TabLayoutMediator
import kotlinx.android.synthetic.main.fragment_user.*
import moe.tlaster.weipo.R
import moe.tlaster.weipo.activity.FragmentActivity
import moe.tlaster.weipo.common.adapter.FragmentAdapter
import moe.tlaster.weipo.common.extensions.factory
import moe.tlaster.weipo.common.extensions.load
import moe.tlaster.weipo.common.extensions.openActivity
import moe.tlaster.weipo.fragment.user.EmptyTabFragment
import moe.tlaster.weipo.fragment.user.FansFragment
import moe.tlaster.weipo.fragment.user.FollowFragment
import moe.tlaster.weipo.fragment.user.WeiboTabFragment
import moe.tlaster.weipo.services.models.ProfileData
import moe.tlaster.weipo.viewmodel.UserViewModel

class UserFragment : BaseFragment(R.layout.fragment_user) {

    private var userId: Long = 0
    private var userName: String = ""

    private val viewModel by activityViewModels<UserViewModel> {
        factory {
            val result = UserViewModel()
            when {
                userId != 0L -> result.initProfile(userId)
                userName.isNotEmpty() -> result.initProfile(userName)
                else -> result.initMe()
            }
            result
        }
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
        FragmentAdapter(childFragmentManager, lifecycle)
    }

    override fun onSaveInstanceState(outState: Bundle) {
        super.onSaveInstanceState(outState)
        outState.putString("user_name", userName)
        outState.putLong("user_id", userId)
    }

    override fun onViewCreated(view: View, savedInstanceState: Bundle?) {
        super.onViewCreated(view, savedInstanceState)

        (savedInstanceState ?: arguments)?.let { bundle ->
            bundle.getString("user_name").takeIf {
                !it.isNullOrEmpty()
            }?.let {
                userName = it
            }
            bundle.getLong("user_id").takeIf {
                it != 0L
            }?.let {
                userId = it
            }
            back_button.isVisible = userId != 0L || userName.isNotEmpty()
        }

        back_button.setOnClickListener {
            activity?.onBackPressed()
        }

        view_pager.adapter = pagerAdapter
        follow_button.setOnClickListener {
            viewModel.updateFollow()
        }
        viewModel.profile.observe(viewLifecycleOwner, Observer<ProfileData> { profile ->
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
                    follow_container.setOnClickListener {
                        context?.openActivity<FragmentActivity>(
                            "className" to FollowFragment::class.java.name,
                            "show_toolbar" to true,
                            "title_res" to R.string.follow,
                            "data" to bundleOf(
                                "userId" to userId
                            )
                        )
                    }
                    fans_container.setOnClickListener {
                        context?.openActivity<FragmentActivity>(
                            "className" to FansFragment::class.java.name,
                            "show_toolbar" to true,
                            "title_res" to R.string.fans,
                            "data" to bundleOf(
                                "userId" to userId
                            )
                        )
                    }
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