package moe.tlaster.weipo.common

import android.graphics.drawable.Drawable
import android.text.SpannableStringBuilder
import android.text.Spanned
import android.text.style.URLSpan
import android.view.View
import android.widget.TextView
import androidx.core.text.HtmlCompat
import androidx.core.text.set
import kotlinx.android.synthetic.main.control_status.view.*


class URLSpanEx(url: String?, private val linkClicked: (url: String) -> Unit) : URLSpan(url) {
    override fun onClick(widget: View) {
        linkClicked.invoke(url)
    }
}

fun fromHtml(html: String, textView: TextView, linkClicked: (url: String) -> Unit): Spanned {
    return HtmlCompat.fromHtml(html, HtmlCompat.FROM_HTML_MODE_COMPACT, object : HtmlHttpImageGetter(textView) {
        override fun getDrawable(source: String): Drawable {
            return super.getDrawable("https:$source")
        }
    }, null).let { spanned ->
        if (spanned is SpannableStringBuilder) {
            spanned.getSpans(0, html.length, URLSpan::class.java)?.forEach { span ->
                val start = spanned.getSpanStart(span)
                val end = spanned.getSpanEnd(span)
                spanned.removeSpan(span)
                spanned[start, end] = URLSpanEx(span.url) { url ->
                    linkClicked.invoke(url)
                }
            }
        }
        spanned
    }
}
