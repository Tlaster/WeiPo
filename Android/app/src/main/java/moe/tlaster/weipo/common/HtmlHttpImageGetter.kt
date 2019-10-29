package moe.tlaster.weipo.common

import android.content.res.Resources
import android.graphics.Bitmap
import android.graphics.BitmapFactory
import android.graphics.Canvas
import android.graphics.drawable.BitmapDrawable
import android.graphics.drawable.Drawable
import android.os.AsyncTask
import android.text.Html.ImageGetter
import android.util.Log
import android.view.View
import android.widget.TextView

import java.io.ByteArrayInputStream
import java.io.ByteArrayOutputStream
import java.io.IOException
import java.io.InputStream
import java.lang.ref.WeakReference
import java.net.URI
import java.net.URL

open class HtmlHttpImageGetter : ImageGetter {
    private val container: TextView
    private var baseUri: URI? = null
    private var matchParentWidth: Boolean = false

    private var compressImage = false
    private var qualityImage = 50

    constructor(textView: TextView) {
        this.container = textView
        this.matchParentWidth = false
    }

    constructor(textView: TextView, baseUrl: String?) {
        this.container = textView
        if (baseUrl != null) {
            this.baseUri = URI.create(baseUrl)
        }
    }

    constructor(textView: TextView, baseUrl: String?, matchParentWidth: Boolean) {
        this.container = textView
        this.matchParentWidth = matchParentWidth
        if (baseUrl != null) {
            this.baseUri = URI.create(baseUrl)
        }
    }

    @JvmOverloads
    fun enableCompressImage(enable: Boolean, quality: Int = 50) {
        compressImage = enable
        qualityImage = quality
    }

    override fun getDrawable(source: String): Drawable {
        val urlDrawable = UrlDrawable()

        // get the actual source
        val asyncTask = ImageGetterAsyncTask(
            urlDrawable, this, container,
            matchParentWidth, compressImage, qualityImage
        )

        asyncTask.execute(source)

        // return reference to URLDrawable which will asynchronously load the image specified in the src tag
        return urlDrawable
    }

    /**
     * Static inner [AsyncTask] that keeps a [WeakReference] to the [UrlDrawable]
     * and [HtmlHttpImageGetter].
     *
     *
     * This way, if the AsyncTask has a longer life span than the UrlDrawable,
     * we won't leak the UrlDrawable or the HtmlRemoteImageGetter.
     */
    private class ImageGetterAsyncTask(
        d: UrlDrawable,
        imageGetter: HtmlHttpImageGetter,
        container: View,
        private val matchParentWidth: Boolean,
        private val compressImage: Boolean = false,
        private val qualityImage: Int = 50
    ) : AsyncTask<String, Void, Drawable>() {
        private val drawableReference: WeakReference<UrlDrawable> = WeakReference(d)
        private val imageGetterReference: WeakReference<HtmlHttpImageGetter> =
            WeakReference(imageGetter)
        private val containerReference: WeakReference<View> = WeakReference(container)
        private val resources: WeakReference<Resources> = WeakReference(container.resources)
        private var source: String? = null
        private var scale: Float = 0.toFloat()

        override fun doInBackground(vararg params: String): Drawable? {
            source = params[0]
            return resources.get()?.let {
                if (compressImage) {
                    fetchCompressedDrawable(it, source)
                } else {
                    fetchDrawable(it, source)
                }
            }
        }

        override fun onPostExecute(result: Drawable?) {
            if (result == null) {
                Log.w("HtmlHttpImageGetter", "Drawable result is null! (source: $source)")
                return
            }
            val size = containerReference.get()?.let {
                        it as? TextView
                    }?.textSize ?: (result.intrinsicWidth * scale)
            val urlDrawable = drawableReference.get() ?: return
// set the correct bound according to the result from HTTP call
            urlDrawable.setBounds(
                0,
                0,
                size.toInt(),
                size.toInt()
            )

            // change the reference of the current drawable to the result from the HTTP call
            urlDrawable.drawable = result

            val imageGetter = imageGetterReference.get() ?: return
// redraw the image by invalidating the container
            imageGetter.container.invalidate()
            // re-set text to fix images overlapping text
            imageGetter.container.text = imageGetter.container.text
        }

        /**
         * Get the Drawable from URL
         */
        fun fetchDrawable(res: Resources, urlString: String?): Drawable? {
            return try {
                val `is` = fetch(urlString)
                val drawable = BitmapDrawable(res, `is`)
                scale = getScale(drawable)
                drawable.setBounds(
                    0,
                    0,
                    (drawable.intrinsicWidth * scale).toInt(),
                    (drawable.intrinsicHeight * scale).toInt()
                )
                drawable
            } catch (e: Exception) {
                null
            }

        }

        /**
         * Get the compressed image with specific quality from URL
         */
        fun fetchCompressedDrawable(res: Resources, urlString: String?): Drawable? {
            try {
                val `is` = fetch(urlString)
                val original = BitmapDrawable(res, `is`).bitmap

                val out = ByteArrayOutputStream()
                original.compress(Bitmap.CompressFormat.JPEG, qualityImage, out)
                original.recycle()
                `is`!!.close()

                val decoded = BitmapFactory.decodeStream(ByteArrayInputStream(out.toByteArray()))
                out.close()

                scale = getScale(decoded)
                val b = BitmapDrawable(res, decoded)

                b.setBounds(
                    0,
                    0,
                    (b.intrinsicWidth * scale).toInt(),
                    (b.intrinsicHeight * scale).toInt()
                )
                return b
            } catch (e: Exception) {
                return null
            }

        }

        private fun getScale(bitmap: Bitmap): Float {
            val container = containerReference.get() ?: return 1f

            val maxWidth = container.width.toFloat()
            val originalDrawableWidth = bitmap.width.toFloat()

            return maxWidth / originalDrawableWidth
        }

        private fun getScale(drawable: Drawable): Float {
            val container = containerReference.get()
            if (!matchParentWidth || container == null) {
                return 1f
            }

            val maxWidth = container.width.toFloat()
            val originalDrawableWidth = drawable.intrinsicWidth.toFloat()

            return maxWidth / originalDrawableWidth
        }

        @Throws(IOException::class)
        private fun fetch(urlString: String?): InputStream? {
            val url: URL
            val imageGetter = imageGetterReference.get() ?: return null
            url = if (imageGetter.baseUri != null) {
                imageGetter.baseUri!!.resolve(urlString!!).toURL()
            } else {
                URI.create(urlString!!).toURL()
            }

            return url.content as InputStream
        }
    }

    inner class UrlDrawable : BitmapDrawable() {
        var drawable: Drawable? = null

        override fun draw(canvas: Canvas) {
            // override the draw to facilitate refresh function later
            if (drawable != null) {
                drawable!!.draw(canvas)
            }
        }
    }
}