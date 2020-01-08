package moe.tlaster.weipo.services

import com.github.kittinunf.fuel.core.FileDataPart
import com.github.kittinunf.fuel.core.InlineDataPart
import com.github.kittinunf.fuel.core.Request
import com.github.kittinunf.fuel.core.ResponseDeserializable
import com.github.kittinunf.fuel.coroutines.awaitObject
import com.github.kittinunf.fuel.coroutines.awaitStringResponse
import com.github.kittinunf.fuel.httpGet
import com.github.kittinunf.fuel.httpPost
import com.github.kittinunf.fuel.httpUpload
import com.github.kittinunf.fuel.serialization.kotlinxDeserializerOf
import kotlinx.serialization.KSerializer
import kotlinx.serialization.json.Json
import kotlinx.serialization.json.JsonObject
import kotlinx.serialization.json.contentOrNull
import kotlinx.serialization.list
import moe.tlaster.weipo.services.models.*
import java.io.File

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
    return this.data ?: throw Error()
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

    suspend fun profile(uid: Long): ProfileData {
        return "$HOST/api/container/getIndex"
            .httpGet(
                listOf(
                    "type" to "uid",
                    "value" to uid
                )
            )
            .awaitWeiboResponse(ProfileData.serializer())
            .getData()
    }

    suspend fun userId(name: String): Long {
        val (_, response, _) = "$HOST/n/$name"
            .httpGet()
            .awaitStringResponse()
        return "/u/(\\d+)"
            .toRegex()
            .find(response.url.toString())
            ?.let {
                it.groups[1]
            }?.let {
                it.value.toLongOrNull()
            } ?: throw Error("user not found")
    }

    suspend fun config(): Config {
        return "$HOST/api/config"
            .httpGet()
            .awaitWeiboResponse(Config.serializer())
            .getData()
    }

    suspend fun profileTab(uid: Long, containerId: String, since_id: Long = 0): JsonObject {
        return "$HOST/api/container/getIndex"
            .httpGet(listOf(
                "type" to "uid",
                "value" to uid,
                "containerid" to containerId,
                "since_id" to since_id
            ))
            .awaitWeiboResponse(JsonObject.serializer())
            .getData()
    }

    suspend fun follow(uid: Long, page: Int = 1): JsonObject {
        val param = getParamFromProfileInfo(uid, "follow")
        return "$HOST/api/container/getSecond?$param"
            .httpGet(listOf("page" to page))
            .awaitWeiboResponse(JsonObject.serializer())
            .getData()
    }

    private suspend fun getParamFromProfileInfo(uid: Long, key: String): String {
        val info = "$HOST/profile/info"
            .httpGet(
                listOf(
                    "uid" to uid
                )
            )
            .awaitWeiboResponse(JsonObject.serializer())
            .getData()
        val container =
            info[key]?.contentOrNull ?: throw Error("Can not find the user profile info")
        return container.substring(container.indexOf('?') + 1)
    }

    suspend fun myFans(since_id: Long = 0): JsonObject {
        return "$HOST/api/container/getIndex"
            .httpGet(listOf(
                "containerid" to "231016_-_selffans",
                "since_id" to since_id
            ))
            .awaitWeiboResponse(JsonObject.serializer())
            .getData()
    }

    suspend fun fans(uid: Long, page: Int = 1): JsonObject {
        val param = getParamFromProfileInfo(uid, "fans")
        return "$HOST/api/container/getSecond?$param"
            .httpGet(listOf("page" to page))
            .awaitWeiboResponse(JsonObject.serializer())
            .getData()
    }

    suspend fun update(content: String, vararg pics: String): JsonObject {
        val st = config().st
        return "$HOST/api/statuses/update"
            .httpPost(listOf(
                "content" to content,
                "st" to st,
                "picId" to pics.joinToString(",")
            ))
            .header("Referer", "$HOST/compose/?${(if (pics.any()) "&pids=${pics.joinToString(",")}" else "")}")
            .awaitWeiboResponse(JsonObject.serializer())
            .getData()
    }

    suspend fun uploadPic(file: File): UploadPic {
        val st = config().st ?: throw Error()
        return "$HOST/api/statuses/uploadPic"
            .httpUpload()
            .add(InlineDataPart("json", "type"))
            .add(InlineDataPart(st, "st"))
            .add(FileDataPart(file, name = "pic", filename = file.name))
            .header("Referer", "$HOST/compose/")
            .awaitObject(UploadPic.serializer())
    }

    suspend fun repost(content: String, reply: ICanReply, picId: String? = null): JsonObject {
        val st = config().st
        return "$HOST/api/statuses/repost"
            .httpPost(listOf(
                "id" to reply.id,
                "mid" to reply.mid,
                "content" to content,
                "st" to st,
                "picId" to picId
            ))
            .header("Referer", "$HOST/compose/repost?id=${reply.id}${(if (!picId.isNullOrEmpty()) "&pids=${picId}" else "")}")
            .awaitWeiboResponse(JsonObject.serializer())
            .getData()
    }

    suspend fun reply(content: String, comment: Comment, picId: String? = null): JsonObject {
        val st = config().st
        return "$HOST/api/comments/reply"
            .httpPost(listOf(
                "id" to comment.status?.id,
                "mid" to comment.status?.id,
                "content" to content,
                "cid" to comment.id,
                "reply" to comment.id,
                "st" to st,
                "picId" to picId
            ))
            .header("Referer", "$HOST/compose/reply?id=${comment.id}${(if (!picId.isNullOrEmpty()) "&pids=${picId}" else "")}")
            .awaitWeiboResponse(JsonObject.serializer())
            .getData()
    }

    suspend fun comment(content: String, reply: ICanReply, picId: String? = null): JsonObject {
        val st = config().st
        return "$HOST/api/comments/create"
            .httpPost(listOf(
                "id" to reply.id,
                "mid" to reply.mid,
                "content" to content,
                "st" to st,
                "picId" to picId
            ))
            .header("Referer", "$HOST/compose/comment?id=${reply.id}${(if (!picId.isNullOrEmpty()) "&pids=${picId}" else "")}")
            .awaitWeiboResponse(JsonObject.serializer())
            .getData()
    }

    suspend fun repostTimeline(id: Long, page: Int = 1): RepostTimeline {
        return "$HOST/api/statuses/repostTimeline"
            .httpGet(listOf(
                "id" to id,
                "page" to page
            ))
            .awaitWeiboResponse(RepostTimeline.serializer())
            .getData()
    }

    suspend fun hotflow(id: Long, mid: Long, maxid: Long = 0): HotflowData {
        return "$HOST/comments/hotflow"
            .httpGet(listOf(
                "id" to id,
                "mid" to mid,
                "max_id" to maxid,
                "max_id_type" to 0
            ))
            .awaitWeiboResponse(HotflowData.serializer())
            .getData()
    }

    suspend fun hotflowChild(cid: Long, maxid: Long): HotflowChildData {
        return "$HOST/comments/hotFlowChild"
            .httpGet(listOf(
                "cid" to cid,
                "max_id" to maxid,
                "max_id_type" to 0
            ))
            .awaitObject(HotflowChildData.serializer())
    }

    suspend fun extend(id: Long): LongTextData {
        return "$HOST/statuses/extend"
            .httpGet(listOf(
                "id" to id
            ))
            .awaitWeiboResponse(LongTextData.serializer())
            .getData()
    }

    suspend fun storyVideoLink(pageInfoLink: String): StoryData {
        return pageInfoLink.replace("/s/video/index", "/s/video/object")
            .httpGet()
            .awaitWeiboResponse(StoryData.serializer())
            .getData()
    }

    suspend fun emoji(): EmojiData {
        return "https://weibo.com/aj/mblog/face?type=face"
            .httpGet()
            .header("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/79.0.3945.16 Safari/537.36 Edg/79.0.309.15")
            .awaitObject(EmojiData.serializer())
    }

    suspend fun follow(uid: Long): User {
        val st = config().st
        return "$HOST/api/friendships/create"
            .httpPost(listOf(
                "st" to st,
                "uid" to uid
            ))
            .awaitWeiboResponse(User.serializer())
            .getData()
    }

    suspend fun unfollow(uid: Long): User {
        val st = config().st
        return "$HOST/api/friendships/destory"
            .httpPost(listOf(
                "st" to st,
                "uid" to uid
            ))
            .awaitWeiboResponse(User.serializer())
            .getData()
    }

    suspend fun unread(): UnreadData {
        return "$HOST/api/remind/unread"
            .httpGet(listOf(
                "t" to System.currentTimeMillis() / 1000
            ))
            .awaitWeiboResponse(UnreadData.serializer())
            .getData()
    }

    suspend fun chatList(uid: Long, count: Int = 10, unfollowing: Int = 0, since_id: Long = 0, is_continuous: Int = 0): DirectMessageData {
        return "$HOST/api/chat/list"
            .httpPost(arrayListOf(
                "uid" to uid,
                "count" to count,
                "unfollowing" to unfollowing
            ).also {
                if (since_id > 0) {
                    it.add("since_id" to since_id)
                }
                if (is_continuous > 0) {
                    it.add("is_continuous" to is_continuous)
                }
            })
            .awaitWeiboResponse(DirectMessageData.serializer())
            .getData()
    }

    suspend fun chatSend(content: String, uid: Long) : DirectMessageData {
        val st = config().st
        return "$HOST/api/chat/send"
            .httpPost(listOf(
                "st" to st,
                "uid" to uid,
                "content" to content
            ))
            .awaitWeiboResponse(DirectMessageData.serializer())
            .getData()
    }

    suspend fun chatUpload(file: File, tuid: Long): ChatUploadData {
        val st = config().st ?: throw Error()
        return "$HOST/api/chat/send"
            .httpUpload()
            .add(InlineDataPart("tuid", tuid.toString()))
            .add(InlineDataPart(st, "st"))
            .add(FileDataPart(file, name = "file", filename = file.name))
            .header("Referer", "$HOST/message/chat?uid=$tuid&name=msgbox")
            .awaitWeiboResponse(ChatUploadData.serializer())
            .getData()
    }

    suspend fun chatSend(content: String, uid: Long, fids: Long, media_type: Int = 1) : DirectMessageData {
        val st = config().st
        return "$HOST/api/chat/send"
            .httpPost(listOf(
                "st" to st,
                "uid" to uid,
                "content" to content,
                "fids" to fids,
                "media_type" to media_type
            ))
            .awaitWeiboResponse(DirectMessageData.serializer())
            .getData()
    }
}