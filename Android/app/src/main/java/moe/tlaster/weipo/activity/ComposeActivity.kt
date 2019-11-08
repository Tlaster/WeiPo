package moe.tlaster.weipo.activity

import android.app.Activity
import android.content.Intent
import android.os.Build
import android.os.Bundle
import android.provider.Settings
import android.view.View
import android.widget.ImageButton
import android.widget.ImageView
import androidx.core.view.updateLayoutParams
import androidx.core.widget.doOnTextChanged
import androidx.recyclerview.widget.ItemTouchHelper
import androidx.recyclerview.widget.RecyclerView
import com.google.android.material.tabs.TabLayoutMediator
import com.hold1.keyboardheightprovider.KeyboardHeightProvider
import kotlinx.android.synthetic.main.activity_compose.*
import kotlinx.coroutines.*
import moe.tlaster.weipo.R
import moe.tlaster.weipo.common.AutoStaggeredGridLayoutManager
import moe.tlaster.weipo.common.SimpleItemTouchHelperCallback
import moe.tlaster.weipo.common.adapter.AutoAdapter
import moe.tlaster.weipo.common.adapter.IncrementalLoadingAdapter
import moe.tlaster.weipo.common.adapter.ItemSelector
import moe.tlaster.weipo.common.collection.CollectionChangedEventArg
import moe.tlaster.weipo.common.extensions.*
import moe.tlaster.weipo.common.isMiUi
import moe.tlaster.weipo.services.Api
import moe.tlaster.weipo.services.models.Emoji
import moe.tlaster.weipo.services.models.ICanReply
import moe.tlaster.weipo.services.models.Status
import moe.tlaster.weipo.viewmodel.ComposeViewModel
import java.io.File
import java.util.*


val emojis = GlobalScope.async(Dispatchers.IO, start = CoroutineStart.LAZY) {
    val result = Api.emoji()
    val list = arrayListOf<Emoji>()
    result.data?.usual?.let {
        list.addAll(it.values.flatten())
    }
    result.data?.more?.let {
        list.addAll(it.values.flatten())
    }
    result.data?.brand?.norm?.let {
        list.addAll(it.values.flatten())
    }
    return@async list.groupBy {
        it.category ?: ""
    }
}

class ComposeActivity : BaseActivity() {
    companion object {
        fun bundle(type: ComposeViewModel.ComposeType, reply: ICanReply? = null): Array<Pair<String, Any?>> {
            return arrayOf(
                "compose_type" to type.ordinal,
                "reply_to" to reply
            )
        }
    }

    private val IMAGE_PICKER_REQUEST_CODE: Int = 3247
    private val maxImageCount by lazy {
        when (composeType) {
            ComposeViewModel.ComposeType.Create -> 9
            ComposeViewModel.ComposeType.Repost -> 1
            ComposeViewModel.ComposeType.Comment -> 1
        }
    }
    private val composeType by lazy {
        ComposeViewModel.ComposeType.fromInt(
            intent.getIntExtra(
                "compose_type",
                ComposeViewModel.ComposeType.Create.ordinal
            )
        ) ?: ComposeViewModel.ComposeType.Create
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
            intent.getParcelableExtra<ICanReply>("reply_to").let { reply ->
                ComposeViewModel(
                    composeType,
                    if (composeType == ComposeViewModel.ComposeType.Repost && reply is Status) {
                        reply.retweetedStatus ?: reply
                    } else {
                        reply
                    }
                )
            }
        })
    }

    private val emojiAdapter by lazy {
        AutoAdapter<Pair<String, List<Emoji>>>(ItemSelector(R.layout.layout_pure_list)).apply {
            setView<RecyclerView>(R.id.recycler_view) { view, item ->
                view.layoutManager = AutoStaggeredGridLayoutManager(40.dp.toInt())
                view.adapter = AutoAdapter<Emoji>(ItemSelector(R.layout.item_emoji)).apply {
                    setView<ImageView>(R.id.image) { view: ImageView, item: Emoji ->
                        item.url?.let { view.load("https:$it") }
                        view.setOnClickListener {
                            item.value?.let { emojiValue ->
                                compose_input.text.insert(compose_input.selectionStart, emojiValue)
                            }
                        }
                    }
                    items = item.second
                }
            }
        }
    }

    private var keyboardHeight = 0
        set(value) {
            field = value
            if (value > 0) {
                emoji_container.updateLayoutParams {
                    height = value.let {
                        var result = it
                        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.P) {
//                            result += getTopCutoutHeight()
                        }
                        if (isMiUi() && Settings.Global.getInt(contentResolver, "force_fsg_nav_bar", 0) != 0) {
                            result += getNavigationBarHeight()
                        }
                        result
                    }
                }
            }
        }
    private lateinit var keyboardHeightProvider: KeyboardHeightProvider

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
            if (viewModel.content.length > viewModel.maxLength) {
                toast("Text length out of range")
            } else {
                viewModel.commit()
                finish()
            }
        }
        compose_input.post {
            if (composeType == ComposeViewModel.ComposeType.Repost) {
                intent.getParcelableExtra<ICanReply>("reply_to")?.let {
                    it as? Status
                }?.let {
                    compose_input.setText(if (it.retweetedStatus == null) {
                        ""
                    } else {
                        "//@${it.user?.screenName}:${it.rawText}"
                    })
                }
            }
        }
        view_pager.adapter = emojiAdapter
        keyboardHeightProvider = KeyboardHeightProvider(this).apply {
            addKeyboardListener(object : KeyboardHeightProvider.KeyboardListener {
                override fun onHeightChanged(height: Int) {
                    keyboardHeight = height
                }
            })
        }
        if (emojis.start()) {
            emojis.invokeOnCompletion {
                if (!isFinishing) {
                    runOnMainThread {
                        updateEmojiView()
                    }
                }
            }
        } else {
            updateEmojiView()
        }
        emoji_button.setOnClickListener {
            toggleSoftInput()
        }
    }

    @UseExperimental(ExperimentalCoroutinesApi::class)
    private fun updateEmojiView() {
        val result = emojis.getCompleted()
        emojiAdapter.items = result.toList()
        TabLayoutMediator(
            tab_layout,
            view_pager,
            TabLayoutMediator.TabConfigurationStrategy { tab, position ->
                result.keys.toList()[position].let {
                    if (it.isNotEmpty()) {
                        tab.setText(it)
                    } else {

                    }
                }
            }).attach()
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
                    (viewModel.images + it.mapNotNull { it.uri.getFilePath(this) }.map { File(it) }).takeLast(
                        maxImageCount
                    )
                }?.let {
                    viewModel.images.clear()
                    viewModel.images.addAll(it)
                }
                data.data?.getFilePath(this)?.let {
                    File(it)
                }?.let {
                    (viewModel.images + it).takeLast(maxImageCount)
                }?.let {
                    viewModel.images.clear()
                    viewModel.images.addAll(it)
                }
            }
        }
    }

    override fun onResume() {
        super.onResume()
        keyboardHeightProvider.onResume()
    }

    override fun onPause() {
        super.onPause()
        keyboardHeightProvider.onPause()
    }

}

