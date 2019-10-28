package moe.tlaster.weipo.controls

import android.content.Context
import android.graphics.Color
import android.graphics.drawable.Drawable
import android.os.Build
import android.text.Html
import android.text.SpannableStringBuilder
import android.text.method.LinkMovementMethod
import android.text.style.URLSpan
import android.util.AttributeSet
import android.view.View
import android.widget.LinearLayout
import androidx.core.text.HtmlCompat
import androidx.core.text.set
import androidx.core.view.updateMarginsRelative
import androidx.core.view.updatePaddingRelative
import kotlinx.android.synthetic.main.control_status.view.*
import moe.tlaster.weipo.R
import moe.tlaster.weipo.common.HtmlHttpImageGetter
import moe.tlaster.weipo.common.adapter.AutoAdapter
import moe.tlaster.weipo.common.adapter.ItemSelector
import moe.tlaster.weipo.common.extensions.dp
import moe.tlaster.weipo.common.extensions.updateItemsSource
import moe.tlaster.weipo.common.extensions.inflate
import moe.tlaster.weipo.common.fromHtml
import moe.tlaster.weipo.services.models.Pic
import moe.tlaster.weipo.services.models.Status

class StatusView : LinearLayout {

    private val linkClicked: ((url: String) -> Unit)? = null
    private lateinit var repostView: StatusView
    var status: Status? = null
        set(value) {
            field = value
            onStatusChanged(value)
        }

    private fun onStatusChanged(value: Status?) {
        if (value?.title == null) {
            View.GONE
        } else {
            View.VISIBLE
        }.also {
            status_title.visibility = it
            status_title_divider.visibility = it
            status_title.text = value?.title?.text ?: ""
        }
        value?.user?.profileImageURL?.also {
            status_person.avatar = it
        }
        value?.user?.screenName?.also {
            status_person.title = it
        }
        value?.createdAt?.also {
            status_person.subTitle = it
        }
        value?.text?.also { html ->
            status_content.text = fromHtml(html, status_content) { url ->
                linkClicked?.invoke(url)
            }
        }
        value?.pics?.also {
            status_image.updateItemsSource(it)
        }
        if (value?.retweetedStatus != null) {
            if (!this::repostView.isInitialized) {
                repostView = StatusView(context)
                repostView.showActions = false
                repostView.layoutParams?.let {
                    it as MarginLayoutParams
                }?.also {
                    it.updateMarginsRelative(start = 8.dp.toInt(), end = 8.dp.toInt())
                }
                repostView.setBackgroundColor(Color.argb(32, 0, 0, 0))
                repost_container.addView(repostView)
            }
            repostView.status = value.retweetedStatus
        }
        repost_container.visibility = if (value?.retweetedStatus == null) {
            View.GONE
        } else {
            View.VISIBLE
        }
        repost_count.text = if (value?.repostsCount != null && value.repostsCount > 0) {
            value.repostsCount.toString()
        } else {
            ""
        }
        comment_count.text = if (value?.commentsCount != null && value.commentsCount > 0) {
            value.commentsCount.toString()
        } else {
            ""
        }
        like_count.text = if (value?.attitudesCount != null && value.attitudesCount > 0) {
            value.attitudesCount.toString()
        } else {
            ""
        }
    }

    var showActions = false
        set(value) {
            field = value
            action_container?.visibility = if (value) {
                View.VISIBLE
            } else {
                View.GONE
            }
        }

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
            setImage(R.id.image) {
                it.url ?: ""
            }
        }
        status_content.movementMethod = LinkMovementMethod.getInstance()
        updatePaddingRelative(bottom = 8.dp.toInt())
    }
}
