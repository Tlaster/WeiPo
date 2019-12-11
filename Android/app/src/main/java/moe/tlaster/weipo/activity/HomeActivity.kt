package moe.tlaster.weipo.activity

import android.os.Bundle
import androidx.core.view.isVisible
import androidx.navigation.findNavController
import androidx.navigation.ui.setupWithNavController
import kotlinx.android.synthetic.main.activity_home.*
import moe.tlaster.weipo.R
import moe.tlaster.weipo.common.extensions.openActivity
import moe.tlaster.weipo.viewmodel.ComposeViewModel

class HomeActivity : BaseActivity() {
    override val layoutId: Int
        get() = R.layout.activity_home

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        val navController = findNavController(R.id.nav_host_fragment)
        bottom_navigation_view.setupWithNavController(navController)
        floating_button.setOnClickListener {
            it?.context?.openActivity<ComposeActivity>("compose_type" to ComposeViewModel.ComposeType.Create)
        }
        bottom_navigation_view.setOnNavigationItemSelectedListener {
            floating_button.isVisible = it.itemId == R.id.navigation_timeline
            false
        }
    }
}
