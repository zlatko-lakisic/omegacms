/// <reference path="./base/iBaseEntity.ts" />
/// <reference path="./base/BaseEntity.ts" />
/// <reference path="../../helpers/entityHelper.ts" />
namespace mdBusinessLogic.dataAccess.entities {
    export class paginationEntity<T extends base.IBaseEntity<T> & base.BaseEntity> implements base.IBaseEntity<paginationEntity<T>> {
        public Items: Array<T>;
        public TotalCount: number;

        constructor(private type: new () => T, obj?: paginationEntity<T>) {
            this.Items = new Array<T>();
            this.TotalCount = 0;
            if (obj !== undefined && obj != null) {
                this.construct(obj);
            }
        }

        public construct(data: any) {
            this.Items = mdBusinessLogic.helpers.entityHelper.getArrayConstructEntityValue<T>(data, "Items", new Array<T>(), new this.type())
            this.TotalCount = mdBusinessLogic.helpers.entityHelper.getValue<number>(data, 'TotalCount', 0);
        }

        public clone(): paginationEntity<T> {
            return new paginationEntity<T>(this.type, this);
        }
    }
}