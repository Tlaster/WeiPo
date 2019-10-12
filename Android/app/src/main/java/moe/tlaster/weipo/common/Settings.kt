package moe.tlaster.weipo.common

import android.content.Context
import androidx.core.content.edit
import moe.tlaster.weipo.appContext


object Settings {
    private val NAME = "weipo"
    fun <T> get(name: String, defaultValue: T): T {
        return appContext.getSharedPreferences(NAME, Context.MODE_PRIVATE).let {
            when (defaultValue) {
                is String -> it.getString(name, defaultValue) as T
                is Float -> it.getFloat(name, defaultValue) as T
                is Boolean -> it.getBoolean(name, defaultValue) as T
                is Int -> it.getInt(name, defaultValue) as T
                is Long -> it.getLong(name, defaultValue) as T
                else -> throw NotImplementedError()
            }
        }
    }

    fun <T> set(name: String, value: T) {
        appContext.getSharedPreferences(NAME, Context.MODE_PRIVATE).edit {
            when (value) {
                is String -> putString(name, value)
                is Float -> putFloat(name, value)
                is Boolean -> putBoolean(name, value)
                is Int -> putInt(name, value)
                is Long -> putLong(name, value)
                else -> throw NotImplementedError()
            }
        }
    }
}