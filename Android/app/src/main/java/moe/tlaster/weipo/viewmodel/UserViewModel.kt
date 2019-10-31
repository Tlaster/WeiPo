package moe.tlaster.weipo.viewmodel

import androidx.lifecycle.MutableLiveData
import androidx.lifecycle.ViewModel
import androidx.lifecycle.ViewModelProvider
import kotlinx.coroutines.GlobalScope
import kotlinx.coroutines.launch
import moe.tlaster.weipo.services.Api
import moe.tlaster.weipo.services.models.ProfileData

class UserViewModelFactory(
    val name: String? = null,
    val id: Long? = null
) : ViewModelProvider.Factory {
    override fun <T : ViewModel?> create(modelClass: Class<T>): T {
        if (name != null) {
            return UserViewModel(name) as T
        }
        if (id != null && id != 0L) {
            return UserViewModel(id) as T
        }
        throw Error("Name or Id should not be null")
    }
}

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
            profile.value = Api.profile(id)
        }
    }
}