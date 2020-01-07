package moe.tlaster.weipo.fragment.user

import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import moe.tlaster.weipo.R
import moe.tlaster.weipo.fragment.TabFragment


abstract class UserTabFragment : TabFragment {
    constructor() : super()
    constructor(contentLayoutId: Int) : super(contentLayoutId)

    abstract val contentLayoutId: Int
    var userId: Long = 0
        private set

    var containerId: String = ""
        private set

    override var title: String = ""
    override val titleRes: Int
        get() = -1
    override val icon: Int
        get() = -1


    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        (savedInstanceState ?: arguments)?.let { bundle ->
            bundle.getString("containerId")?.let {
                containerId = it
            }
            if (bundle.getLong("userId") != 0L) {
                userId = bundle.getLong("userId")
            }
        }
        return super.onCreateView(inflater, container, savedInstanceState) ?: inflater.inflate(contentLayoutId, container, false)
    }

    override fun onSaveInstanceState(outState: Bundle) {
        super.onSaveInstanceState(outState)
        outState.putString("containerId", containerId)
        outState.putLong("userId", userId)
    }
}

class EmptyTabFragment() :
    UserTabFragment() {
    override val contentLayoutId: Int
        get() = R.layout.empty
}