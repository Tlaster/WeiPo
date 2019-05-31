package moe.tlaster.weipo.shiba.mappers

import moe.tlaster.shiba.IShibaContext
import moe.tlaster.shiba.dataBinding.ShibaBinding
import moe.tlaster.shiba.mapper.PropertyMap
import moe.tlaster.shiba.mapper.ViewMapper
import moe.tlaster.shiba.type.View
import moe.tlaster.weipo.shiba.controls.OptionalView

class OptionalMapper : ViewMapper<OptionalView>() {
    override fun createNativeView(context: IShibaContext): OptionalView {
        return OptionalView(context.getContext()).also { view ->
            ShibaBinding("dataContext").also { binding ->
                binding.targetView = view
                binding.viewSetter = {view, value ->
                    if (view is OptionalView) {
                        view.dataContext = value
                    }
                }
                binding.source = context
                binding.setValueToView()
                context.bindings += binding
            }
        }
    }

    override fun propertyMaps(): ArrayList<PropertyMap> {
        return super.propertyMaps().apply {
            add(PropertyMap("when", { view, value ->
                if (view is OptionalView && value is String) {
                    view.selector = value
                }
            }))
            add(PropertyMap("show", { view, value ->
                if (view is OptionalView && value is String) {
                    view.creator = value
                }
            }))
        }
    }

    override fun map(view: View, context: IShibaContext): OptionalView {
        return super.map(view, context).apply {
            finishMapping()
        }
    }
}