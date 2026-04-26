/// <reference path="./base/iBaseEntity.ts" />
/// <reference path="./base/BaseEntity.ts" />
/// <reference path="./base/BaseEntity.ts" />
namespace mdBusinessLogic.dataAccess.entities {

    export class contentTypeDefinitionFolderDataBoundSync implements base.IBaseEntity<contentTypeDefinitionFolderDataBoundSync>{
        public FolderId: number;
        public ContentTypeDefinitionId: number;
        public StartTime: Date;
        public EndTime: Date;
        public Frequency: number;
        public SyncType: contentTypeDefinitionFolderDataBoundSyncType;
        public DeltaFieldId: number;

        constructor(obj?: contentTypeDefinitionFolderDataBoundSync) {
            this.FolderId = 0;
            this.ContentTypeDefinitionId = 0;
            this.StartTime = new Date();
            this.EndTime = null;
            this.Frequency = (60 * 60 * 12);
            this.SyncType = contentTypeDefinitionFolderDataBoundSyncType.NoSync;
            this.DeltaFieldId = null;
            if (obj != null) {
                this.construct(obj);
            }
        }

        public construct(data: any) {
            this.FolderId = helpers.entityHelper.getValue<number>(data, 'FolderId', 0);
            this.ContentTypeDefinitionId = helpers.entityHelper.getValue<number>(data, 'ContentTypeDefinitionId', 0);
            this.StartTime = helpers.entityHelper.getValue<Date>(data, 'StartTime', new Date());
            this.EndTime = helpers.entityHelper.getValue<Date>(data, 'EndTime', null);
            this.Frequency = helpers.entityHelper.getValue<number>(data, 'Frequency', (60 * 60 * 12));
            this.SyncType = helpers.entityHelper.getValue<contentTypeDefinitionFolderDataBoundSyncType>(data, 'SyncType', contentTypeDefinitionFolderDataBoundSyncType.NoSync);
            this.DeltaFieldId = helpers.entityHelper.getValue<number>(data, 'DeltaFieldId', null);
        }

        public clone(): contentTypeDefinitionFolderDataBoundSync {
            return new contentTypeDefinitionFolderDataBoundSync(this);
        }
    }

    export enum contentTypeDefinitionFolderDataBoundSyncType {
        NoSync = 0,
        RemoteToOmega = 1,
        OmegaToRemote = 2,
        Bidirectional = 3
    }
}
