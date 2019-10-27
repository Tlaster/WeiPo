package moe.tlaster.weipo.common.extensions

import android.graphics.Bitmap
import android.webkit.WebView
import android.webkit.WebViewClient


var WebView.source: String
    get() {
        return url
    }
    set(value) {
        settings.javaScriptEnabled = true
        loadUrl(value)
    }

private val BINDINGWEBVIEWCLIENT_ID = -811

fun WebView.onPageFinished(callback: (url: String?) -> Unit) {
    if (getTag(BINDINGWEBVIEWCLIENT_ID) !is BindingWebViewClient) {
        BindingWebViewClient().let {
            webViewClient = it
            setTag(BINDINGWEBVIEWCLIENT_ID, it)
        }
    }
    getTag(BINDINGWEBVIEWCLIENT_ID)?.let {
        it as? BindingWebViewClient
    }?.let {
        it.pageFinished = callback
    }
}

fun WebView.onPageStarted(callback: (url: String?) -> Unit) {
    if (getTag(BINDINGWEBVIEWCLIENT_ID) !is BindingWebViewClient) {
        BindingWebViewClient().let {
            webViewClient = it
            setTag(BINDINGWEBVIEWCLIENT_ID, it)
        }
    }
    getTag(BINDINGWEBVIEWCLIENT_ID)?.let {
        it as? BindingWebViewClient
    }?.let {
        it.pageStarted = callback
    }
}


private class BindingWebViewClient : WebViewClient() {

    var pageFinished: ((url: String?) -> Unit)? = null
    var pageStarted: ((url: String?) -> Unit)? = null

    override fun onPageFinished(view: WebView?, url: String?) {
        pageFinished?.invoke(url)
    }

    override fun onPageStarted(view: WebView?, url: String?, favicon: Bitmap?) {
        pageStarted?.invoke(url)
    }
}