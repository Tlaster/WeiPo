import Shiba from "@shibajs/core";
import { jsbind } from "./extensions/jsbind";
import { res } from "./extensions/res";

class WeiboSource {
    private currentMaxId = 0;

    // @ts-ignore
    public async getPagedItemsAsync(page: number): any {
        // @ts-ignore
        const cookieMap = JSON.parse(shibaStorage.load("usercookie", ""));
        let cookie = "";
        for (const key in cookieMap) {
            if (cookieMap.hasOwnProperty(key)) {
                const element = cookieMap[key];
                cookie += `${key}=${element};`;
            }
        }
        // @ts-ignore
        const result = await http.get("https://m.weibo.cn/feed/friends", cookie);
        const json = JSON.parse(result);
        return json.data.statuses;
    }
}

const TimelineView =
    <items
        layout="staggered"
        creator={() => <weiboStatus dataContext={jsbind("")}/>}
        source={() => new WeiboSource()}/>;

registerComponent("weiboTimeline", TimelineView);
