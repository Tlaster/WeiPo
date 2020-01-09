
buildscript {
    repositories {
        google()
        jcenter()
    }
    dependencies {
        classpath("com.android.tools.build:gradle:4.0.0-alpha08")
        classpath(kotlin("gradle-plugin", appConfig.kotlinVersion))
        classpath(kotlin("serialization", appConfig.kotlinVersion))
    }
}

allprojects {
    repositories {
        maven("http://dl.bintray.com/piasy/maven")
        google()
        mavenCentral()
        jcenter()
        maven("https://jitpack.io")
        maven("https://dl.bintray.com/crysis21/Android")
    }
}
