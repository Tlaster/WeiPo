package moe.tlaster.weipo.activity

import androidx.lifecycle.ViewModelProviders
import moe.tlaster.weipo.R
import moe.tlaster.weipo.databinding.ActivityTimelineBinding
import moe.tlaster.weipo.viewmodel.TimelineViewModel

class TimelineActivity : BindingActivity<ActivityTimelineBinding>() {
    override val layoutId: Int
        get() = R.layout.activity_timeline

    override fun initBinding(binding: ActivityTimelineBinding) {
        binding.viewmodel = ViewModelProviders.of(this).get(TimelineViewModel::class.java)
    }

}
