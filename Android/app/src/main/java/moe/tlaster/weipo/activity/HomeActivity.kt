package moe.tlaster.weipo.activity

import android.os.Bundle
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
        navController.addOnDestinationChangedListener { _, destination, _ ->
            if (destination.id == R.id.navigation_timeline) {
                floating_button.show()
            } else {
                floating_button.hide()
            }
        }
    }
}
