package moe.tlaster.weipo.common.extensions

import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup


fun ViewGroup.inflate(layout: Int): View =
    LayoutInflater.from(context).inflate(layout, this, true)
