package moe.tlaster.weipo.fragment

interface ITabItem {
    val titleRes: Int
    val title: String
    val icon: Int
}
abstract class TabFragment : BaseFragment(), ITabItem {
    override var title: String = ""
    override val titleRes: Int
        get() = -1
    override val icon: Int
        get() = -1
}
