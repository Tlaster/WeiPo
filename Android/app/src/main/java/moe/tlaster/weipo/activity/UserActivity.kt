package moe.tlaster.weipo.activity

import android.os.Bundle
import androidx.core.os.bundleOf
import moe.tlaster.weipo.R
import moe.tlaster.weipo.fragment.UserFragment

class UserActivity : BaseActivity() {

    companion object {
        fun bundle(name: String? = null, id: Long? = null): Array<Pair<String, Any?>> {
            return arrayOf(
                "user_name" to name,
                "user_id" to (id ?: 0L)
            )
        }
    }

    override val layoutId: Int
        get() = R.layout.activity_fragment

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        supportFragmentManager.beginTransaction().let { fragmentTransaction ->
            fragmentTransaction.replace(R.id.frame_layout, UserFragment().apply {
                arguments = bundleOf(
                    "user_id" to intent.getLongExtra("user_id", 0L),
                    "user_name" to intent.getStringExtra("user_name")
                )
            })
            fragmentTransaction.commit()
        }
    }
}
