import { extension } from "@shibajs/core";
import { IShibaExtension } from "@shibajs/core/lib/types";

export function res<T>(target: string, converter?: string | ((value: T) => any)): IShibaExtension {
    return extension("res", target, converter);
}
