package moe.tlaster.weipo.activity

import android.app.Activity
import android.content.Intent
import android.os.Bundle
import android.view.View
import android.widget.ImageButton
import android.widget.ImageView
import androidx.core.widget.doOnTextChanged
import androidx.recyclerview.widget.ItemTouchHelper
import kotlinx.android.synthetic.main.activity_compose.*
import kotlinx.coroutines.GlobalScope
import kotlinx.coroutines.launch
import moe.tlaster.weipo.R
import moe.tlaster.weipo.common.SimpleItemTouchHelperCallback
import moe.tlaster.weipo.common.adapter.IncrementalLoadingAdapter
import moe.tlaster.weipo.common.adapter.ItemSelector
import moe.tlaster.weipo.common.collection.CollectionChangedEventArg
import moe.tlaster.weipo.common.extensions.*
import moe.tlaster.weipo.viewmodel.ComposeViewModel
import java.io.File
import java.util.*


class ComposeActivity : BaseActivity() {
    private val IMAGE_PICKER_REQUEST_CODE: Int = 3247
    private val maxImageCount by lazy {
        intent.getIntExtra("max_image_count", 9)
    }
    private val onImageCollectionChanged: (Any, CollectionChangedEventArg) -> Unit = { _, _ ->
        image_picked_list.visibility = if (viewModel.images.any()) {
            View.VISIBLE
        } else {
            View.GONE
        }
    }

    private val imageAdapter by lazy {
        IncrementalLoadingAdapter<File>(ItemSelector(R.layout.item_compose_image)).apply {
            setView<ImageView>(R.id.image) { view, item, _, _ ->
                view.load(item)
            }
            setView<ImageButton>(R.id.remove_button) { view, item, _, _ ->
                view.setOnClickListener {
                    viewModel.images.remove(item)
                }
            }
            items = viewModel.images
        }
    }

    private val touchHelper by lazy {
        ItemTouchHelper(SimpleItemTouchHelperCallback { fromPosition, toPosition ->
            Collections.swap(viewModel.images, fromPosition, toPosition)
            imageAdapter.notifyItemMoved(fromPosition, toPosition)
        })
    }

    override val layoutId: Int
        get() = R.layout.activity_compose

    private val viewModel by lazy {
        viewModel<ComposeViewModel>(factory {
            ComposeViewModel(ComposeViewModel.ComposeType.Create)
        })
    }

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        compose_input.post {
            compose_input.requestFocus()
        }
        close_button.setOnClickListener {
            onBackPressed()
        }
        open_image_picker_button.setOnClickListener {
            openImagePicker()
        }
        image_picked_list.adapter = imageAdapter
        viewModel.images.collectionChanged += onImageCollectionChanged
        touchHelper.attachToRecyclerView(image_picked_list)
        compose_input.doOnTextChanged { text, _, _, _ ->
            viewModel.content = text.toString()
        }
        send_button.setOnClickListener {
            GlobalScope.launch {
                viewModel.commit()
                runOnMainThread {
                    finish()
                }
            }
        }
    }

    private fun openImagePicker() {
        Intent().apply {
            type = "image/*"
            putExtra(Intent.EXTRA_ALLOW_MULTIPLE, true)
            action = Intent.ACTION_GET_CONTENT
        }.also {
            startActivityForResult(
                Intent.createChooser(it, "Select picture"),
                IMAGE_PICKER_REQUEST_CODE
            )
        }
    }

    override fun onActivityResult(requestCode: Int, resultCode: Int, data: Intent?) {
        super.onActivityResult(requestCode, resultCode, data)
        when {
            requestCode == IMAGE_PICKER_REQUEST_CODE && resultCode == Activity.RESULT_OK && data != null -> {
                data.clipData?.let { clip ->
                    (0 until clip.itemCount).map { i ->
                        clip.getItemAt(i)
                    }
                }?.let {
                    (viewModel.images + it.mapNotNull { it.uri.getFilePath(this) }.map { File(it) }).take(maxImageCount)
                }?.let {
                    viewModel.images.clear()
                    viewModel.images.addAll(it)
                }
            }
        }
    }
}

