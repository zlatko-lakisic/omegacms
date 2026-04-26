/// <reference path="./base/iBaseEntity.ts" />
/// <reference path="./base/BaseEntity.ts" />
/// <reference path="./base/BaseEntity.ts" />
namespace mdBusinessLogic.dataAccess.entities {

    export class contentTypeDefinitionFolderDataBoundCondition implements base.IBaseEntity<contentTypeDefinitionFolderDataBoundCondition>{
        public ContentTypeDefinitionFieldId: number;
        public Value: string;
        public ContentTypeDefinitionId: number;
        public FolderId: number;
        public Comparer: ComparerType;

        constructor(obj?: contentTypeDefinitionFolderDataBoundCondition) {
            this.ContentTypeDefinitionFieldId = 0;
            this.Value = '';
            this.ContentTypeDefinitionId = 0;
            this.FolderId = 0;
            this.Comparer = ComparerType.Equals;
            if (obj != null) {
                this.construct(obj);
            }
        }

        public construct(data: any) {
            this.ContentTypeDefinitionFieldId = helpers.entityHelper.getValue<number>(data, 'ContentTypeDefinitionFieldId', 0);
            this.Value = helpers.entityHelper.getValue<string>(data, 'Value', '');
            this.ContentTypeDefinitionId = helpers.entityHelper.getValue<number>(data, 'ContentTypeDefinitionId', 0);
            this.FolderId = helpers.entityHelper.getValue<number>(data, 'FolderId', 0);
            this.Comparer = helpers.entityHelper.getValue<ComparerType>(data, 'Comparer', ComparerType.Equals);
        }

        public clone(): contentTypeDefinitionFolderDataBoundCondition {
            return new contentTypeDefinitionFolderDataBoundCondition(this);
        }
    }

    export enum ComparerType {
        Equals = 1,
        NotEquals = 2,
        Like = 3,
        GreaterThan = 4,
        GreaterThanOrEqualTo = 5,
        LessThan = 6,
        LessThanOrEqualTo = 7
    }
}