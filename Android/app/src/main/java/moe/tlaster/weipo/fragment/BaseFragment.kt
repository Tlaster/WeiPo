package moe.tlaster.weipo.fragment

import androidx.fragment.app.Fragment
import androidx.recyclerview.widget.RecyclerView
import moe.tlaster.weipo.R

abstract class BaseFragment : Fragment {
    constructor() : super()
    constructor(contentLayoutId: Int) : super(contentLayoutId)

    override fun onDestroyView() {
        super.onDestroyView()
        view?.findViewById<RecyclerView>(R.id.recycler_view)?.adapter = null
    }
}