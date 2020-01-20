package moe.tlaster.weipo.services.models

import kotlinx.serialization.SerialName
import kotlinx.serialization.Serializable

@Serializable
data class ProfileData (
    val userInfo: UserInfo? = null,

    @SerialName("fans_scheme")
    val fansScheme: String? = null,

    @SerialName("follow_scheme")
    val followScheme: String? = null,

    val tabsInfo: TabsInfo? = null,
    val scheme: String? = null,
    val showAppTips: Long? = null
)

@Serializable
data class TabsInfo (
    val selectedTab: Long? = null,
    val tabs: List<Tab>? = null
)

@Serializable
data class Tab (
    val id: Long? = null,
    val tabKey: String? = null,

    @SerialName("must_show")
    val mustShow: Long? = null,

    val hidden: Long? = null,
    val title: String? = null,

    @SerialName("tab_type")
    val tabType: String? = null,

    val containerid: String? = null,
    val apipath: String? = null,
    val url: String? = null
)

@Serializable
data class UserInfo (
    val id: Long? = null,

    @SerialName("screen_name")
    val screenName: String? = null,

    @SerialName("profile_image_url")
    val profileImageURL: String? = null,

    @SerialName("profile_url")
    val profileURL: String? = null,

    @SerialName("statuses_count")
    val statusesCount: Long? = null,

    val verified: Boolean? = null,

    @SerialName("verified_type")
    val verifiedType: Long? = null,

    @SerialName("verified_type_ext")
    val verifiedTypeEXT: Long? = null,

    @SerialName("verified_reason")
    val verifiedReason: String? = null,

    @SerialName("close_blue_v")
    val closeBlueV: Boolean? = null,

    val description: String? = null,
    val gender: String? = null,
    val mbtype: Long? = null,
    val urank: Long? = null,
    val mbrank: Long? = null,

    @SerialName("follow_me")
    val followMe: Boolean? = null,

    val following: Boolean? = null,

    @SerialName("followers_count")
    val followersCount: Long? = null,

    @SerialName("follow_count")
    val followCount: Long? = null,

    @SerialName("cover_image_phone")
    val coverImagePhone: String? = null,

    @SerialName("avatar_hd")
    val avatarHD: String? = null,

    val like: Boolean? = null,

    @SerialName("like_me")
    val likeMe: Boolean? = null,

    @SerialName("toolbar_menus")
    val toolbarMenus: List<ToolbarMenu>? = null
)

@Serializable
data class ToolbarMenu (
    val type: String? = null,
    val name: String? = null,
    val pic: String? = null,
    val params: Params? = null,
    val scheme: String? = null
)

@Serializable
data class Params (
    val uid: Long? = null,
    val scheme: String? = null
)

@Serializable
data class DirectMessageData (
    val msgs: List<Msg>,
    val users: Map<String, User>,

    @SerialName("total_number")
    val totalNumber: Long,

    val following: Boolean,

    @SerialName("last_read_mid")
    val lastReadMid: Long? = null,

    val title: String
)

@Serializable
data class Msg (
    @SerialName("created_at")
    val createdAt: String,

    @SerialName("dm_type")
    val dmType: Long,

    val id: String,
    val text: String,

    @SerialName("msg_status")
    val msgStatus: Long,

    @SerialName("media_type")
    val mediaType: Long,

    @SerialName("recipient_id")
    val recipientID: Long,

    @SerialName("recipient_screen_name")
    val recipientScreenName: String,

    @SerialName("sender_id")
    val senderID: Long,

    @SerialName("sender_screen_name")
    val senderScreenName: String,

    val attachment: Attachment? = null,

    var user: User? = null
)

@Serializable
data class Attachment (
    val fid: Long,
    val vfid: Long,
    val filename: String,
    val extension: String,
    val filesize: String,

    @SerialName("original_image")
    val originalImage: OriginalImage,

    val thumbnail: OriginalImage
)

@Serializable
data class OriginalImage (
    val url: String,
    val width: Long,
    val height: Long
)

@Serializable
data class ChatUploadData (
    val fids: Long,
    val vfid: Long,
    val filename: String,
    val filesize: String,

    @SerialName("original_image")
    val originalImage: OriginalImage,

    val thumbnail: OriginalImage
)