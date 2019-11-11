package moe.tlaster.weipo.viewmodel

import androidx.lifecycle.MutableLiveData
import androidx.lifecycle.ViewModel
import kotlinx.coroutines.GlobalScope
import kotlinx.coroutines.launch
import moe.tlaster.weipo.common.extensions.runOnMainThread
import moe.tlaster.weipo.services.Api
import moe.tlaster.weipo.services.models.Config
import moe.tlaster.weipo.services.models.ProfileData

open class UserViewModel : ViewModel() {
    lateinit var config: Config
    val profile = MutableLiveData<ProfileData>()

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

    fun initProfile(name: String) {
        GlobalScope.launch {
            val id = Api.userId(name)
            initProfile(id)
        }
    }

    fun initProfile(id: Long) {
        GlobalScope.launch {
            config = Api.config()
            val value = Api.profile(id)
            runOnMainThread {
                profile.value = value
            }
        }
    }

    fun initMe() {
        GlobalScope.launch {
            config = Api.config()
            config.uid?.toLongOrNull().takeIf {
                it != null && it != 0L
            }?.let {
                Api.profile(it)
            }?.let {
                runOnMainThread {
                    profile.value = it
                }
            }
        }
    }
}