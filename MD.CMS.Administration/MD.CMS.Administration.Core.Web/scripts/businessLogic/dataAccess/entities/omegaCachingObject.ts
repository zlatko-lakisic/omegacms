/// <reference path="./base/iBaseEntity.ts" />
/// <reference path="./base/baseEntity.ts" />

namespace mdBusinessLogic.dataAccess.entities {
    export class omegaCachingObject implements base.IBaseEntity<omegaCachingObject> {
        public ByteSize: number;
        public CacheSource: string;
        public CacheKey: string;
        public Timeout: string;
        public CacheTime: Date;
        public CacheValue: string;

        constructor(obj?: omegaCachingObject) {
            this.ByteSize = 0;
            this.CacheKey = '';
            this.Timeout = '';
            this.CacheTime = new Date();
            this.CacheValue = '';
            if (obj != undefined && obj != null) {
                this.construct(obj);
            }
        }

        public construct(data: any): void {
            this.ByteSize = helpers.entityHelper.getValue<number>(data, "ByteSize", 0);
            this.CacheSource = helpers.entityHelper.getValue<string>(data, "CacheSource", '');
            this.CacheKey = helpers.entityHelper.getValue<string>(data, "CacheKey", '');
            this.Timeout = helpers.entityHelper.getValue<string>(data, "Timeout", '');
            this.CacheTime = helpers.entityHelper.getValue<Date>(data, "CacheTime", new Date());
            this.CacheValue = helpers.entityHelper.getValue<string>(data, "CacheValue", '');
        }

        public clone(): omegaCachingObject {
            return new omegaCachingObject(this);
        }

    }
}