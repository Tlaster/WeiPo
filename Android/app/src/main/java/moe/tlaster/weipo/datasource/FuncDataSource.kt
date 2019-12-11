package moe.tlaster.weipo.datasource

import kotlinx.serialization.KSerializer
import kotlinx.serialization.json.Json
import kotlinx.serialization.list
import moe.tlaster.weipo.appContext
import moe.tlaster.weipo.common.collection.ICachedIncrementalSource
import moe.tlaster.weipo.common.collection.IIncrementalSource
import java.io.File


class FuncDataSource<T>(
    private val func: suspend (page: Int) -> List<T>
) : IIncrementalSource<T> {
    override suspend fun getPagedItemAsync(page: Int, count: Int): List<T> {
        return func.invoke(page)
    }
}


class CachedFuncDataSource<T>(
    private val cacheKey: String,
    private val serializer: KSerializer<T>,
    private val func: suspend (page: Int) -> List<T>
) : ICachedIncrementalSource<T> {
    override suspend fun getPagedItemAsync(page: Int, count: Int): List<T> {
        val result = func.invoke(page)
        if (page == 0)  {
            File(appContext.cacheDir, "$cacheKey.json").let {
                it.createNewFile()
                it.writeText(Json.stringify(serializer.list, result))
            }
        }
        return result
    }

    override suspend fun getCachedItemsAsync(): List<T> {
        return File(appContext.cacheDir, "$cacheKey.json")
            .takeIf {
                it.exists()
            }?.let {
                Json.parse(serializer.list, it.readText())
            } ?: emptyList()
    }
}