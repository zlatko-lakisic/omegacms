/// <reference path="./base/iBaseEntity.ts" />
/// <reference path="./omegaCachingObject.ts" />

namespace mdBusinessLogic.dataAccess.entities {
    export class cacheResponse implements base.IBaseEntity<cacheResponse> {
        public ProviderName: string;
        public CacheObjects: Array<omegaCachingObject>;

        constructor(obj?: cacheResponse) {
            this.ProviderName = '';
            this.CacheObjects = new Array<omegaCachingObject>();
            if (obj != undefined && obj != null) {
                this.construct(obj);
            }
        }

        construct(data: any): void {
            this.ProviderName = helpers.entityHelper.getValue<string>(data, "ProviderName", '');
            this.CacheObjects = helpers.entityHelper.getArrayConstructValue<omegaCachingObject>(data, 'CacheObjects', new Array<omegaCachingObject>(), new omegaCachingObject());
        }

        public clone(): cacheResponse {
            return new cacheResponse(this);
        }

    }
}