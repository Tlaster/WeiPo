import Shiba from "@shibajs/core";
import { json } from "./extensions/json";
import { res } from "./extensions/res";

const StatusView =
    <stack margin={8} padding={4} background={res("statusBackground")}>
        <stack orientation="horizontal">
            <roundImg width={40} height={40} source={json("user.profile_image_url")} />
            <stack margin={{ left: 8 }} orientation="vertical">
                <text text={json("user.screen_name")} />
                <text color="gray" text={json("created_at")} />
            </stack>
        </stack>
        <weiboText text={json("text", "weiboTextConverter")} />
        <optional
            when={(it: any) => it.pics !== undefined && it.pics !== null}
            show={() =>
                <items
                    source={json("pics")}
                    layout="nineGrid"
                    creator={() => <img source={json("url")} />} />
            } />
        <optional
            when={(it: any) => it.page_info !== undefined && it.page_info !== null && it.page_info.type === "video"}
            show={() =>
                <grid>
                    <img source={json("page_info.page_pic.url")} />
                </grid>
            } />
        <optional
            when={(it: any) => it.retweeted_status !== undefined && it.retweeted_status !== null}
            show={() => <weiboStatus background={res("retweetBackground")} dataContext={json("retweeted_status")} />} />
    </stack>;

registerComponent("weiboStatus", StatusView);
