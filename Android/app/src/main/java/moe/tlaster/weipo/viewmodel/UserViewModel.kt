package moe.tlaster.weipo.viewmodel

import androidx.lifecycle.MutableLiveData
import androidx.lifecycle.ViewModel
import androidx.lifecycle.ViewModelProvider
import kotlinx.coroutines.GlobalScope
import kotlinx.coroutines.launch
import moe.tlaster.weipo.common.extensions.runOnMainThread
import moe.tlaster.weipo.services.Api
import moe.tlaster.weipo.services.models.ProfileData

class UserViewModel : ViewModel {
    val profile = MutableLiveData<ProfileData>()

    constructor(id: Long) {
        initProfile(id)
    }
    constructor(name: String) {
        init(name)
    }

    private fun init(name: String) {
        GlobalScope.launch {
            val id = Api.userId(name)
            initProfile(id)
        }
    }

    private fun initProfile(id: Long) {
        GlobalScope.launch {
            val value = Api.profile(id)
            runOnMainThread {
                profile.value = value
            }
        }
    }
}