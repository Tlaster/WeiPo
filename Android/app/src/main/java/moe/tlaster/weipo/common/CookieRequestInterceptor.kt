package moe.tlaster.weipo.common

import android.util.Log
import com.github.kittinunf.fuel.core.FoldableRequestInterceptor
import com.github.kittinunf.fuel.core.FoldableResponseInterceptor
import com.github.kittinunf.fuel.core.RequestTransformer
import com.github.kittinunf.fuel.core.ResponseTransformer
import kotlinx.serialization.ImplicitReflectionSerializer
import kotlinx.serialization.json.Json
import kotlinx.serialization.parseMap


object CookieRequestInterceptor : FoldableRequestInterceptor {
    @ImplicitReflectionSerializer
    override fun invoke(next: RequestTransformer): RequestTransformer {
        return { request ->
            Settings.get("user_cookie", "").takeIf {
                it.isNotEmpty()
            }?.let {
                request.header("Cookie", Json.nonstrict.parseMap<String, String>(it).map {
                    "${it.key}=${it.value}"
                }.joinToString(";"))
            }
            next(request)
        }
    }

}



object AndroidLogRequestInterceptor : FoldableRequestInterceptor {
    override fun invoke(next: RequestTransformer): RequestTransformer {
        return { request ->
            Log.i("Furl Request", request.toString())
            next(request)
        }
    }
}


object AndroidLogResponseInterceptor : FoldableResponseInterceptor {
    override fun invoke(next: ResponseTransformer): ResponseTransformer {
        return { request, response ->
            Log.i("Furl Response", response.toString())
            next(request, response)
        }
    }
}