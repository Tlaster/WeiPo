package moe.tlaster.weipo.fragment

import androidx.fragment.app.Fragment

interface ITabItem {
    val titleRes: Int
    val title: String
    val icon: Int
}
abstract class TabFragment : Fragment(), ITabItem {
    override var title: String = ""
    override val titleRes: Int
        get() = -1
    override val icon: Int
        get() = -1
}
