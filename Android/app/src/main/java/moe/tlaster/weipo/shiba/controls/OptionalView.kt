package moe.tlaster.weipo.shiba.controls

import android.content.Context
import android.util.AttributeSet
import android.widget.FrameLayout
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.Job
import kotlinx.coroutines.withContext
import moe.tlaster.shiba.*
import moe.tlaster.shiba.dataBinding.ShibaBinding

class OptionalView @JvmOverloads constructor(
    context: Context, attrs: AttributeSet? = null, defStyleAttr: Int = 0
) : FrameLayout(context, attrs, defStyleAttr), INotifyPropertyChanged {
    private var binding: ShibaBinding? = null
    override var propertyChanged: Event<String> = Event()
    private var cachedChild: ShibaHost? = null
    var creator: String? = null
        set(value) {
            field = value
            cachedChild?.let {
                it.creator = value
            }
        }
    var selector: String? = null
    @get:Binding(name = "dataContext")
    @set:Binding(name = "dataContext")
    var dataContext: Any? = null
        set(value) {
            field = value
            propertyChanged.invoke(this, "dataContext")
            checkIfNeedUpdate()
        }

    internal fun finishMapping() {
        checkIfNeedUpdate()
    }

    private fun checkIfNeedUpdate() {
        val currentCreator = creator
        val currentSelector = selector
        val currentDataContext = dataContext
        if (currentSelector == null || currentCreator == null) {
            return
        }
        if (currentDataContext == null) {
            if (childCount > 0) {
            }
            return
        }

        val selectorResult = Shiba.configuration.scriptRuntime.callFunction(currentSelector, currentDataContext)
        if (selectorResult is Boolean) {
            if (selectorResult) {
                if (childCount == 0) {
                    if (cachedChild == null && creator != null) {
                        cachedChild = ShibaHost(context).also { view ->
                            view.creator = currentCreator
                        }
                    }
                    if (binding == null && cachedChild != null) {
                        this@OptionalView.binding = ShibaBinding("dataContext").also { binding ->
                            binding.targetView = cachedChild
                            binding.viewSetter = { view, value ->
                                if (view is ShibaHost) {
                                    view.dataContext = value
                                }
                            }
                            binding.source = this@OptionalView
                        }
                    }
                    binding?.setValueToView()
                    addView(cachedChild)
                }
            } else {
                if (childCount > 0) {
                    removeView(cachedChild)
                }
            }
        }
//        if (::currentJob.isInitialized && currentJob.isActive) {
//            currentJob.cancel()
//        }
//        currentJob = GlobalScope.launch {
//            updateImpl(currentCreator, currentSelector, currentDataContext)
//        }
    }

    private suspend fun updateImpl(
        currentCreator: String,
        currentSelector: String,
        currentDataContext: Any
    ) = withContext(Dispatchers.Default) {
        val selectorResult = Shiba.configuration.scriptRuntime.callFunction(currentSelector, currentDataContext)
        if (selectorResult is Boolean) {
            if (selectorResult) {
                if (childCount == 0) {
                    if (cachedChild == null && creator != null) {
                        cachedChild = ShibaHost(context).also { view ->
                            view.creator = currentCreator
                        }
                    }
                    if (binding == null && cachedChild != null) {
                        this@OptionalView.binding = ShibaBinding("dataContext").also { binding ->
                            binding.targetView = cachedChild
                            binding.viewSetter = { view, value ->
                                if (view is ShibaHost) {
                                    view.dataContext = value
                                }
                            }
                            binding.source = this@OptionalView
                        }
                    }
                    withContext(Dispatchers.Main) {
                        binding?.setValueToView()
                        addView(cachedChild)
                    }
                }
            } else {
                if (childCount > 0) {
                    withContext(Dispatchers.Main) {
                        removeView(cachedChild)
                    }
                }
            }
        }
    }

    override fun removeAllViews() {
        super.removeAllViews()
        binding?.release()
        binding = null
    }
}