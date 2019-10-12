package moe.tlaster.weipo.common

import android.content.res.Resources
import android.util.TypedValue


internal val Number.dp: Float
    get() = TypedValue.applyDimension(
        TypedValue.COMPLEX_UNIT_DIP,
        this.toFloat(),
        Resources.getSystem().displayMetrics
    )