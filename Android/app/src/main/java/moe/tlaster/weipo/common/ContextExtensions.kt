package moe.tlaster.weipo.common

import android.app.Activity
import android.content.Context
import android.content.Intent

inline fun <reified T: Activity> Context.start(intent: Intent? = null) {
    startActivity(Intent(this, T::class.java).apply {
        intent?.let {
            putExtras(it)
        }
    })
}