namespace mdBusinessLogic.settings {
    export enum adminEventTypes {
        onTransitionBefore,
        onTransitionSuccess,
        onTransitionError,
        ajaxOnComplete,
        ajaxOnBeforeSend,
        ajaxOnUnauthorized,
        ajaxOnForbidden,
        ajaxOnJsonSerialize,
        onLogin,
        onLogout,
        onLogedInAndPermissionsLoaded,
        onBeforeUnload
    }

    export class adminEvent {
        private type: adminEventTypes;
        private event: Function;

        public constructor(type: adminEventTypes, event: Function) {
            this.type = type;
            this.event = event;
        }

        public getType(): adminEventTypes {
            return this.type;
        }

        public getPromise(...args: any[]): Promise<any> {
            return this.event.call(this, args);
        }
    }

    export class admin {
        private static adminEvents: Array<adminEvent> = new Array<adminEvent>();

        public static registerAdminEvent(adminEvent?: adminEvent) {
            if (!adminEvent) {
                throw new helpers.mdException('No admin event provided!');
            }

            switch (adminEvent.getType()) {
                case adminEventTypes.ajaxOnJsonSerialize:
                    let sxistingEvent: adminEvent = admin.adminEvents.filter(function (event) {
                        return event.getType() === adminEvent.getType();
                    })[0];
                    if (sxistingEvent !== undefined) {
                        sxistingEvent = adminEvent;
                        break;
                    }
                default:
                    this.adminEvents.push(adminEvent);
            }
        }

        public static onEvent(type: adminEventTypes, ...args: any[]): Promise<any[]> {
            return Promise.all(admin.adminEvents.filter(function (event) {
                return event.getType() === type;
            }).map(function (ev) {
                return ev.getPromise(args);
            }).filter(function (ev) {
                return ev !== undefined && ev != null;
            }).map(function (ev) {
                return new Promise(function (resolve, reject) {
                    try {
                        ev.then(function (data) {
                            let responseData = undefined;
                            if (data !== undefined) {
                                responseData = data[0];
                            }
                            if (responseData !== undefined) {
                                responseData = responseData[0];
                            }
                            resolve(responseData);
                        }).catch(function (data) {
                            reject(data);
                        });
                    } catch (e) {
                        reject(e);
                    }
                });
            }));
        }
    }
}
