package moe.tlaster.weipo.controls

import android.content.Context
import android.graphics.Color
import android.text.method.LinkMovementMethod
import android.util.AttributeSet
import android.view.MotionEvent
import android.view.View
import android.widget.ImageView
import android.widget.LinearLayout
import androidx.cardview.widget.CardView
import androidx.core.os.bundleOf
import androidx.core.view.isVisible
import androidx.core.view.updateMarginsRelative
import androidx.core.view.updatePaddingRelative
import androidx.lifecycle.LifecycleOwner
import androidx.lifecycle.coroutineScope
import androidx.recyclerview.widget.LinearLayoutManager
import androidx.recyclerview.widget.RecyclerView
import kotlinx.android.synthetic.main.control_status.view.*
import kotlinx.coroutines.GlobalScope
import kotlinx.coroutines.launch
import moe.tlaster.weipo.R
import moe.tlaster.weipo.activity.*
import moe.tlaster.weipo.common.adapter.AutoAdapter
import moe.tlaster.weipo.common.adapter.ItemSelector
import moe.tlaster.weipo.common.extensions.*
import moe.tlaster.weipo.common.fromHtml
import moe.tlaster.weipo.common.openWeiboLink
import moe.tlaster.weipo.fragment.UserFragment
import moe.tlaster.weipo.services.Api
import moe.tlaster.weipo.services.models.*
import moe.tlaster.weipo.viewmodel.ComposeViewModel
import java.text.SimpleDateFormat
import java.util.*

class StatusView : LinearLayout {
    private val linkClicked: ((url: String) -> Unit) = {
        openWeiboLink(context, it)
    }
    private lateinit var repostView: StatusView

    var data: Any? = null
        set(value) {
            field = value
            when (value) {
                is Status -> onStatusChanged(value)
                is Comment -> onCommentChanged(value)
                is Attitude -> onAttitudeChanged(value)
            }
        }

    private fun onAttitudeChanged(value: Attitude) {
        updateHotFlowVisibility(false)
        story_container.isVisible = false
        video_container.isVisible = false
        action_container.visibility = View.GONE
        status_title.visibility = View.GONE
        status_title_divider.visibility = View.GONE
        status_image.isVisible = false
        value.user?.let {
            setPersonCard(it)
        }
        value.createdAt?.also {
            status_person.subTitle = it.toHumanizedTime()
        }
        if (value.status != null && showRepost) {
            initRepostView()
            repostView.data = value.status
        }
        repost_container.visibility = if (value.status == null && showRepost) {
            View.GONE
        } else {
            View.VISIBLE
        }
        status_content.text = context.getString(R.string.attitude_action_text)
    }

    private fun setPersonCard(user: User) {
        user.profileImageURL?.let {
            status_person.avatar = it
        }
        user.screenName?.let {
            status_person.title = it
        }
    }

    private fun onCommentChanged(value: Comment) {
        updateHotFlowVisibility(value.comments != null)
        story_container.isVisible = false
        video_container.isVisible = false
        hotflow_more_button.isVisible = value.totalNumber ?: 0L > 0L
        value.comments?.let {
            it as? Comments.ListValue
        }?.let {
            hotflow_list.updateItemsSource(it.value)
            hotflow_list.isVisible = it.value.any()
        }
        action_container.visibility = if (showActions) {
            View.VISIBLE
        } else {
            View.GONE
        }
        status_title.visibility = View.GONE
        status_title_divider.visibility = View.GONE
        repost_button.visibility = View.GONE
        status_image.isVisible = value.pic != null
        value.user?.let {
            setPersonCard(it)
        }
        value.createdAt?.also {
            status_person.subTitle = it.toHumanizedTime()
        }
        value.text?.also { html ->
            status_content.text = fromHtml(html, status_content) { url ->
                linkClicked.invoke(url)
            }
        }
        value.pic?.also {
            status_image.updateItemsSource(listOf(it))
        }
        if (value.status != null && showRepost) {
            initRepostView()
            repostView.data = value.status
        }
        repost_container.visibility = if (value.status == null && showRepost) {
            View.GONE
        } else {
            View.VISIBLE
        }
        comment_count.text = if (value.replyCount != null && value.replyCount > 0) {
            value.replyCount.toString()
        } else {
            ""
        }
        like_count.text = if (value.likeCount != null && value.likeCount > 0) {
            value.likeCount.toString()
        } else {
            ""
        }
        hotflow_more_button.text = "${value.totalNumber ?: 0} More"
    }

