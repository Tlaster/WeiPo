package moe.tlaster.weipo.activity

import android.os.Bundle
import android.text.method.LinkMovementMethod
import android.widget.ImageView
import android.widget.TextView
import androidx.activity.viewModels
import androidx.recyclerview.widget.LinearLayoutManager
import com.bumptech.glide.request.RequestOptions
import kotlinx.android.synthetic.main.activity_chat.*
import moe.tlaster.weipo.R
import moe.tlaster.weipo.common.adapter.IItemSelector
import moe.tlaster.weipo.common.adapter.IncrementalLoadingAdapter
import moe.tlaster.weipo.common.extensions.factory
import moe.tlaster.weipo.common.extensions.load
import moe.tlaster.weipo.common.fromHtml
import moe.tlaster.weipo.common.openWeiboLink
import moe.tlaster.weipo.services.models.Config
import moe.tlaster.weipo.services.models.Msg
import moe.tlaster.weipo.services.models.User
import moe.tlaster.weipo.viewmodel.ChatViewModel

class ChatSelector(private val uid: Long) : IItemSelector<Msg> {
    override fun selectLayout(item: Msg): Int {
        return if (item.senderID == uid) {
            R.layout.item_chat_from_me
        } else {
            R.layout.item_chat_from_other
        }
    }
}

class ChatActivity : BaseActivity() {
    override val layoutId: Int
        get() = R.layout.activity_chat

    private val data by lazy {
        intent.getParcelableExtra<User>("data")
    }

    private val config by lazy {
        intent.getParcelableExtra<Config>("config")
    }

    private val chatSelector by lazy {
        ChatSelector(config?.uid?.toLong() ?: 0)
    }

    private val viewModel by viewModels<ChatViewModel> {
        factory {
            ChatViewModel(data)
        }
    }

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setSupportActionBar(toolbar)
        data?.let { user ->
            supportActionBar?.let { actionBar ->
                actionBar.title = user.screenName
            }
        }
        viewModel.init()
        recycler_view.apply {
            layoutManager =
                LinearLayoutManager(this@ChatActivity, LinearLayoutManager.VERTICAL, true)
            adapter = IncrementalLoadingAdapter(chatSelector).apply {
                this.items = viewModel.source
                this.autoRefresh = true
                setView<ImageView>(R.id.user_avatar) { view, item ->
                    item.user?.profileImageURL?.let {
                        view.load(it) {
                            apply(RequestOptions.circleCropTransform())
                        }
                    }
                }

                setView<TextView>(R.id.chat_message) { view, item ->
                    view.movementMethod = LinkMovementMethod.getInstance()
                    view.text = fromHtml(item.text, view) {
                        openWeiboLink(this@ChatActivity, it)
                    }
                }

                setText(R.id.user_name) {
                    it.user?.screenName.toString()
                }
            }
        }
    }
}
