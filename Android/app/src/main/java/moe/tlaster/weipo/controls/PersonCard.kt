package moe.tlaster.weipo.controls

import android.content.Context
import android.util.AttributeSet
import androidx.constraintlayout.widget.ConstraintLayout
import com.bumptech.glide.request.RequestOptions
import kotlinx.android.synthetic.main.control_person_card.view.*
import moe.tlaster.weipo.R
import moe.tlaster.weipo.common.extensions.load
import moe.tlaster.weipo.common.inflate

class PersonCard : ConstraintLayout {
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
        inflate(R.layout.control_person_card)
    }

    var avatar: String = ""
        set(value) {
            field = value
            person_avatar.load(value) {
                apply(RequestOptions.circleCropTransform())
            }
        }
    var title: String = ""
        set(value) {
            field = value
            person_title.text = value
        }
    var subTitle: String = ""
        set(value) {
            field = value
            person_sub_title.text = value
        }
}