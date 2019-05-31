package moe.tlaster.weipo.shiba.extensionExecutors

import android.graphics.Color
import moe.tlaster.shiba.IShibaContext
import moe.tlaster.shiba.extensionExecutor.IMutableExtensionExecutor
import moe.tlaster.shiba.type.ShibaExtension

class ResExecutor(override val name: String = "res") : IMutableExtensionExecutor {
    override fun provideValue(context: IShibaContext?, extension: ShibaExtension): Any? {
        return Color.TRANSPARENT
    }
}