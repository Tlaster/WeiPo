import Shiba from "@shibajs/core";
import { jsbind } from "./extensions/jsbind";
import { res } from "./extensions/res";

const StatusView =
    <stack margin={8} padding={4}>
        <stack orientation="horizontal">
            <roundImg width={40} height={40} source={jsbind("user.profile_image_url")} />
            <stack margin={{ left: 8 }} orientation="vertical">
                <text text={jsbind("user.screen_name")} />
                <text color="gray" text={jsbind("created_at")} />
            </stack>
        </stack>
        <weiboText text={jsbind("text", "weiboTextConverter")} />
        <optional
            when={(it: any) => it.pics !== undefined && it.pics !== null}
            show={() =>
                <items
                    source={jsbind("pics")}
                    layout="nineGrid"
                    creator={() => <img source={jsbind("url")} />} />
            } />
        <optional
            when={(it: any) => it.page_info !== undefined && it.page_info !== null && it.page_info.type === "video"}
            show={() =>
                <grid>
                    <img source={jsbind("page_info.page_pic.url")} />
                </grid>
            } />
        <optional
            when={(it: any) => it.retweeted_status !== undefined && it.retweeted_status !== null}
            show={() =>
                <weiboStatus background={res("retweetBackground")} dataContext={jsbind("retweeted_status")} />} />
    </stack>;

registerComponent("weiboStatus", StatusView);
