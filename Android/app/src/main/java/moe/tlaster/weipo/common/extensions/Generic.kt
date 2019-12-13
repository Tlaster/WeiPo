package moe.tlaster.weipo.common.extensions

import android.content.Context
import android.content.res.Resources
import android.net.Uri
import android.os.Build
import android.provider.OpenableColumns
import android.util.TypedValue
import android.view.View
import androidx.lifecycle.*
import androidx.lifecycle.Observer
import androidx.swiperefreshlayout.widget.SwipeRefreshLayout
import kotlinx.coroutines.*
import moe.tlaster.weipo.common.collection.IncrementalLoadingCollection
import org.ocpsoft.prettytime.PrettyTime
import java.io.File
import java.io.FileInputStream
import java.io.FileOutputStream
import java.text.DateFormat
import java.text.SimpleDateFormat
import java.util.*


fun ViewModel.runOnMainThread(action: () -> Unit) {
    viewModelScope.runOnMainThread(action)
}

fun LifecycleOwner.runOnMainThread(action: () -> Unit) {
    lifecycleScope.runOnMainThread(action)
}

fun View.runOnMainThread(action: () -> Unit) {
    (context?.let {
        it as? LifecycleOwner
    }?.lifecycleScope ?: GlobalScope).runOnMainThread(action)
}

fun runOnMainThread(action: () -> Unit) {
    GlobalScope.runOnMainThread(action)
}

fun runOnIOThread(action: () -> Unit) {
    GlobalScope.runOnIOThread(action)
}

fun runOnDefaultThread(action: () -> Unit) {
    GlobalScope.runOnDefaultThread(action)
}

fun CoroutineScope.runOnMainThread(action: () -> Unit) {
    launch {
        withContext(Dispatchers.Main) {
            action.invoke()
        }
    }
}

fun CoroutineScope.runOnIOThread(action: () -> Unit) {
    launch {
        withContext(Dispatchers.IO) {
            action.invoke()
        }
    }
}

fun CoroutineScope.runOnDefaultThread(action: () -> Unit) {
    launch {
        withContext(Dispatchers.Default) {
            action.invoke()
        }
    }
}

inline fun <reified T: ViewModel> factory(crossinline creator: () -> T): ViewModelProvider.Factory {
    return object : ViewModelProvider.Factory {
        override fun <T : ViewModel?> create(modelClass: Class<T>): T {
            return creator.invoke() as T
        }
    }
}

inline fun ViewModel.async(noinline block: suspend () -> Unit) {
    viewModelScope.launch {
        block.invoke()
    }
}

fun <T> (suspend () -> T).fireAndForgot() {
    GlobalScope.launch {
        this@fireAndForgot.invoke()
    }
}

internal val Number.dp: Float
    get() = TypedValue.applyDimension(
        TypedValue.COMPLEX_UNIT_DIP,
        this.toFloat(),
        Resources.getSystem().displayMetrics
    )

fun SwipeRefreshLayout.bindLoadingCollection(
    incrementalLoadingCollection: IncrementalLoadingCollection<*, *>,
    lifecycleOwner: LifecycleOwner
) {
    incrementalLoadingCollection.stateChanged.observe(lifecycleOwner, Observer { args ->
        isRefreshing = when {
            args == IncrementalLoadingCollection.CollectionState.Loading && !incrementalLoadingCollection.any() -> true
            args == IncrementalLoadingCollection.CollectionState.Completed -> false
            else -> false
        }
    })
    setOnRefreshListener {
        incrementalLoadingCollection.refresh()
    }
}

private fun getFileName(context: Context, uri: Uri): String {
    var result: String? = null
    if (uri.scheme == "content") {
        val cursor = context.contentResolver.query(uri, null, null, null, null)
        try {
            if (cursor != null && cursor.moveToFirst()) {
                result = cursor.getString(cursor.getColumnIndex(OpenableColumns.DISPLAY_NAME))
            }
        } finally {
            cursor!!.close()
        }
    }
    if (result == null) {
        result = uri.path
        val cut = result!!.lastIndexOf('/')
        if (cut != -1) {
            result = result.substring(cut + 1)
        }
    }
    return result
}

private fun getFile(context: Context, uri: Uri?): String? {
    if (uri == null) {
        return null
    }
    if ("file" == uri.scheme) {
        return uri.path
    }
    // TODO: Different image file with the same name
    val temp = File(context.cacheDir, getFileName(context, uri))
    try {
        val `in` = context.contentResolver.openInputStream(uri) as FileInputStream?
        val out = FileOutputStream(temp)
        val inChannel = `in`!!.channel
        val outChannel = out.channel
        inChannel.transferTo(0, inChannel.size(), outChannel)
        `in`.close()
        out.close()
    } catch (e: Exception) {
    }

    return Uri.fromFile(temp).path
}

fun Uri.getFilePath(context: Context): String? {
    return getFile(context, this)
}

private val SECOND_MILLIS = 1000
private val MINUTE_MILLIS = 60 * SECOND_MILLIS
private val HOUR_MILLIS = 60 * MINUTE_MILLIS
private val DAY_MILLIS = 24 * HOUR_MILLIS

private val prettyTime = PrettyTime(if (Build.VERSION.SDK_INT > Build.VERSION_CODES.N) {
    Resources.getSystem().configuration.locales[0]
} else {
    Resources.getSystem().configuration.locale
})

private val simpleDateFormat = SimpleDateFormat("EEE MMM dd HH:mm:ss Z yyyy", Locale.US)

fun String.toHumanizedTime(): String {
    kotlin.runCatching {
        val date = simpleDateFormat.parse(this)
        return if (System.currentTimeMillis() - date.time > 3 * HOUR_MILLIS) {
            DateFormat.getDateTimeInstance().format(date)
        } else {
            prettyTime.format(date)
        }
    }.onFailure {
    }
    return this
}