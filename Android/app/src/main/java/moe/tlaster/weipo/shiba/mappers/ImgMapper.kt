package moe.tlaster.weipo.shiba.mappers

import android.view.ViewGroup
import com.facebook.drawee.generic.GenericDraweeHierarchyBuilder
import com.facebook.drawee.generic.RoundingParams
import com.facebook.drawee.view.SimpleDraweeView
import moe.tlaster.shiba.IShibaContext
import moe.tlaster.shiba.mapper.PropertyMap
import moe.tlaster.shiba.mapper.ViewMapper

open class ImgMapper : ViewMapper<SimpleDraweeView>() {
    override fun createNativeView(context: IShibaContext): SimpleDraweeView {
        return SimpleDraweeView(context.getContext())
    }

    override fun getViewLayoutParams(): ViewGroup.LayoutParams {
        return ViewGroup.MarginLayoutParams(ViewGroup.LayoutParams.MATCH_PARENT, ViewGroup.LayoutParams.MATCH_PARENT)
    }
    override fun propertyMaps(): ArrayList<PropertyMap> {
        return super.propertyMaps().apply {
            add(PropertyMap("source", {view, value ->
                if (view is SimpleDraweeView && value is String) {
                    view.setImageURI(value)
                }
            }))
        }
    }
}

class RoundImgMapper : ImgMapper() {
    override fun createNativeView(context: IShibaContext): SimpleDraweeView {
        return super.createNativeView(context).apply {
            hierarchy = GenericDraweeHierarchyBuilder(resources)
                .setRoundingParams(RoundingParams.fromCornersRadius(8F))
                .build()
        }
    }
}