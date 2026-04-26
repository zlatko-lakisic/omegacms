namespace mdBusinessLogic.settings {
    export module ajax {
        /*export function onComplete(xhr: any): void {

        }

        export function onBeforeSend(xhr: XMLHttpRequest): void {

        }

        export function onUnauthorized(error: mdBusinessLogic.helpers.mdException): void {

        }

        export function onForbidden(error: mdBusinessLogic.helpers.mdException): void {

        }

        export function onJsonSerialize(nonSerializedRequest: any): any {
            return nonSerializedRequest;
        }*/

        export interface connectionObject<T> {
            id: string,
            obj: T,
            successEvents?: { (data: any): void; }[],
            errorEvents?: { (data: any): void; }[]
        }

        export class connections {
            private static _sockets: Array<connectionObject<WebSocket>> = new Array<connectionObject<WebSocket>>();
            private static _requests: Array<connectionObject<XMLHttpRequest>> = new Array<connectionObject<XMLHttpRequest>>();

            public static addSocket(socket: connectionObject<WebSocket>): void {
                this._sockets.push(socket);
            }

            public static addRequest(request: connectionObject<XMLHttpRequest>): void {
                this._requests.push(request);
            }

            public static getSocket(id: string): WebSocket {
                let returnObj: connectionObject<WebSocket> = this._sockets.filter(sockObj => { return sockObj.id == id; })[0];
                if (returnObj !== undefined) {
                    return returnObj.obj;
                }
                return null;
            }

            public static getRequest(id: string): XMLHttpRequest {
                return this.getRequestObject(id).obj;
            }

            public static getRequestObject(id: string): connectionObject<XMLHttpRequest> {
                let returnObj: connectionObject<XMLHttpRequest> = this._requests.filter(sockObj => { return sockObj.id == id; })[0];
                if (returnObj !== undefined) {
                    return returnObj;
                }
                return null;
            }

            public static removeSocket(id: string): void {
                for (let i = this._sockets.length - 1; i >= 0; i--) {
                    if (this._sockets[i].id == id) {
                        this._sockets[i].obj.close();
                        this._sockets.splice(i, 1);
                    }
                }
            }

            public static removeRequest(id: string): void {
                for (let i = this._requests.length - 1; i >= 0; i--) {
                    if (this._requests[i].id == id) {
                        this._requests[i].obj.abort();
                        this._requests.splice(i, 1);
                    }
                }
            }

            public static closeSockets(): void {
                for (let i in this._sockets) {
                    if (this._sockets[i] !== undefined &&
                        this._sockets[i] != null &&
                        this._sockets[i].obj !== undefined &&
                        this._sockets[i].obj != null) {
                        this._sockets[i].obj.close();
                    }
                }
            }

            public static closeRequests(): void {
                for (let i in this._requests) {
                    if (this._sockets[i] !== undefined &&
                            this._sockets[i] != null &&
                            this._sockets[i].obj !== undefined &&
                            this._sockets[i].obj != null) {
                        this._requests[i].obj.abort();
                    }
                }
            }

            public static closeAll(): void {
                this.closeSockets();
                this.closeRequests();
            }
        }
    }
}
