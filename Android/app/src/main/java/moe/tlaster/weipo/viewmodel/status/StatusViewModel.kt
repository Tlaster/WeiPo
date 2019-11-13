package moe.tlaster.weipo.viewmodel.status

import androidx.lifecycle.ViewModel
import moe.tlaster.weipo.common.extensions.async
import moe.tlaster.weipo.common.extensions.runOnMainThread
import moe.tlaster.weipo.services.Api
import moe.tlaster.weipo.services.models.Status

class StatusViewModel(
    private val item: Status,
    private val updateTextCallback: (String) -> Unit
) : ViewModel() {
    fun init() {
        if (item.isLongText == true) {
            loadLongText()
        }
    }

    private fun loadLongText() {
        async {
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