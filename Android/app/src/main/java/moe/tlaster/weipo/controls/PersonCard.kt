package moe.tlaster.weipo.controls

import android.content.Context
import android.util.AttributeSet
import androidx.constraintlayout.widget.ConstraintLayout
import androidx.databinding.BindingAdapter
import androidx.lifecycle.MutableLiveData
import moe.tlaster.weipo.R
import moe.tlaster.weipo.common.bindingInflate
import moe.tlaster.weipo.databinding.ControlPersonCardBinding

class PersonCard : ConstraintLayout {

    internal data class Data(
        var avatar: MutableLiveData<String> = MutableLiveData(),
        var title: MutableLiveData<String> = MutableLiveData(),
        var subTitle: MutableLiveData<String> = MutableLiveData()
    )

    internal val data by lazy {
        Data()
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
        bindingInflate<ControlPersonCardBinding>(R.layout.control_person_card)
            .also {
                it.data = data
            }
    }
}

@BindingAdapter("avatar")
fun setAvatar(personCard: PersonCard, avatar: String?) {
    personCard.data.avatar.value = avatar
}

@BindingAdapter("title")
fun setTitle(personCard: PersonCard, title: String?) {
    personCard.data.title.value = title
}

@BindingAdapter("subTitle")
fun setSubTitle(personCard: PersonCard, subTitle: String?) {
    personCard.data.subTitle.value = subTitle
}