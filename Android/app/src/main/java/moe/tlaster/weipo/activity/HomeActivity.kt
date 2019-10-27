package moe.tlaster.weipo.activity

import android.os.Bundle
import androidx.navigation.findNavController
import androidx.navigation.plusAssign
import androidx.navigation.ui.setupWithNavController
import kotlinx.android.synthetic.main.activity_home.*
import moe.tlaster.weipo.R
import moe.tlaster.weipo.common.KeepStateNavigator

class HomeActivity : BaseActivity() {
    override val layoutId: Int
        get() = R.layout.activity_home

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        val navController = findNavController(R.id.nav_host_fragment)

        val navigator = KeepStateNavigator(this, nav_host_fragment.childFragmentManager, R.id.nav_host_fragment)
        navController.navigatorProvider += navigator
        navController.setGraph(R.navigation.home_navigation)
        nav_view.setupWithNavController(navController)
    }
}
