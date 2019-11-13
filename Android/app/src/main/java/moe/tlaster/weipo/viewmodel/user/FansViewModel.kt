package moe.tlaster.weipo.viewmodel.user

import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import kotlinx.serialization.ImplicitReflectionSerializer
import moe.tlaster.weipo.common.collection.IncrementalLoadingCollection
import moe.tlaster.weipo.common.extensions.toObject
import moe.tlaster.weipo.datasource.FuncDataSource
import moe.tlaster.weipo.services.Api
import moe.tlaster.weipo.services.models.User

class FansViewModel(
    val userId: Long
) : ViewModel() {
    private var isMe: Boolean? = null
    @UseExperimental(ImplicitReflectionSerializer::class)
    val source = IncrementalLoadingCollection(FuncDataSource<User> {
        if (it == 0) {
            if (isMe == null) {
                val config = Api.config()
                isMe = config.uid?.toLongOrNull() == userId
            }

            if (isMe == true) {
                //Clear notification for my fans
                Api.myFans()
            }
        }
        Api.fans(userId, it + 1)
            .getArrayOrNull("cards")?.mapNotNull {
                it.jsonObject["user"]?.jsonObject?.toObject<User>()
            } ?: emptyList()
    }, scope = viewModelScope)
}

