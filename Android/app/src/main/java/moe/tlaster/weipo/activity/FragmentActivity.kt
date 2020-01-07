package moe.tlaster.weipo.activity

import android.os.Bundle
import androidx.core.view.isVisible
import androidx.fragment.app.Fragment
import kotlinx.android.synthetic.main.activity_fragment.*
import moe.tlaster.weipo.R

class FragmentActivity : BaseActivity() {
    override val layoutId: Int
        get() = R.layout.activity_fragment

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        toolbar.isVisible = intent.getBooleanExtra("show_toolbar", false)
        if (toolbar.isVisible) {
            setSupportActionBar(toolbar)
            intent.getIntExtra("title_res", 0).takeIf {
                it != 0
            }?.let {
                supportActionBar?.setTitle(it)
            }
        }
        intent.getStringExtra("className")?.let { className ->
            supportFragmentManager.beginTransaction().let { fragmentTransaction ->
                fragmentTransaction.replace(R.id.frame_layout, Class.forName(className).newInstance().let {
                    it as Fragment
                }.apply {
                    intent.getBundleExtra("data")?.let {
                        arguments = it
                    }
                })
                fragmentTransaction.commit()
            }
        }

    }
}
