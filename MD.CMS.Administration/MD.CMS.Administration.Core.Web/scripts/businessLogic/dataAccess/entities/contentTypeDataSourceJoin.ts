/// <reference path="./base/iBaseEntity.ts" />
/// <reference path="./base/BaseEntity.ts" />
/// <reference path="./base/BaseEntity.ts" />
namespace mdBusinessLogic.dataAccess.entities {

    export class contentTypeDataSourceJoin extends base.BaseEntity implements base.IBaseEntity<contentTypeDataSourceJoin>{
        public RightDataSourceId: number;
        public LeftRightDataSourceJoinType: string;
        public LeftFieldId: number;
        public RightFieldId: number;

        constructor(obj?: contentTypeDataSourceJoin) {
            super(obj);
            this.RightDataSourceId = 0;
            this.LeftRightDataSourceJoinType = '';
            this.LeftFieldId = 0;
            this.RightFieldId = 0;
            if (obj !== undefined && obj != null) {
                this.construct(obj);
            }
        }

        public construct(data: any) {
            super.construct(data);
            this.RightDataSourceId = helpers.entityHelper.getValue<number>(data, 'RightDataSourceId', 0);
            this.LeftRightDataSourceJoinType = helpers.entityHelper.getValue<string>(data, 'LeftRightDataSourceJoinType', '');
            this.LeftFieldId = helpers.entityHelper.getValue<number>(data, 'LeftFieldId', 0);
            this.RightFieldId = helpers.entityHelper.getValue<number>(data, 'RightFieldId', 0);
        }

        public clone(): contentTypeDataSourceJoin {
            return new contentTypeDataSourceJoin(this);
        }
    }
}