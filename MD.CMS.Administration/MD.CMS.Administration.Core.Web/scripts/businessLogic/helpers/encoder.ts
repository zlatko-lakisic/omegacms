/// <reference path="../js-base64.shim.d.ts" />

namespace mdBusinessLogic.helpers.encoder.base64 {
    export var encode = (input: string): string => {
        if (input === undefined || input == null) {
            return input;
        }
        return window.Base64.encode(input)
    };
    export var decode = (input: string): string => {
        if (input === undefined || input == null) {
            return input;
        }
        return window.Base64.decode(input)
    };
}
