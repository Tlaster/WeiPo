package moe.tlaster.weipo

import android.app.Application
import android.app.NotificationChannel
import android.app.NotificationManager
import android.content.Context
import android.os.Build
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
        createNotificationChannel()
    }

    private fun createNotificationChannel() {
        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.O) {
            val name = getString(R.string.channel_name)
            val channelId = getString(R.string.channel_id)
            val descriptionText = getString(R.string.channel_description)
            val importance = NotificationManager.IMPORTANCE_DEFAULT
            val channel = NotificationChannel(channelId, name, importance).apply {
                description = descriptionText
            }
            val notificationManager: NotificationManager =
                getSystemService(Context.NOTIFICATION_SERVICE) as NotificationManager
            notificationManager.createNotificationChannel(channel)
        }
    }

}