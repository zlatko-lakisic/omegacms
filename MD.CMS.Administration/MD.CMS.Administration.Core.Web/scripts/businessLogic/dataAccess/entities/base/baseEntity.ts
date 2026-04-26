/// <reference path="../../../helpers/entityHelper.ts" />
namespace mdBusinessLogic.dataAccess.entities.base {
    interface NoParamConstructor<T> {
        new(): T;
    }

    export abstract class BaseEntity {
        public Id: any;
        public IsDeleted: boolean;

        constructor(obj?: BaseEntity) {
            this.Id = 0;
            this.IsDeleted = false;
            if (obj !== undefined && obj != null) {
                this.Id = obj.Id;
                this.IsDeleted = obj.IsDeleted;
            }
        }

        public static getDateValue(data: any, fieldName: string, defaultValue: Date): Date {
            return helpers.entityHelper.getDateValue(data, fieldName, defaultValue);
        }

        public getDateValue(data: any, fieldName: string, defaultValue: Date): Date {
            return helpers.entityHelper.getDateValue(data, fieldName, defaultValue);
        }

        public static getTimeZoneValue(data: any, fieldName: string, defaultValue: string): string {
            return helpers.entityHelper.getTimeZoneValue(data, fieldName, defaultValue);
        }

        public getTimeZoneValue(data: any, fieldName: string, defaultValue: string): string {
            return helpers.entityHelper.getTimeZoneValue(data, fieldName, defaultValue);
        }

        public static getValue<T>(data: any, fieldName: string, defaultValue: T): T {
            return helpers.entityHelper.getValue<T>(data, fieldName, defaultValue);
        }

        public getValue<T>(data: any, fieldName: string, defaultValue: T): T {
            return helpers.entityHelper.getValue<T>(data, fieldName, defaultValue);
        }

        public static getConstructValue<T extends IBaseEntity<T>>(data: any, fieldName: string, defaultValue: T): T {
            return helpers.entityHelper.getConstructValue<T>(data, fieldName, defaultValue);
        }

        public getConstructValue<T extends IBaseEntity<T>>(data: any, fieldName: string, defaultValue: T): T {
            return helpers.entityHelper.getConstructValue<T>(data, fieldName, defaultValue);
        }

        public static getConstructEntityValue<T extends IBaseEntity<T> & BaseEntity>(data: any, fieldName: string, defaultValue: T, returnNullIfInvalid?: boolean): T {
            return helpers.entityHelper.getConstructEntityValue<T>(data, fieldName, defaultValue, returnNullIfInvalid);
        }

        public getConstructEntityValue<T extends IBaseEntity<T> & BaseEntity>(data: any, fieldName: string, defaultValue: T, returnNullIfInvalid?: boolean): T {
            return helpers.entityHelper.getConstructEntityValue<T>(data, fieldName, defaultValue, returnNullIfInvalid);
        }

        public static getArrayConstructValue<T extends IBaseEntity<T>>(data: any, fieldName: string, defaultValue: Array<T>, defaultTypeValue: T): Array<T> {
            return helpers.entityHelper.getArrayConstructValue<T>(data, fieldName, defaultValue, defaultTypeValue);
        }

        public getArrayConstructValue<T extends IBaseEntity<T>>(data: any, fieldName: string, defaultValue: Array<T>, defaultTypeValue: T): Array<T> {
            return helpers.entityHelper.getArrayConstructValue<T>(data, fieldName, defaultValue, defaultTypeValue);
        }

        public static getArrayConstructEntityValue<T extends IBaseEntity<T> & BaseEntity>(data: any, fieldName: string, defaultValue: Array<T>, defaultTypeValue: T, returnNullIfInvalid?: boolean): Array<T> {
            return helpers.entityHelper.getArrayConstructEntityValue<T>(data, fieldName, defaultValue, defaultTypeValue, returnNullIfInvalid);
        }

        public getArrayConstructEntityValue<T extends IBaseEntity<T> & BaseEntity>(data: any, fieldName: string, defaultValue: Array<T>, defaultTypeValue: T, returnNullIfInvalid?: boolean): Array<T> {
            return helpers.entityHelper.getArrayConstructEntityValue<T>(data, fieldName, defaultValue, defaultTypeValue, returnNullIfInvalid);
        }

        public construct(data: any) {
            this.Id = this.getValue<number>(data, 'Id', 0);
            this.IsDeleted = this.getValue<boolean>(data, 'IsDeleted', false);
        }
    }
}