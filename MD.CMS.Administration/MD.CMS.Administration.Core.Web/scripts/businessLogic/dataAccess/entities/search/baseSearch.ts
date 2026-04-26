/// <reference path="../base/iBaseEntity.ts" />
/// <reference path="../../../helpers/entityHelper.ts" />
namespace mdBusinessLogic.dataAccess.entities.search {
    export abstract class baseSearch {
        public Id: number;
        public Name: string;
        public TableName: string;

        constructor(obj?: baseSearch) {
            this.Id = 0;
            this.Name = '';
            this.TableName = '';
            if (obj !== undefined && obj != null) {
                this.Id = obj.Id;
                this.Name = obj.Name;
                this.TableName = obj.TableName;
            }
        }

        public construct(data: any) {
            this.Id = mdBusinessLogic.helpers.entityHelper.getValue<number>(data, 'Id', 0);
            this.Name = mdBusinessLogic.helpers.entityHelper.getValue<string>(data, 'Name', '');
            this.TableName = mdBusinessLogic.helpers.entityHelper.getValue<string>(data, 'TableName', '');
        }
    }
}
