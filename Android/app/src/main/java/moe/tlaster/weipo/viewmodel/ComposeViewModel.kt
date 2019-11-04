package moe.tlaster.weipo.viewmodel

import android.net.Uri
import androidx.lifecycle.ViewModel
import moe.tlaster.weipo.common.collection.ObservableCollection

class ComposeViewModel : ViewModel() {
    val images = ObservableCollection<Uri>()
}