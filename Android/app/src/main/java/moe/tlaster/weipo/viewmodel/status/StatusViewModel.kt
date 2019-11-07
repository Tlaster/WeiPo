package moe.tlaster.weipo.viewmodel.status

import androidx.lifecycle.ViewModel
import kotlinx.coroutines.GlobalScope
import kotlinx.coroutines.launch
import moe.tlaster.weipo.common.extensions.runOnMainThread
import moe.tlaster.weipo.services.Api
import moe.tlaster.weipo.services.models.Status

class StatusViewModel(
    private val item: Status,
    private val updateTextCallback: (String) -> Unit
) : ViewModel() {
    init {
        if (item.isLongText == true) {
            loadLongText()
        }
    }

    private fun loadLongText() {
        GlobalScope.launch {
            item.id?.toLongOrNull()?.let {
                Api.extend(it)
            }?.let {
                it.longTextContent
            }?.let {
                runOnMainThread {
                    updateTextCallback.invoke(it)
                }
            }
        }
    }
}