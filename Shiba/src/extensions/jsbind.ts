import { extension } from "@shibajs/core";
import { IShibaExtension } from "@shibajs/core/lib/types";

export function jsbind<T>(target: string, converter?: string | ((value: T) => any)): IShibaExtension {
    return extension("jsbind", target, converter);
}
