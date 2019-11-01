package moe.tlaster.weipo.controls

import android.content.Context
import android.util.AttributeSet
import androidx.constraintlayout.widget.ConstraintLayout
import moe.tlaster.weipo.R
import moe.tlaster.weipo.common.extensions.inflate
import moe.tlaster.weipo.services.models.User

class UserCard : ConstraintLayout {
    constructor(context: Context?) : super(context)
    constructor(context: Context?, attrs: AttributeSet?) : super(context, attrs)
    constructor(context: Context?, attrs: AttributeSet?, defStyleAttr: Int) : super(
        context,
        attrs,
        defStyleAttr
    )
    constructor(
        context: Context?,
        attrs: AttributeSet?,
        defStyleAttr: Int,
        defStyleRes: Int
    ) : super(context, attrs, defStyleAttr, defStyleRes)

    var user: User? = null
        set(value) {
            field = value
            onUserChanged(value)
        }

    private fun onUserChanged(value: User?) {
        if (value == null) {
            return
        }

    }

    init {
        inflate(R.layout.control_user)
    }
}