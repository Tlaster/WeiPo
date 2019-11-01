package moe.tlaster.weipo.datasource

import kotlinx.serialization.ImplicitReflectionSerializer
import kotlinx.serialization.json.JsonObject
import kotlinx.serialization.json.contentOrNull
import kotlinx.serialization.json.longOrNull
import moe.tlaster.weipo.common.collection.IIncrementalSource
import moe.tlaster.weipo.common.extensions.toObject
import moe.tlaster.weipo.services.Api
import moe.tlaster.weipo.services.models.Status


class UserWeiboDataSource(
    val userId: Long,
    val containerId: String
) : IIncrementalSource<Any> {

    var sinceId = 0L

    @ImplicitReflectionSerializer
    override suspend fun getPagedItemAsync(page: Int, count: Int): List<Any> {
        if (page == 0) {
            sinceId = 0
        }
        val result = Api.profileTab(userId, containerId, sinceId)
        sinceId = result.getObjectOrNull("cardlistInfo")?.get("since_id")?.longOrNull ?: 0L
        return result.getArray("cards").map {
            if (it !is JsonObject) {
                return@map null
            }
            if (it.containsKey("mblog")) {
                return@map it.getObject("mblog").toObject<Status>() as Any
            }
            if (it.containsKey("itemid") && it["itemid"]?.contentOrNull == "INTEREST_PEOPLE") {
                //TODO
            }
            return@map null
        }.filterNotNull()
    }
}
