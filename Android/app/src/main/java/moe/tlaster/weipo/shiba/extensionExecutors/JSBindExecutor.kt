package moe.tlaster.weipo.shiba.extensionExecutors

import moe.tlaster.shiba.IShibaContext
import moe.tlaster.shiba.Shiba
import moe.tlaster.shiba.converters.ShibaConverter
import moe.tlaster.shiba.dataBinding.ShibaBinding
import moe.tlaster.shiba.extensionExecutor.IExtensionExecutor
import moe.tlaster.shiba.type.ShibaExtension

private const val dataContextPath = "dataContext"
class JSBindExecutor : IExtensionExecutor {
    override val name: String
        get() = "jsbind"

    override fun provideValue(context: IShibaContext?, extension: ShibaExtension): Any? {
        val path = extension.value
        val bindingPath = dataContextPath
        return ShibaBinding(bindingPath).apply {
            source = context
            parameter = path
            converter = JSBindConverter
        }
}
}
object JSBindConverter : ShibaConverter() {
    override fun convert(value: Any?, parameter: Any?): Any? {
        if (parameter !is String || parameter.isEmpty()) {
            return value
        }

        var result: Any? = value
        parameter.split('.').forEach {
            result = Shiba.configuration.scriptRuntime.getProperty(result, it)
        }
        if (Shiba.configuration.scriptRuntime.isArray(result)) {
            return Shiba.configuration.scriptRuntime.toArray(result)
        }
        return result
    }
}