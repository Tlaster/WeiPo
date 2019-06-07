package moe.tlaster.weipo.activity

import android.content.Context
import android.os.Bundle
import android.webkit.WebView
import android.webkit.WebViewClient
import androidx.appcompat.app.AppCompatActivity
import kotlinx.android.synthetic.main.activity_login.*
import moe.tlaster.weipo.R
import android.webkit.CookieManager
import androidx.core.content.edit
import com.fasterxml.jackson.databind.ObjectMapper
import moe.tlaster.weipo.common.start


class LoginActivity : AppCompatActivity() {

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_login)
        webView.settings.apply {
            javaScriptEnabled = true
        }
        webView.webViewClient = object : WebViewClient() {
            override fun onPageFinished(view: WebView?, url: String?) {
                super.onPageFinished(view, url)
                val cookies = CookieManager.getInstance().getCookie(url).split(';').map {
                    val res = it.split('=')
                    res[0].trim() to res[1].trim()
                }.toMap()
                if (cookies.containsKey("MLOGIN") && cookies["MLOGIN"] == "1") {
                    getSharedPreferences("Shiba", Context.MODE_PRIVATE).edit {
                        val json = ObjectMapper().writeValueAsString(cookies)
                        putString("usercookie" ,json)
                    }
                    view?.destroy()
                    start<TimelineActivity>()
                    finish()
                }
            }
        }
        webView.loadUrl("https://m.weibo.cn/login?backURL=https%253A%252F%252Fm.weibo.cn%252F")
    }
}
