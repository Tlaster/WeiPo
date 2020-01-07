package moe.tlaster.weipo.activity

import android.os.Bundle
import androidx.fragment.app.Fragment
import moe.tlaster.weipo.R

class FragmentActivity : BaseActivity() {
    override val layoutId: Int
        get() = R.layout.activity_fragment

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
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
