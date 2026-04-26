/// <reference path="./base/iBaseEntity.ts" />
/// <reference path="./base/baseEntity.ts" />
/// <reference path="./mediaContentMetaDataFeldValues.ts" />

namespace mdBusinessLogic.dataAccess.entities {
    export class mediaContent extends base.BaseEntity implements base.IBaseEntity<mediaContent> {
        public Id: number;
        public LCID: number;
        public Size: string;
        public Path: string;
        public FileType: number;
        public FolderId: number;
        public Name: string;
        public Description: string;
        public Type: any;
        public InputType: mediaContentInputType;
        public MediaContentMetaDataFieldValues: Array<mediaContentMetaDataFeldValues>;
        public PreviewUrl: string;
        public FullNameFile: string;
        public Icon: string;
        public DateCreated: Date;
        public UniqueId: string;

        constructor(obj?: mediaContent) {
            super(obj);
            this.Id = 0;
            this.LCID = 0;
            this.Size = '';
            this.Path = '';
            this.FileType = 0;
            this.FolderId = 0;
            this.Name = '';
            this.Description = '';
            this.Type = null;
            this.InputType = null;
            this.MediaContentMetaDataFieldValues = new Array<mediaContentMetaDataFeldValues>();
            this.PreviewUrl = '';
            this.FullNameFile = '';
            this.Icon = '';
            this.DateCreated = new Date();
            this.UniqueId = "";
            if (obj != undefined && obj != null) {
                this.construct(obj);
            }
        }

        construct(data: any): void {
            super.construct(data);
            this.Id = this.getValue<number>(data, "Id", 0);
            this.LCID = this.getValue<number>(data, "LCID", 0);
            this.Size = this.getValue<string>(data, "Size", '');
            this.Path = this.getValue<string>(data, "Path", '');
            this.FileType = this.getValue<number>(data, "FileType", 0);
            this.FolderId = this.getValue<number>(data, "FolderId", 0);
            this.Name = this.getValue<string>(data, "Name", '');
            this.Description = this.getValue<string>(data, "Description", '');
            this.Type = this.getValue<number>(data, "Type", 0);
            this.InputType = this.getValue<number>(data, "InputType", 0);
            this.MediaContentMetaDataFieldValues = this.getArrayConstructEntityValue<mediaContentMetaDataFeldValues>(data, "MediaContentMetaDataFieldValues", new Array<mediaContentMetaDataFeldValues>(), new mediaContentMetaDataFeldValues());
            this.PreviewUrl = this.getValue<string>(data, "PreviewUrl", '');
            this.FullNameFile = this.getValue<string>(data, "FullNameFile", '');
            this.Icon = this.getValue<string>(data, "Icon", '');
            this.DateCreated = this.getValue<Date>(data, "DateCreated", new Date());
            this.UniqueId = this.getValue<string>(data, 'UniqueId', '');
        }

        public clone(): mediaContent {
            return new mediaContent(this);
        }

    }

    export enum mediaContentInputType {
        jpg = 1,
        txt = 2,
        mp4 = 3,
        JPG = 4,
        png = 5,
        PNG = 6,
        flv = 7,
        mkv = 8,
        jpeg = 9,
        JPEG = 10,
        pdf = 11,
        docx = 12,
        xls = 13,
        xlsx = 14
    }
}
