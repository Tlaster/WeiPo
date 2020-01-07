package moe.tlaster.weipo.fragment

interface ITabItem {
    val titleRes: Int
    val title: String
    val icon: Int
}
abstract class TabFragment : BaseFragment, ITabItem {
    constructor() : super()
    constructor(contentLayoutId: Int) : super(contentLayoutId)

    override var title: String = ""
    override val titleRes: Int
        get() = -1
    override val icon: Int
        get() = -1
}