    private fun onStatusChanged(value: Status) {
        updateHotFlowVisibility(false)
        action_container.isVisible = showActions
        if (value.title == null) {
            View.GONE
        } else {
            View.VISIBLE
        }.also {
            status_title.visibility = it
            status_title_divider.visibility = it
            status_title.text = value.title?.text ?: ""
        }

        video_container.isVisible =
            value.pageInfo != null && (value.pageInfo.type == "article" || value.pageInfo.type == "video")
        story_container.isVisible =
            value.pageInfo != null && value.pageInfo.type == "story"
        status_image.isVisible = value.pics != null
        value.user?.let {
            setPersonCard(it)
        }
        value.createdAt?.also {
            status_person.subTitle = it.toHumanizedTime()
        }
        value.text?.also { html ->
            status_content.text = fromHtml(html, status_content) { url ->
                linkClicked?.invoke(url)
            }
        }
        value.pics?.also {
            status_image.updateItemsSource(it)
        }
        if (value.retweetedStatus != null && showRepost) {
            initRepostView()
            repostView.data = value.retweetedStatus
        }
        repost_container.visibility = if (value.retweetedStatus == null && showRepost) {
            View.GONE
        } else {
            View.VISIBLE
        }
        repost_count.text = if (value.repostsCount != null && value.repostsCount > 0) {
            value.repostsCount.toString()
        } else {
            ""
        }
        comment_count.text = if (value.commentsCount != null && value.commentsCount > 0) {
            value.commentsCount.toString()
        } else {
            ""
        }
        like_count.text = if (value.attitudesCount != null && value.attitudesCount > 0) {
            value.attitudesCount.toString()
        } else {
            ""
        }
        value.pageInfo?.let { pageInfo ->
            pageInfo.pagePic?.url?.let { video_image.load(it) }
            pageInfo.pageTitle?.let { video_title.text = it }
            pageInfo.content1?.let { video_content1.text = it }
            pageInfo.content2?.let { video_content2.text = it }
            video_time.text = kotlin.run {
                pageInfo.mediaInfo?.duration?.let {
                    val tz = TimeZone.getTimeZone("UTC")
                    val df = SimpleDateFormat("HH:mm:ss", Locale.US)
                    df.timeZone = tz
                    df.format(Date((it * 1000).toLong()))
                } ?: ""
            }
        }
    }

    private fun updateHotFlowVisibility(isVisible: Boolean) {
        hotflow_list.isVisible = isVisible
        hotflow_more_button.isVisible = isVisible
    }

    private fun initRepostView() {
        if (!this::repostView.isInitialized) {
            repostView = StatusView(context)
            repostView.showActions = false
            repostView.showRepost = false
            repostView.layoutParams?.let {
                it as MarginLayoutParams
            }?.also {
                it.updateMarginsRelative(start = 8.dp.toInt(), end = 8.dp.toInt())
            }
            repostView.setBackgroundColor(Color.argb(16, 0, 0, 0))
            repost_container.addView(repostView)
        }
    }

    var showActions = true
        set(value) {
            field = value
            action_container?.isVisible = value
        }

    var showRepost = true
        set(value) {
            field = value
            repost_container?.isVisible = value
        }

    var showHotflow
        get() = hotflow_list.isVisible
        set(value) {
            hotflow_list.isVisible = value
            hotflow_more_button.isVisible = value
        }

    var isTextSelectionEnabled: Boolean
        get() = status_content.isTextSelectable
        set(value) = status_content.setTextIsSelectable(value)

    constructor(context: Context) : super(context)
    constructor(context: Context, attrs: AttributeSet?) : super(context, attrs)
    constructor(context: Context, attrs: AttributeSet?, defStyleAttr: Int) : super(
        context,
        attrs,
        defStyleAttr
    )

