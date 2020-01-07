package moe.tlaster.weipo.common.adapter

import androidx.fragment.app.Fragment
import androidx.fragment.app.FragmentActivity
import androidx.fragment.app.FragmentManager
import androidx.lifecycle.Lifecycle
import androidx.viewpager2.adapter.FragmentStateAdapter

class FragmentAdapter : FragmentStateAdapter {
    var items: List<Fragment>
        set(value) {
            field = value
            notifyDataSetChanged()
        }

    constructor(
        fragmentActivity: FragmentActivity,
        items: List<Fragment> = emptyList()
    ) : super(fragmentActivity) {
        this.items = items
    }

    constructor(fragment: Fragment, items: List<Fragment> = emptyList()) : super(fragment) {
        this.items = items
    }

    constructor(
        fragmentManager: FragmentManager,
        lifecycle: Lifecycle,
        items: List<Fragment> = emptyList()
    ) : super(fragmentManager, lifecycle) {
        this.items = items
    }

    override fun getItemCount(): Int {
        return items.count()
    }

    override fun createFragment(position: Int): Fragment {
        return items[position]
    }
}