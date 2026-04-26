namespace mdBusinessLogic.helpers {
    export class math {
        public static random(): number {
            const crypto = window.crypto || window['msCrypto'];
            let array = new Uint32Array(1);
            return parseFloat('0.' + crypto.getRandomValues(array)[0].toString());
        }
    }
}
