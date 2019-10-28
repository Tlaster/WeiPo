import org.jetbrains.kotlin.gradle.tasks.KotlinCompile

plugins {
    id("com.android.application")
    kotlin("android")
    kotlin("android.extensions")
    id("kotlinx-serialization")
    kotlin("kapt")
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
            proguardFiles(
                    getDefaultProguardFile("proguard-android-optimize.txt"),
                    "proguard-rules.pro"
            )
        }
    }
    compileOptions {
        sourceCompatibility = JavaVersion.VERSION_1_8
        targetCompatibility = JavaVersion.VERSION_1_8
    }
    tasks.withType < KotlinCompile > {
        kotlinOptions.jvmTarget = "1.8"
    }
}

dependencies {
    implementation(kotlin("stdlib-jdk8", appConfig.kotlinVersion))
    implementation("org.jetbrains.kotlinx:kotlinx-coroutines-android:1.3.0")
    implementation("org.jetbrains.kotlinx:kotlinx-serialization-runtime:0.13.0")

    implementation("androidx.swiperefreshlayout:swiperefreshlayout:1.1.0-alpha03")
    implementation("androidx.appcompat:appcompat:1.1.0")
    implementation("com.google.android.material:material:1.2.0-alpha01")
    implementation("androidx.core:core-ktx:1.1.0")
    implementation("androidx.constraintlayout:constraintlayout:2.0.0-beta3")
    implementation("androidx.navigation:navigation-fragment:2.1.0")
    implementation("androidx.navigation:navigation-ui:2.1.0")
    implementation("androidx.lifecycle:lifecycle-extensions:2.1.0")
    implementation("androidx.navigation:navigation-fragment-ktx:2.1.0")
    implementation("androidx.navigation:navigation-ui-ktx:2.1.0")

    implementation("com.github.kittinunf.fuel:fuel:${appConfig.dependencyVersion.fuel}")
    implementation("com.github.kittinunf.fuel:fuel-kotlinx-serialization:${appConfig.dependencyVersion.fuel}")
    implementation("com.github.kittinunf.fuel:fuel-coroutines:${appConfig.dependencyVersion.fuel}")


    implementation("com.github.bumptech.glide:glide:${appConfig.dependencyVersion.glide}")
    kapt("com.github.bumptech.glide:compiler:${appConfig.dependencyVersion.glide}")

    implementation("org.jsoup:jsoup:1.12.1")

    implementation("androidx.compose:compose-runtime:${appConfig.dependencyVersion.compose}")
    kapt("androidx.compose:compose-compiler:${appConfig.dependencyVersion.compose}")
    implementation("androidx.ui:ui-core:${appConfig.dependencyVersion.compose}")
    implementation("androidx.ui:ui-layout:${appConfig.dependencyVersion.compose}")
    implementation("androidx.ui:ui-framework:${appConfig.dependencyVersion.compose}")
    implementation("androidx.ui:ui-material:${appConfig.dependencyVersion.compose}")
    implementation("androidx.ui:ui-foundation:${appConfig.dependencyVersion.compose}")
    implementation("androidx.ui:ui-text:${appConfig.dependencyVersion.compose}")
    implementation("androidx.ui:ui-android-text:${appConfig.dependencyVersion.compose}")
    implementation("androidx.ui:ui-animation:${appConfig.dependencyVersion.compose}")
    implementation("androidx.ui:ui-animation-core:${appConfig.dependencyVersion.compose}")
    implementation("androidx.ui:ui-foundation:${appConfig.dependencyVersion.compose}")
    implementation("androidx.ui:ui-platform:${appConfig.dependencyVersion.compose}")
    implementation("androidx.ui:ui-tooling:${appConfig.dependencyVersion.compose}")
    testImplementation("androidx.ui:ui-test:${appConfig.dependencyVersion.compose}")

    testImplementation("junit:junit:4.12")
    androidTestImplementation("androidx.test:runner:1.2.0")
    androidTestImplementation("androidx.test.espresso:espresso-core:3.2.0")
}
