package moe.tlaster.weipo.common

import android.content.Context
import android.net.Uri
import android.text.SpannableString
import android.text.SpannableStringBuilder
import android.text.Spanned
import android.text.style.ImageSpan
import androidx.core.text.buildSpannedString
import org.jsoup.Jsoup
import org.jsoup.select.Elements

object Html {
    fun toSpanned(value: String, context: Context) : Spanned {
        val doc = Jsoup.parse(value)
        return render(doc.body().children())
    }

    private fun render(elements: Elements): Spanned {
        TODO()
    }
}