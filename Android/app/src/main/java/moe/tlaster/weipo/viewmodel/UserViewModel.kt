package moe.tlaster.weipo.viewmodel

import androidx.lifecycle.MutableLiveData
import androidx.lifecycle.ViewModel
import kotlinx.coroutines.GlobalScope
import kotlinx.coroutines.launch
import moe.tlaster.weipo.common.extensions.runOnMainThread
import moe.tlaster.weipo.services.Api
import moe.tlaster.weipo.services.models.Config
import moe.tlaster.weipo.services.models.ProfileData

class UserViewModel : ViewModel {
    lateinit var config: Config
    val profile = MutableLiveData<ProfileData>()

    constructor(id: Long) {
        initProfile(id)
    }
    constructor(name: String) {
        init(name)
    }

    fun updateFollow() {
        profile.value?.userInfo?.following?.let { state ->
            profile.value?.userInfo?.id?.let { id ->
                GlobalScope.launch {
                    val result = if (state) {
                        Api.unfollow(id)
                    } else {
                        Api.follow(id)
                    }
                    val value = Api.profile(id)
                    runOnMainThread {
                        profile.value = value
                    }
                }
            }
        }
    }

    private fun init(name: String) {
        GlobalScope.launch {
            val id = Api.userId(name)
            initProfile(id)
        }
    }

    private fun initProfile(id: Long) {
        GlobalScope.launch {
            config = Api.config()
            val value = Api.profile(id)
            runOnMainThread {
                profile.value = value
            }
        }
    }
}