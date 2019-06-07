package moe.tlaster.weipo

import android.app.Application
import com.facebook.drawee.backends.pipeline.Fresco
import moe.tlaster.shiba.Shiba
import moe.tlaster.weipo.shiba.extensionExecutors.JSBindExecutor
import moe.tlaster.weipo.shiba.extensionExecutors.ResExecutor
import moe.tlaster.weipo.shiba.mappers.*



class App : Application() {
    override fun onCreate() {
        super.onCreate()
        Fresco.initialize(this)

        val script = resources.openRawResource(R.raw.bundle).let { inputStream ->
            ByteArray(inputStream.available()).also {
                inputStream.read(it)
            }
        }.let { String(it) }
        Shiba.apply {
            init(this@App)
            addExtensionExecutor(ResExecutor())
            addExtensionExecutor(JSBindExecutor())
            addRenderer("img", ImgMapper())
            addRenderer("roundImg", RoundImgMapper())
            addRenderer("weiboText", WeiboTextMapper())
            addRenderer("items", ItemsMapper())
            addRenderer("optional", OptionalMapper())
            addConverter("weiboTextConverter") {
                it.firstOrNull()
            }
            configuration.scriptRuntime.execute(script)
        }
    }
}