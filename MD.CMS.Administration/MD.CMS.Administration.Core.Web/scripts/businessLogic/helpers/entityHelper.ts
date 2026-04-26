/// <reference path="../dataAccess/entities/base/iBaseEntity.ts" />
/// <reference path="../settings.ts" />
/// <reference path="../moment.shim.d.ts" />
namespace mdBusinessLogic.helpers {
    export class entityHelper {
        public static parseDateAndTimezoneToString(date: any = new Date(), timezone: string = moment.tz.guess(), delimiter: string = ';'): string {
            return moment(date).utc().format() + delimiter + timezone;
        }

        public static parseDateStringValue(data: any, defaultValue: Date = new Date(), delimiter: string = ';'): string {
            let returnValue: string = moment(defaultValue).utc().format();
            try {
                if (data !== undefined && data !== undefined) {
                    returnValue = data.split(delimiter)[0];
                }
            } catch (e) {
                if (settings.debug) {
                    console.warn(e);
                }
            }
            return returnValue;
        }

        public static parseDateValue(data: any, defaultValue: Date = new Date(), delimiter: string = ';'): Date {
            let returnValue: Date = defaultValue;
            try {
                if (data !== undefined && data !== undefined) {
                    returnValue = moment(data.split(delimiter)[0]).tz(this.parseTimeZoneValue(data)).toDate();
                }
            } catch (e) {
                if (settings.debug) {
                    console.warn(e);
                }
            }
            return returnValue;
        }

        public static parseTimeZoneValue(data: any, defaultValue: string = moment.tz.guess(), delimiter: string = ';'): string {
            let returnValue: string = defaultValue;
            try {
                if (data !== undefined && data !== undefined && data.split(delimiter)[1] !== undefined) {
                    returnValue = data.split(delimiter)[1];
                }
            } catch (e) {
                if (settings.debug) {
                    console.warn(e);
                }
            }
            return returnValue;
        }

        public static getDateValue(data: any, fieldName: string, defaultValue: Date, delimiter: string = ';'): Date {
            return this.parseDateValue(data[fieldName], defaultValue, delimiter);
        }

        public static getTimeZoneValue(data: any, fieldName: string, defaultValue: string, delimiter: string = ';'): string {
            return this.parseTimeZoneValue(data[fieldName], defaultValue, delimiter);
        }

        public static getValue<T>(data: any, fieldName: string, defaultValue: T): T {
            let returnValue: T = defaultValue;
            try {
                if (data !== undefined && data[fieldName] !== undefined) {
                    if (defaultValue instanceof Date) {
                        returnValue = moment(data[fieldName]).toDate() as any;
                    } else {
                        returnValue = data[fieldName];
                    }
                }
            } catch (e) {
                if (settings.debug) {
                    console.warn(e);
                }
            }
            return returnValue;
        }

        public static getConstructEntityValue<T extends dataAccess.entities.base.IBaseEntity<T> & dataAccess.entities.base.BaseEntity>(data: any, fieldName: string, defaultValue: T, returnNullIfInvalid?: boolean): T {
            if (returnNullIfInvalid === undefined) {
                returnNullIfInvalid = true;
            }
            let returnValue: T = defaultValue;
            try {
                let parsedJson: any = this.getValue<any>(data, fieldName, null);
                if (parsedJson != null && returnValue != null) {
                    returnValue.construct(parsedJson)
                    return returnValue.clone();
                } else if (returnNullIfInvalid) {
                    return null;
                }
            } catch (e) {
                if (settings.debug) {
                    console.warn(e);
                }
            }
            return defaultValue;
        }

        public static getConstructValue<T extends dataAccess.entities.base.IBaseEntity<T>>(data: any, fieldName: string, defaultValue: T): T {
            let returnValue: T = defaultValue;
            try {
                let parsedJson: any = this.getValue<any>(data, fieldName, null);
                if (parsedJson != null && returnValue != null) {
                    returnValue.construct(parsedJson);
                }
            } catch (e) {
                if (settings.debug) {
                    console.warn(e);
                }
            }
            return returnValue.clone();
        }

        public static getArrayConstructEntityValue<T extends dataAccess.entities.base.IBaseEntity<T> & dataAccess.entities.base.BaseEntity>(data: any, fieldName: string, defaultValue: Array<T>, defaultTypeValue: T, returnNullIfInvalid?: boolean): Array<T> {
            if (returnNullIfInvalid === undefined) {
                returnNullIfInvalid = true;
            }
            let returnValue: Array<T> = defaultValue;
            try {
                let parsedJson: any = this.getValue<any>(data, fieldName, null);
                if (parsedJson != null && parsedJson instanceof Array) {
                    for (let i: number = 0; i < parsedJson.length; i++) {
                        returnValue.push(this.getConstructEntityValue<T>(parsedJson, i.toString(), defaultTypeValue, returnNullIfInvalid));
                    }
                }
            } catch (e) {
                if (settings.debug) {
                    console.warn(e);
                }
            }
            return returnValue.filter(function (item) {
                return item !== undefined && item != null;
            });
        }

        public static getArrayConstructValue<T extends dataAccess.entities.base.IBaseEntity<T>>(data: any, fieldName: string, defaultValue: Array<T>, defaultTypeValue: T): Array<T> {
            let returnValue: Array<T> = defaultValue;
            try {
                let parsedJson: any = this.getValue<any>(data, fieldName, null);
                if (parsedJson != null && parsedJson instanceof Array) {
                    for (let i: number = 0; i < parsedJson.length; i++) {
                        returnValue.push(this.getConstructValue<T>(parsedJson, i.toString(), defaultTypeValue));
                    }
                }
            } catch (e) {
                if (settings.debug) {
                    console.warn(e);
                }
            }
            return returnValue.filter(function (item) {
                return item !== undefined && item != null;
            });
        }
    }
}