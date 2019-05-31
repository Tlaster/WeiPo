package moe.tlaster.weipo.shiba.mappers

import android.widget.TextView
import moe.tlaster.shiba.IShibaContext
import moe.tlaster.shiba.mapper.PropertyMap
import moe.tlaster.shiba.mapper.ViewMapper

class WeiboTextMapper : ViewMapper<TextView>() {
    override fun createNativeView(context: IShibaContext): TextView {
        return TextView(context.getContext())
    }

    override fun propertyMaps(): ArrayList<PropertyMap> {
        return super.propertyMaps().apply {
            add(PropertyMap("text", { view, value ->
                if (view is TextView && value is String) {
                    view.text = value
                }
            }))
        }
    }
}