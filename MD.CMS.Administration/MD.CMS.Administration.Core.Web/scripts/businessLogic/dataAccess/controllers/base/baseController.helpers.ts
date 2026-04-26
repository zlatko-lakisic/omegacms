/// <reference path="../../entities/base/iBaseEntity.ts" />
namespace mdBusinessLogic.dataAccess.controllers.base {
    /**
     * BaseController Helpers class
     */
    export abstract class BaseController_helpers {
        protected loadParentNamesAsArray(nameArray: Array<any>, obj: any, parentName: string, parentLinkName: string): void {
            if (parentName === undefined) {
                parentName = 'Name';
            }
            if (obj[parentName] !== undefined && obj[parentName] != null) {
                if (parentLinkName !== undefined && obj[parentLinkName] !== undefined && obj[parentLinkName] != null) {
                    var customObj: Object = new Object();
                    customObj[parentName] = obj[parentName];
                    customObj[parentLinkName] = obj[parentLinkName];
                    nameArray.push(customObj);
                } else {
                    nameArray.push(obj[parentName]);
                }
            }
            if (obj.Parent !== undefined && obj.Parent != null) {
                this.loadParentNamesAsArray(nameArray, obj.Parent, parentName, parentLinkName);
            }
        }

        protected parseUrl(url: string): HTMLAnchorElement {
            let l = document.createElement("a");
            l.href = url;
            return l;
        }

        public getAddress(endpoint: string, data?: any): string {
            let address: string = endpoint;
            if (data !== undefined) {
                if (data instanceof Array) {
                    if (address[address.length - 1] != '/') {
                        address += '/';
                    }
                    for (let i = 0; i < data.length; i++) {
                        if (data[i] !== undefined && data[i] !== null) {
                            address += data[i].toString();
                        }
                        if (i < data.length - 1) {
                            address += '/';
                        }
                    }
                } else {
                    if (address[address.length - 1] != '?') {
                        address += '?';
                    }
                    let counter = 0;
                    for (let key in data) {
                        if (counter > 0) {
                            address += '&';
                        }
                        if (data[key] !== undefined && data[key] !== null) {
                            address += encodeURIComponent(key) + '=' + encodeURIComponent((data[key] instanceof Array) ? data[key].join(',') : data[key]);
                            counter++;
                        }
                    }
                }
            }
            return address;
        }
    }
}
