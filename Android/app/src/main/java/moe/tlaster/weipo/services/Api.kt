package moe.tlaster.weipo.services

import com.github.kittinunf.fuel.core.Request
import com.github.kittinunf.fuel.core.ResponseDeserializable
import com.github.kittinunf.fuel.coroutines.awaitObject
import com.github.kittinunf.fuel.httpGet
import com.github.kittinunf.fuel.serialization.kotlinxDeserializerOf
import kotlinx.serialization.KSerializer
import kotlinx.serialization.json.Json
import kotlinx.serialization.list
import moe.tlaster.weipo.services.models.*

inline fun <reified T : Any> defaultKotlinxDeserializerOf(loader: KSerializer<T>): ResponseDeserializable<T> {
    return kotlinxDeserializerOf(loader, json = Json.nonstrict)
}

suspend inline fun <reified T : Any> Request.awaitObject(loader: KSerializer<T>): T {
    return this.awaitObject(defaultKotlinxDeserializerOf(loader))
}

suspend inline fun <reified T : Any> Request.awaitWeiboResponse(loader: KSerializer<T>): WeiboResponse<T> {
    return this.awaitObject(defaultKotlinxDeserializerOf(WeiboResponse.serializer(loader)))
}

suspend inline fun <reified T: Any> WeiboResponse<T>.getData(): T {
    return this.data
}

object Api {
    const val HOST = "https://m.weibo.cn"

    suspend fun timeline(maxid: Long = 0): TimelineData {
        return "$HOST/feed/friends"
            .httpGet(listOf("max_id" to maxid))
            .awaitWeiboResponse(TimelineData.serializer())
            .getData()
    }


    suspend fun mentionsAt(page: Int = 1): List<Status> {
        return "$HOST/message/mentionsAt"
            .httpGet(listOf("page" to page))
            .awaitWeiboResponse(Status.serializer().list)
            .getData()
    }

    suspend fun mentionsCmt(page: Int = 1): List<Comment> {
        return "$HOST/message/mentionsCmt"
            .httpGet(listOf("page" to page))
            .awaitWeiboResponse(Comment.serializer().list)
            .getData()
    }

    suspend fun comment(page: Int = 1): List<Comment> {
        return "$HOST/message/cmt"
            .httpGet(listOf("page" to page))
            .awaitWeiboResponse(Comment.serializer().list)
            .getData()
    }

    suspend fun attitude(page: Int = 1): List<Attitude> {
        return "$HOST/message/attitude"
            .httpGet(listOf("page" to page))
            .awaitWeiboResponse(Attitude.serializer().list)
            .getData()
    }

    suspend fun messageList(page: Int = 1): List<MessageList> {
        return "$HOST/message/msglist"
            .httpGet(listOf("page" to page))
            .awaitWeiboResponse(MessageList.serializer().list)
            .getData()
    }
}