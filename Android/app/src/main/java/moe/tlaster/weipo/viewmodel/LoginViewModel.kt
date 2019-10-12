package moe.tlaster.weipo.viewmodel

import android.webkit.CookieManager
import kotlinx.serialization.ImplicitReflectionSerializer
import kotlinx.serialization.UnstableDefault
import kotlinx.serialization.json.Json
import kotlinx.serialization.stringify
import moe.tlaster.weipo.common.Settings

class LoginViewModel : ViewModelBase() {
    var loginCompleted: (() -> Unit)? = null

    @UnstableDefault
    @ImplicitReflectionSerializer
    val onPageFinished = { url: String? ->
        val cookies = CookieManager.getInstance().getCookie(url).split(';').map {
            val res = it.split('=')
            res[0].trim() to res[1].trim()
        }.toMap()
        if (cookies.containsKey("MLOGIN") && cookies["MLOGIN"] == "1") {
            Settings.set("user_cookie", Json.nonstrict.stringify(cookies))
            loginCompleted?.invoke()
        }
    }
}