    init {
        orientation = VERTICAL
        inflate(R.layout.control_status)
        status_image.adapter = AutoAdapter<Pic>(ItemSelector(R.layout.item_image)).apply {
            setView<ImageView>(R.id.image) { view, item, _, _ ->
                view.load(item.url ?: "")
                view.setOnClickListener {
                    onImageClicked(item, items)
                }
            }
        }
        status_image.addOnItemTouchListener(object : RecyclerView.SimpleOnItemTouchListener() {
            override fun onInterceptTouchEvent(
                recyclerView: RecyclerView,
                motionEvent: MotionEvent
            ): Boolean {
                if (motionEvent.action != MotionEvent.ACTION_UP) {
                    return false
                }
                val child = recyclerView.findChildViewUnder(motionEvent.x, motionEvent.y)
                return if (child != null) {
                    // tapped on child
                    false
                } else {
                    showStatusDetail()
                    true
                }
            }
        })

        status_content.movementMethod = LinkMovementMethod.getInstance()
        updatePaddingRelative(bottom = 8.dp.toInt())
        status_person.setOnClickListener {
            data?.let {
                it as? IWithUser
            }?.let {
                it.user?.id
            }?.let {
                context.openActivity<FragmentActivity>(
                    "className" to UserFragment::class.java.name,
                    "data" to bundleOf(
                        "user_id" to it
                    )
                )
            }
        }
        repost_button.setOnClickListener { _ ->
            data?.let {
                it as ICanReply
            }?.let {
                context.openActivity<ComposeActivity>(
                    *ComposeActivity.bundle(
                        ComposeViewModel.ComposeType.Repost,
                        it
                    )
                )
            }
        }
        comment_button.setOnClickListener { _ ->
            data?.let {
                it as ICanReply
            }?.let {
                context.openActivity<ComposeActivity>(
                    *ComposeActivity.bundle(
                        ComposeViewModel.ComposeType.Comment,
                        it
                    )
                )
            }
        }
        like_button.setOnClickListener {

        }
        status_content.setOnClickListener {
            showStatusDetail()
        }
        hotflow_list.layoutManager = LinearLayoutManager(context)
        hotflow_list.adapter = AutoAdapter<Comment>(ItemSelector(R.layout.item_status)).apply {
            setView<StatusView>(R.id.item_status) { view, item, _, _ ->
                view.showActions = false
                view.showRepost = false
                view.data = item
            }
            setView<CardView>(R.id.item_status_card) { view, _, _, _ ->
                view.cardElevation = 0F
            }
        }
        video_container.setOnClickListener {
            data?.let {
                it as? Status
            }?.pageInfo?.also { pageInfo ->
                when (pageInfo.type) {
                    "video" -> {
                        var url = pageInfo.urls?.let {
                            //trick: new [] { "mp4_720p_mp4", "mp4_hd_mp4", "mp4_ld_mp4", "mp4_1080p_mp4", "pre_ld_mp4"}.OrderBy(it => it) => OrderedEnumerable<string, string> { "mp4_1080p_mp4", "mp4_720p_mp4", "mp4_hd_mp4", "mp4_ld_mp4", "pre_ld_mp4" }
                            it.toList().minBy { it.first }?.second
                        }
                        if (url.isNullOrEmpty()) {
                            url = pageInfo.mediaInfo?.mp4_720p_mp4
                        }
                        if (url.isNullOrEmpty()) {
                            url = pageInfo.mediaInfo?.streamURLHD
                        }
                        if (url.isNullOrEmpty()) {
                            url = pageInfo.mediaInfo?.streamURL
                        }
                        if (url.isNullOrEmpty() || !url.startsWith("http")) {
                            pageInfo.pageURL?.let {
                                context.openBrowser(it)
                            }
                        } else {
                            context.openActivity<VideoActivity>(
                                // Fix Android P blocking http request
                                "url" to url.replace("http://", "https://")
                            )
                        }
                    }
                    "article" -> {
                        pageInfo.pageURL?.let {
                            context.openBrowser(it)
                        }
                    }
                }
            }
        }
        story_container.setOnClickListener {
            data?.let {
                it as? Status
            }?.let {
                it.pageInfo?.pageURL
            }?.let { link ->
                (context?.let {
                    it as? LifecycleOwner
                }?.lifecycle?.coroutineScope ?: GlobalScope).launch {
                    kotlin.runCatching {
                        Api.storyVideoLink(link).storyDataObject?.stream?.url?.let {
                            context.openActivity<VideoActivity>(
                                // Fix Android P blocking http request
                                "url" to it.replace("http://", "https://")
                            )
                        }
                    }.onFailure {
                        context.openBrowser(link)
                    }
                }
            }
        }
        setOnClickListener {
            showStatusDetail()
        }
        hotflow_more_button.setOnClickListener {
            data?.let {
                it as? Comment
            }?.let {
                context.openActivity<HotflowChildActivity>(
                    "item" to it
                )
            }
        }
    }

    fun updateContent(content: String) {
        status_content.text = fromHtml(content, status_content) { url ->
            linkClicked.invoke(url)
        }
    }

    private fun showStatusDetail() {
        if (isTextSelectionEnabled) {
            return
        }
        data?.let {
            it as? Status
        }?.let {
            context.openActivity<StatusActivity>(*StatusActivity.bundle(it))
        }
    }

    private fun onImageClicked(
        item: Pic,
        items: List<Pic>
    ) {
        context.openActivity<ImageActivity>(*ImageActivity.bundle(items.map {
            ImageData(
                placeHolder = it.url ?: "",
                source = it.large?.url ?: "",
                width = it.large?.geo?.width ?: 0L,
                height = it.large?.geo?.height ?: 0L
            )
        }, items.indexOf(item)))
    }
}
