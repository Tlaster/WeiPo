package moe.tlaster.weipo.services

import com.github.kittinunf.fuel.core.Request
import com.github.kittinunf.fuel.core.ResponseDeserializable
import com.github.kittinunf.fuel.coroutines.awaitObject
import com.github.kittinunf.fuel.httpGet
import com.github.kittinunf.fuel.serialization.kotlinxDeserializerOf
import kotlinx.serialization.KSerializer
import kotlinx.serialization.json.Json
import moe.tlaster.weipo.services.models.TimelineData
import moe.tlaster.weipo.services.models.WeiboResponse

inline fun <reified T : Any> defaultKotlinxDeserializerOf(loader: KSerializer<T>): ResponseDeserializable<T> {
    return kotlinxDeserializerOf(loader, json = Json.nonstrict)
}

suspend inline fun <reified T : Any> Request.awaitObject(loader: KSerializer<T>): T {
    return this.awaitObject(defaultKotlinxDeserializerOf(loader))
}

suspend inline fun <reified T : Any> Request.awaitWeiboResponse(loader: KSerializer<T>): WeiboResponse<T> {
    return this.awaitObject(defaultKotlinxDeserializerOf(WeiboResponse.serializer(loader)))
}

object Api {
    const val HOST = "https://m.weibo.cn"

    suspend fun timeline(maxid: Long = 0): WeiboResponse<TimelineData> {
        return "$HOST/feed/friends"
            .httpGet(listOf("max_id" to maxid))
            .awaitWeiboResponse(TimelineData.serializer())
    }


}