package moe.tlaster.weipo.controls

import android.content.Context
import android.util.AttributeSet
import android.view.View
import androidx.constraintlayout.widget.ConstraintLayout
import com.bumptech.glide.request.RequestOptions
import kotlinx.android.synthetic.main.control_user.view.*
import moe.tlaster.weipo.R
import moe.tlaster.weipo.activity.UserActivity
import moe.tlaster.weipo.common.extensions.inflate
import moe.tlaster.weipo.common.extensions.load
import moe.tlaster.weipo.common.extensions.openActivity
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
        value.profileImageURL?.let {
            user_avatar.load(it) {
                apply(RequestOptions.circleCropTransform())
            }
        }
        value.screenName?.let {
            user_name.text = it
        }
        value.verifiedReason?.let {
            user_verify.text = it
        }
        user_verify.visibility = if (value.verified == true) {
            View.VISIBLE
        } else {
            View.GONE
        }
        value.description?.let {
            user_desc.text = it
        }
    }

    init {
        inflate(R.layout.control_user)
        setOnClickListener {
            context.openActivity<UserActivity>(*UserActivity.bundle(id = user?.id, name = user?.screenName))
        }
    }
}