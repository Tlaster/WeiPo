package moe.tlaster.weipo.common.extensions

import kotlinx.serialization.ImplicitReflectionSerializer
import kotlinx.serialization.json.Json
import kotlinx.serialization.json.JsonObject

@ImplicitReflectionSerializer
inline fun <reified T : Any> JsonObject.toObject(): T {
    return Json.nonstrict.fromJson<T>(this)
}