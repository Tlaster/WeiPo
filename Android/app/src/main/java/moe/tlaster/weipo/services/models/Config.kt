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
    val repostsCount: Long? = null,
    @SerialName("comments_count")
    val commentsCount: Long? = null,
    @SerialName("attitudes_count")
    val attitudesCount: Long? = null
)