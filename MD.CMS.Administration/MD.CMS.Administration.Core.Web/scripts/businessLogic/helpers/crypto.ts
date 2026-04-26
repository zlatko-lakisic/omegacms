/// <reference path="../crypto-js.shim.d.ts" />

namespace mdBusinessLogic.helpers {
    export class crypto {
        public static md5(input: string): string {
            return CryptoJS.MD5(input).toString();
        }
        public static aes(input: string): string {
            return CryptoJS.AES.encrypt(input, '').toString();
        }
        public static sha256(input: string): string {
            return CryptoJS.SHA256(input).toString();
        }
        public static sha3(input: string): string {
            return CryptoJS.SHA3(input).toString();
        }
    }
}
