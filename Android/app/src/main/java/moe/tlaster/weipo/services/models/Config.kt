package moe.tlaster.weipo.services.models

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