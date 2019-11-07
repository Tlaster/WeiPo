package moe.tlaster.weipo

import android.app.Application
import android.content.Context
import com.github.kittinunf.fuel.core.FuelManager
import com.github.piasy.biv.BigImageViewer
import com.github.piasy.biv.loader.glide.GlideImageLoader
import moe.tlaster.weipo.common.AndroidLogRequestInterceptor
import moe.tlaster.weipo.common.AndroidLogResponseInterceptor
import moe.tlaster.weipo.common.CookieRequestInterceptor

lateinit var appContext: Context

class App : Application() {
    override fun onCreate() {
        super.onCreate()
        appContext = applicationContext
        FuelManager.instance.apply {
            if (BuildConfig.DEBUG) {
                addRequestInterceptor(AndroidLogRequestInterceptor)
                addResponseInterceptor(AndroidLogResponseInterceptor)
            }
            addRequestInterceptor(CookieRequestInterceptor)
        }
        BigImageViewer.initialize(GlideImageLoader.with(appContext))
    }
}