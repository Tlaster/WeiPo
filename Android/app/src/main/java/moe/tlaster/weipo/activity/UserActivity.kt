package moe.tlaster.weipo.activity

import android.os.Bundle
import android.view.View
import androidx.fragment.app.Fragment
import androidx.lifecycle.Observer
import com.google.android.material.tabs.TabLayoutMediator
import kotlinx.android.synthetic.main.activity_user.*
import moe.tlaster.weipo.R
import moe.tlaster.weipo.common.adapter.AutoAdapter
import moe.tlaster.weipo.common.adapter.ItemSelector
import moe.tlaster.weipo.common.extensions.load
import moe.tlaster.weipo.common.extensions.viewModel
import moe.tlaster.weipo.services.models.ProfileData
import moe.tlaster.weipo.viewmodel.UserViewModel
import moe.tlaster.weipo.viewmodel.UserViewModelFactory

class UserActivity : BaseActivity() {
    private val viewModel by lazy {
        viewModel<UserViewModel>(UserViewModelFactory(
            name = intent.getStringExtra("user_name"),
            id = intent.getLongExtra("user_id", 0)
        ))
    }

    override val layoutId: Int
        get() = R.layout.activity_user

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        viewModel.profile.observe(this, Observer<ProfileData> { profile ->
            profile.userInfo?.coverImagePhone?.let {
                user_background.load(it)
            }
            profile.userInfo?.profileImageURL?.let {
                user_avatar.load(it)
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
        })

        TabLayoutMediator(
            tab_layout,
            view_pager,
            TabLayoutMediator.TabConfigurationStrategy { tab, position ->

            }).attach()
    }
}
