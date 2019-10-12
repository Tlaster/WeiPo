package moe.tlaster.weipo.activity

import androidx.lifecycle.ViewModelProviders
import moe.tlaster.weipo.R
import moe.tlaster.weipo.common.openActivity
import moe.tlaster.weipo.databinding.ActivityLoginBinding
import moe.tlaster.weipo.viewmodel.LoginViewModel


class LoginActivity : BindingActivity<ActivityLoginBinding>() {
    override val layoutId: Int
        get() = R.layout.activity_login

    override fun initBinding(binding: ActivityLoginBinding) {
        binding.viewmodel = ViewModelProviders.of(this).get(LoginViewModel::class.java).also {
            it.loginCompleted = {
                openActivity<TimelineActivity>()
            }
        }
    }

}
