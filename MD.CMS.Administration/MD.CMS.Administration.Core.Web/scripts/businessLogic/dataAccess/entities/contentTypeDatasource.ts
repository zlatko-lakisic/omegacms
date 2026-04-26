/// <reference path="./base/iBaseEntity.ts" />
/// <reference path="./base/BaseEntity.ts" />
/// <reference path="./base/BaseEntity.ts" />

namespace mdBusinessLogic.dataAccess.entities {

    export class contentTypeDataSource extends base.BaseEntity implements base.IBaseEntity<contentTypeDataSource>{
        public ConnectionString: string;
        public ConnectionStringObject: any;
        public DbType: string;
        public ContentTypeDefinitionId: number;
        public CustomName: string;

        constructor(obj?: contentTypeDataSource) {
            super(obj);
            this.ConnectionString = '';
            this.ConnectionStringObject = {};
            this.DbType = '';
            this.ContentTypeDefinitionId = 0;
            this.CustomName = null;
            if (obj !== undefined && obj != null) {
                this.construct(obj);
            }
        }

        public construct(data: any) {
            super.construct(data);
            this.ConnectionString = this.getValue<string>(data, 'ConnectionString', '');
            this.ConnectionStringObject = this.getValue<any>(data, 'ConnectionStringObject', {}) as any;
            this.DbType = this.getValue<string>(data, 'DbType', '');
            this.ContentTypeDefinitionId = this.getValue<number>(data, 'ContentTypeDefinitionId', 0);
        }

        public toString(): string {
            if (this.CustomName !== undefined && this.CustomName != null && typeof this.CustomName) {
                return this.CustomName;
            }
            return this.DbType + ' ' + this.ConnectionString;
        }

        public clone(): contentTypeDataSource {
            return new contentTypeDataSource(this);
        }
    }
}