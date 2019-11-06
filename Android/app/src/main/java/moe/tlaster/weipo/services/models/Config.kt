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