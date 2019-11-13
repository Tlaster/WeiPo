package moe.tlaster.weipo.viewmodel.user

import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import kotlinx.serialization.ImplicitReflectionSerializer
import moe.tlaster.weipo.common.collection.IncrementalLoadingCollection
import moe.tlaster.weipo.common.extensions.toObject
import moe.tlaster.weipo.datasource.FuncDataSource
import moe.tlaster.weipo.services.Api
import moe.tlaster.weipo.services.models.User

class FollowViewModel(
    private val userId: Long
) : ViewModel() {
    @UseExperimental(ImplicitReflectionSerializer::class)
    val source = IncrementalLoadingCollection(FuncDataSource {
        Api.follow(userId, it + 1)
            .getArrayOrNull("cards")?.mapNotNull {
                it.jsonObject["user"]?.jsonObject?.toObject<User>()
            } ?: emptyList()
    }, scope = viewModelScope)
}