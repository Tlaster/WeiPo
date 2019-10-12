package moe.tlaster.weipo.common

import android.view.LayoutInflater
import android.view.ViewGroup
import androidx.databinding.DataBindingUtil
import androidx.databinding.ViewDataBinding
import androidx.lifecycle.LifecycleOwner


inline fun <reified T : ViewDataBinding> ViewGroup.bindingInflate(layout: Int): T {
    val binding = DataBindingUtil.inflate<T>(LayoutInflater.from(context), layout, this, true)
    context?.let {
        it as? LifecycleOwner
    }?.let {
        binding.lifecycleOwner = it
    }
    return binding
}
