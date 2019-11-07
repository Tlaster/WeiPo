package moe.tlaster.weipo.activity

import android.os.Bundle
import android.os.Parcelable
import com.bumptech.glide.Glide
import com.github.chrisbanes.photoview.PhotoView
import kotlinx.android.parcel.Parcelize
import kotlinx.android.synthetic.main.activity_image.*
import moe.tlaster.weipo.R
import moe.tlaster.weipo.common.adapter.AutoAdapter
import moe.tlaster.weipo.common.adapter.ItemSelector

@Parcelize
data class ImageData(
    val source: String,
    val placeHolder: String,
    val width: Long = 0,
    val height: Long = 0
): Parcelable

class ImageActivity : BaseActivity() {

    companion object {
        fun bundle(data: List<ImageData>, selectedIndex: Int = 0) : Array<Pair<String, Any?>> {
            return arrayOf(
                "image" to data,
                "selected_index" to selectedIndex
            )
        }
    }

    override val layoutId: Int
        get() = R.layout.activity_image

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        view_pager.adapter = AutoAdapter<ImageData>(ItemSelector(R.layout.item_zoomable_image)).apply {
            setView<PhotoView>(R.id.image_view) { view, item, _, _ ->
                Glide.with(view).load(item.source).also {
                    if (item.height > item.width * 3) {
                        // TODO:
                    }
                }.into(view)
            }
            intent.getParcelableArrayListExtra<ImageData>("image")?.let {
                items = it
            }
        }

        view_pager.post {
            view_pager.setCurrentItem(intent.getIntExtra("selected_index", 0), false)
        }
    }
}
