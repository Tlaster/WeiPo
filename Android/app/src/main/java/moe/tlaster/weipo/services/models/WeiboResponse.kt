package moe.tlaster.weipo.services.models

import kotlinx.serialization.SerialName
import kotlinx.serialization.Serializable

@Serializable
data class WeiboResponse<T>(
    val data: T,
    val ok: Long,
    @SerialName("http_code")
    val httpCode: Long
)