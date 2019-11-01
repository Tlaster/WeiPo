package moe.tlaster.weipo.fragment.user

import androidx.fragment.app.Fragment
import moe.tlaster.weipo.R

interface ITabItem {
    val titleRes: Int
    val title: String
    val icon: Int
}
abstract class TabFragment(contentLayoutId: Int) : Fragment(contentLayoutId), ITabItem

abstract class UserTabFragment(
    contentLayoutId: Int,
    val userId: Long,
    val containerId: String
) : TabFragment(contentLayoutId) {
    override var title: String = ""
    override val titleRes: Int
        get() = -1
    override val icon: Int
        get() = -1
}

class EmptyTabFragment(userId: Long, containerId: String) :
    UserTabFragment(R.layout.empty, userId, containerId)