package moe.tlaster.weipo.common.extensions

import android.content.res.Resources
import android.util.TypedValue
import androidx.fragment.app.Fragment
import androidx.fragment.app.FragmentActivity
import androidx.lifecycle.ViewModel
import androidx.lifecycle.ViewModelProviders
import kotlinx.coroutines.*


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

inline fun <reified T: ViewModel> Fragment.viewModel(): T {
    return this.activity?.let { ViewModelProviders.of(it) }?.get(T::class.java)!!
}

inline fun <reified T: ViewModel> FragmentActivity.viewModel(): T {
    return ViewModelProviders.of(this)[T::class.java]
}

inline fun async(noinline block: suspend () -> Unit): suspend () -> Unit = block

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