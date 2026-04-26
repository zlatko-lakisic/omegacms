/// <reference path="./base/iBaseEntity.ts" />
/// <reference path="./base/BaseEntity.ts" />
/// <reference path="./content.ts" />
/// <reference path="../../helpers/entityHelper.ts" />
namespace mdBusinessLogic.dataAccess.entities {
    export class aliasModel<T extends content & base.IBaseEntity<T>> implements base.IBaseEntity<aliasModel<T>> {
        public Id: string;
        public Template: string;
        public Content: T;
        public AliasType: aliasType;
        private Instance: T;

        constructor(obj?: aliasModel<T>) {
            this.Id = '';
            this.Template = '';
            this.Content = null;
            this.Instance = new content() as T;
            this.AliasType = aliasType.Content;
            if (obj !== undefined && obj != null) {
                this.construct(obj);
            }
        }

        public construct(data: any) {
            this.Id = mdBusinessLogic.helpers.entityHelper.getValue<string>(data, 'Id', '');
            this.Template = mdBusinessLogic.helpers.entityHelper.getValue<string>(data, 'Template', '');
            this.AliasType = mdBusinessLogic.helpers.entityHelper.getValue<aliasType>(data, 'AliasType', aliasType.Content);
            this.Content = mdBusinessLogic.helpers.entityHelper.getConstructEntityValue<T>(data, 'Content', this.Instance);
        }

        public clone(): aliasModel<T> {
            return new aliasModel<T>(this);
        }
    }

    export enum aliasType {
        Content = 1,
        Taxonomy = 2,
        Folder = 3
    }
}