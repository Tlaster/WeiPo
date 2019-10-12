package moe.tlaster.weipo.controls

import android.content.Context
import android.util.AttributeSet
import android.widget.FrameLayout
import androidx.databinding.BindingAdapter
import moe.tlaster.weipo.R
import moe.tlaster.weipo.common.bindingInflate
import moe.tlaster.weipo.databinding.ControlStatusBinding
import moe.tlaster.weipo.services.models.Status

class StatusView : FrameLayout {

    private var binding: ControlStatusBinding
    var status: Status? = null
        set(value) {
            field = value
            binding.status = value
        }

    constructor(context: Context) : super(context)
    constructor(context: Context, attrs: AttributeSet?) : super(context, attrs)
    constructor(context: Context, attrs: AttributeSet?, defStyleAttr: Int) : super(
        context,
        attrs,
        defStyleAttr
    )

    constructor(
        context: Context,
        attrs: AttributeSet?,
        defStyleAttr: Int,
        defStyleRes: Int
    ) : super(context, attrs, defStyleAttr, defStyleRes)

    init {
        binding = bindingInflate<ControlStatusBinding>(R.layout.control_status).also {
            it.status = status
        }
    }
}

@BindingAdapter("status")
fun setStatus(statusView: StatusView, status: Status) {
    statusView.status = status
}
