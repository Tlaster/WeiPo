package moe.tlaster.weipo.common

import android.app.Activity
import android.content.Context
import android.content.Intent
import androidx.fragment.app.Fragment

inline fun <reified T : Activity> Context.openActivity(intent: Intent? = null) {
    startActivity(Intent(this, T::class.java).also {
        if (intent != null) {
            it.putExtras(intent)
        }
    })
}

inline fun <reified T : Activity> Activity.openActivityForResult(
    requestCode: Int,
    intent: Intent? = null
) {
    startActivityForResult(Intent(this, T::class.java).also {
        if (intent != null) {
            it.putExtras(intent)
        }
    }, requestCode)
}

inline fun <reified T : Activity> Fragment.openActivityForResult(
    requestCode: Int,
    intent: Intent? = null
) {
    startActivityForResult(Intent(context, T::class.java).also {
        if (intent != null) {
            it.putExtras(intent)
        }
    }, requestCode)
}

