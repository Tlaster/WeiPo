package moe.tlaster.weipo.common.extensions

import android.graphics.drawable.Drawable
import android.widget.ImageView
import com.bumptech.glide.Glide
import com.bumptech.glide.RequestBuilder
import com.bumptech.glide.load.resource.drawable.DrawableTransitionOptions.withCrossFade
import com.bumptech.glide.request.transition.DrawableCrossFadeFactory
import java.io.File


var factory = DrawableCrossFadeFactory.Builder().setCrossFadeEnabled(true).build()


fun ImageView.load(source: String, customAction:(RequestBuilder<Drawable>.() -> Unit)? = null) {
    Glide.with(this).load(source).transition(withCrossFade(factory)).centerCrop().also {
        customAction?.invoke(it)
    }.into(this)
}

fun ImageView.load(source: File, customAction:(RequestBuilder<Drawable>.() -> Unit)? = null) {
    Glide.with(this).load(source).transition(withCrossFade(factory)).centerCrop().also {
        customAction?.invoke(it)
    }.into(this)
}
