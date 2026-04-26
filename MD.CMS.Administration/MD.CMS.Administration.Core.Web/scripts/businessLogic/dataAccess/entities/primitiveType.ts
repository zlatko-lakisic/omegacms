/// <reference path="./base/iBaseEntity.ts" />
/// <reference path="./base/baseEntity.ts" />

namespace mdBusinessLogic.dataAccess.entities {
    export class primitiveType<T> extends base.BaseEntity implements base.IBaseEntity<primitiveType<T>> {
        public Value: any;

        constructor(obj?: primitiveType<T>) {
            super(obj);
            this.Value = null;
            if (obj != undefined && obj != null) {
                this.Value = obj.Value;
            }
        }

        construct(value: T) {
            if (value != undefined && value != null) {
                if (!isNaN(parseInt(value.toString()))) {
                    this.Value = parseInt(value.toString());
                } else if (value.toString() === "true" || value.toString() === "false") {
                    this.Value = value.toString() === "true";
                } else {
                    try {
                        this.Value = JSON.parse(value.toString());
                    } catch (e) {
                        this.Value = value;
                    }
                }
            } else {
                this.Value = value;
            }
        }

        public clone(): primitiveType<T> {
            return new primitiveType<T>(this);
        }
    }
}