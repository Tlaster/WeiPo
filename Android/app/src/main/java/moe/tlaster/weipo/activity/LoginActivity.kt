package moe.tlaster.weipo.activity

import android.os.Bundle
import android.webkit.CookieManager
import kotlinx.android.synthetic.main.activity_login.*
import kotlinx.serialization.ImplicitReflectionSerializer
import kotlinx.serialization.json.Json
import kotlinx.serialization.stringify
import moe.tlaster.weipo.R
import moe.tlaster.weipo.common.Settings
import moe.tlaster.weipo.common.extensions.onPageStarted
import moe.tlaster.weipo.common.extensions.source
import moe.tlaster.weipo.common.extensions.openActivity

class LoginActivity : BaseActivity() {
    override val layoutId: Int
        get() = R.layout.activity_login

    @ImplicitReflectionSerializer
    override fun onPostCreate(savedInstanceState: Bundle?) {
        super.onPostCreate(savedInstanceState)
        webView.source = "https://m.weibo.cn/login?backURL=https%253A%252F%252Fm.weibo.cn%252F"
        webView.onPageStarted {
            if (!isFinishing) {
                val cookies = CookieManager.getInstance().getCookie(it).split(';').map {
                    val res = it.split('=')
                    res[0].trim() to res[1].trim()
                }.toMap()
                if (cookies.containsKey("MLOGIN") && cookies["MLOGIN"] == "1") {
                    Settings.set("user_cookie", Json.nonstrict.stringify(cookies))
                    openActivity<HomeActivity>()
                    finish()
                }
            }
        }
    }
}
