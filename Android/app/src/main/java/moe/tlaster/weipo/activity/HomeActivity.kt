package moe.tlaster.weipo.activity

import android.os.Bundle
import androidx.navigation.findNavController
import androidx.navigation.ui.setupWithNavController
import kotlinx.android.synthetic.main.activity_home.*
import moe.tlaster.weipo.R

class HomeActivity : BaseActivity() {
    override val layoutId: Int
        get() = R.layout.activity_home

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        nav_view.setupWithNavController(findNavController(R.id.nav_host_fragment))
    }
}
