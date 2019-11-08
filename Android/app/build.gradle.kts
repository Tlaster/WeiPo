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
    androidExtensions {
        isExperimental = true
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
    implementation("androidx.lifecycle:lifecycle-extensions:2.1.0")

    implementation("com.github.kittinunf.fuel:fuel:${appConfig.dependencyVersion.fuel}")
    implementation("com.github.kittinunf.fuel:fuel-kotlinx-serialization:${appConfig.dependencyVersion.fuel}")
    implementation("com.github.kittinunf.fuel:fuel-coroutines:${appConfig.dependencyVersion.fuel}")

    implementation("com.github.bumptech.glide:glide:${appConfig.dependencyVersion.glide}")
    kapt("com.github.bumptech.glide:compiler:${appConfig.dependencyVersion.glide}")

    implementation("org.ocpsoft.prettytime:prettytime:4.0.1.Final")

    implementation("com.github.piasy:BigImageViewer:${appConfig.dependencyVersion.bigImageViewer}")
    implementation("com.github.piasy:GlideImageLoader:${appConfig.dependencyVersion.bigImageViewer}")
    implementation("com.github.piasy:ProgressPieIndicator:${appConfig.dependencyVersion.bigImageViewer}")
    implementation("com.github.piasy:GlideImageViewFactory:${appConfig.dependencyVersion.bigImageViewer}")

    implementation("com.google.android.exoplayer:exoplayer:2.10.7")

    implementation("com.hold1:keyboardheightprovider:0.0.9")

    testImplementation("junit:junit:4.12")
    androidTestImplementation("androidx.test:runner:1.2.0")
    androidTestImplementation("androidx.test.espresso:espresso-core:3.2.0")
}
