package moe.tlaster.weipo.services.models

import android.os.Parcelable
import kotlinx.serialization.SerialName
import kotlinx.serialization.Serializable

@Serializable
data class Config (
    val login: Boolean? = null,
    val st: String? = null,
    val uid: String? = null
)

@Serializable
data class UploadPic(
    @SerialName("pic_id")
    val picId: String? = null
)

interface ICanReply : Parcelable {
    val id: String?
    val mid: String?
}


@Serializable
data class RepostTimeline (
    val data: List<Status>? = null,

    @SerialName("total_number")
    val totalNumber: Long? = null,

    @SerialName("hot_total_number")
    val hotTotalNumber: Long? = null,

    val max: Long? = null
)


@Serializable
data class HotflowData (
    val data: List<Comment>? = null,

    @SerialName("total_number")
    val totalNumber: Long? = null,

    val status: Status? = null,

    @SerialName("max_id")
    val maxID: Long? = null,

    val max: Long? = null,

    @SerialName("max_id_type")
    val maxIDType: Long? = null
)

@Serializable
data class HotflowChildData (
    val ok: Long? = null,
    val data: List<Comment>? = null,

    @SerialName("total_number")
    val totalNumber: Long? = null,

    @SerialName("max_id")
    val maxID: Long? = null,

    @SerialName("max_id_type")
    val maxIDType: Long? = null,

    val max: Long? = null,
    val rootComment: List<Comment>? = null
)

@Serializable
data class LongTextData (
    val ok: Long? = null,
    val longTextContent: String? = null,
    @SerialName("reposts_count")
    val repostsCount: Count? = null,
    @SerialName("comments_count")
    val commentsCount: Count? = null,
    @SerialName("attitudes_count")
    val attitudesCount: Count? = null
)


@Serializable
data class StoryData (
    @SerialName("object_id")
    val objectID: String? = null,

    @SerialName("object_type")
    val objectType: String? = null,

    @SerialName("object")
    val storyDataObject: Object? = null
)

@Serializable
data class Object (
    val summary: String? = null,
    val author: User? = null,
    val stream: Stream? = null,

    @SerialName("created_at")
    val createdAt: String? = null,

    val image: Image? = null
)

@Serializable
data class Image (
    val width: Long? = null,
    val url: String? = null,
    val height: Long? = null
)

@Serializable
data class Stream (
    val url: String? = null
)

@Serializable
data class EmojiData (
    val code: String? = null,
    val msg: String? = null,
    val data: Data? = null
)

@Serializable
data class Data (
    val usual: Map<String, List<Emoji>>? = null,
    val more: Map<String, List<Emoji>>? = null,
    val brand: Brand? = null
)

@Serializable
data class Brand (
    val norm: Map<String, List<Emoji>>? = null
)

@Serializable
data class Emoji (
    val phrase: String? = null,
    val type: String? = null,
    val url: String? = null,
    val hot: Boolean? = null,
    val common: Boolean? = null,
    val category: String? = null,
    val icon: String? = null,
    val value: String? = null,
    val picid: String? = null
)

@Serializable
data class UnreadData (
    val cmt: Long? = null,
    val status: Long? = null,
    val follower: Long? = null,
    val dm: Long? = null,

    @SerialName("mention_cmt")
    val mentionCmt: Long? = null,

    @SerialName("mention_status")
    val mentionStatus: Long? = null,

    val attitude: Long? = null,
    val unreadmblog: Long? = null,
    val uid: String? = null,
    val bi: Long? = null,
    val newfans: Long? = null,
    val unreadmsg: Map<String, Long>? = null,
//    val group: Any? = null,
    val notice: Long? = null,
    val photo: Long? = null,
    val msgbox: Long? = null
)
