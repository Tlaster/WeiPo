package moe.tlaster.weipo.controls

import android.content.Context
import android.graphics.Color
import android.text.method.LinkMovementMethod
import android.util.AttributeSet
import android.view.View
import android.widget.LinearLayout
import androidx.core.view.updateMarginsRelative
import androidx.core.view.updatePaddingRelative
import kotlinx.android.synthetic.main.control_status.view.*
import moe.tlaster.weipo.R
import moe.tlaster.weipo.activity.ComposeActivity
import moe.tlaster.weipo.activity.UserActivity
import moe.tlaster.weipo.common.adapter.AutoAdapter
import moe.tlaster.weipo.common.adapter.ItemSelector
import moe.tlaster.weipo.common.extensions.*
import moe.tlaster.weipo.common.fromHtml
import moe.tlaster.weipo.common.openWeiboLink
import moe.tlaster.weipo.services.models.*
import moe.tlaster.weipo.viewmodel.ComposeViewModel

class StatusView : LinearLayout {
    private val linkClicked: ((url: String) -> Unit) = {
        openWeiboLink(context, it)
    }
    private lateinit var repostView: StatusView
    
    var data: Any? = null
        set(value) {
            field = value
            when(value) {
                is Status -> onStatusChanged(value)
                is Comment -> onCommentChanged(value)
                is Attitude -> onAttitudeChanged(value)
            }
        }

    private fun onAttitudeChanged(value: Attitude) {
        action_container.visibility = View.GONE
        status_title.visibility = View.GONE
        status_title_divider.visibility = View.GONE
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
        status_content.text = "Likes your tweet"
    }

    private fun setPersonCard(user: User)  {
        user.profileImageURL?.let {
            status_person.avatar = it
        }
        user.screenName?.let {
            status_person.title = it
        }
    }

    private fun onCommentChanged(value: Comment) {
        action_container.visibility = if (showActions) {
            View.VISIBLE
        } else {
            View.GONE
        }
        status_title.visibility = View.GONE
        status_title_divider.visibility = View.GONE
        repost_button.visibility = View.GONE
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
    }

    private fun onStatusChanged(value: Status) {
        action_container.visibility = if (showActions) {
            View.VISIBLE
        } else {
            View.GONE
        }
        if (value.title == null) {
            View.GONE
        } else {
            View.VISIBLE
        }.also {
            status_title.visibility = it
            status_title_divider.visibility = it
            status_title.text = value.title?.text ?: ""
        }
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
        value.pics.also {
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
            repostView.setBackgroundColor(Color.argb(32, 0, 0, 0))
            repost_container.addView(repostView)
        }
    }

    var showActions = true
        set(value) {
            field = value
            action_container?.visibility = if (value) {
                View.VISIBLE
            } else {
                View.GONE
            }
        }

    var showRepost = true
        set(value) {
            field = value
            repost_container?.visibility = if (value) {
                View.VISIBLE
            } else {
                View.GONE
            }
        }

    constructor(context: Context) : super(context)
    constructor(context: Context, attrs : AttributeSet?) : super(context, attrs)
    constructor(context: Context, attrs: AttributeSet?, defStyleAttr: Int) : super(
        context,
        attrs,
        defStyleAttr
    )

    init {
        orientation = VERTICAL
        inflate(R.layout.control_status)
        status_image.adapter = AutoAdapter<Pic>(ItemSelector(R.layout.item_image)).apply {
            setImage(R.id.image) {
                it.url ?: ""
            }
        }
        status_content.movementMethod = LinkMovementMethod.getInstance()
        updatePaddingRelative(bottom = 8.dp.toInt())
        status_person.setOnClickListener {
            data?.let {
                it as? IWithUser
            }?.let {
                it.user?.id
            }?.let {
                context.openActivity<UserActivity>(
                    "user_id" to it
                )
            }
        }
        repost_button.setOnClickListener { _ ->
            data?.let {
                it as ICanReply
            }?.let {
                context.openActivity<ComposeActivity>(*ComposeActivity.bundle(ComposeViewModel.ComposeType.Repost, it))
            }
        }
        comment_button.setOnClickListener { _ ->
            data?.let {
                it as ICanReply
            }?.let {
                context.openActivity<ComposeActivity>(*ComposeActivity.bundle(ComposeViewModel.ComposeType.Comment, it))
            }

        }
        like_button.setOnClickListener {

        }
    }
}
