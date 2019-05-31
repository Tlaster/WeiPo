plugins {
    id("com.android.application")
    kotlin("android")
    kotlin("android.extensions")
}
android {

    compileSdkVersion(appConfig.compileSdkVersion)
    defaultConfig {
        applicationId = appConfig.appId
        minSdkVersion(appConfig.minSdkVersion)
        targetSdkVersion(appConfig.targetSdkVersion)
        versionCode = appConfig.versionCode
        versionName = appConfig.versionName
        testInstrumentationRunner = "androidx.test.runner.AndroidJUnitRunner"
    }

    buildTypes {
        getByName("release") {
            isMinifyEnabled = false
            proguardFiles(getDefaultProguardFile("proguard-android-optimize.txt"), "proguard-rules.pro")
        }
    }
}

dependencies {
    implementation(kotlin("stdlib-jdk8", appConfig.kotlinVersion))
    implementation("moe.tlaster:shiba:0.3.+")
    implementation("org.jetbrains.kotlinx:kotlinx-coroutines-android:1.1.0")
    implementation("com.facebook.fresco:fresco:1.13.0")
    implementation("androidx.appcompat:appcompat:1.1.0-alpha05")
    implementation("androidx.core:core-ktx:1.2.0-alpha01")
    implementation("com.fasterxml.jackson.core:jackson-databind:2.9.+")
    implementation("androidx.constraintlayout:constraintlayout:2.0.0-beta1")
    testImplementation("junit:junit:4.12")
    androidTestImplementation("androidx.test:runner:1.2.0-beta01")
    androidTestImplementation("androidx.test.espresso:espresso-core:3.2.0-beta01")
}
