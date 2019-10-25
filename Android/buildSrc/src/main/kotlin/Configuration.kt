import org.gradle.api.Project
class Configuration(project: Project) {
    val appId = "moe.tlaster.weipo"
    val kotlinVersion = "1.3.50"
    val minSdkVersion = 21
    val targetSdkVersion = 29
    val compileSdkVersion = 29
    val versionCode = project.getConfiguration("versionCode", 1)
    val versionName = project.getConfiguration("versionName", "1.0")
    
    val signKeyStore = project.getConfiguration("signKeyStore", "./key.jks")
    val signKeyStorePassword = project.getConfiguration("signKeyStorePassword", "password")
    val signKeyAlias = project.getConfiguration("signKeyAlias", "alias")
    val signKeyPassword = project.getConfiguration("signKeyPassword", "password")
    val dependencyVersion = DependencyVersion()
}
class DependencyVersion {
    val fuel = "2.2.1"
    val glide = "4.9.0"
    val compose = "0.1.0-dev01"
}

val Project.appConfig
    get() = Configuration(this)

inline fun <reified T: Any> Project.getConfiguration(name: String, defaultValue: T): T {
    return (if (project.hasProperty(name)) {
        val property = project.property(name)
        if (property == null) {
            defaultValue
        } else {
            when (defaultValue) {
                is String -> property
                is Boolean -> property.toString().toBoolean()
                is Int -> property.toString().toInt()
                is Double -> property.toString().toDouble()
                else -> property
            }
        }
    } else {
        defaultValue
    }) as T
}