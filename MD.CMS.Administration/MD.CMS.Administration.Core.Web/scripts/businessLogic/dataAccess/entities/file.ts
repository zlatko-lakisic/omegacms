/// <reference path="./base/iBaseEntity.ts" />
/// <reference path="./base/baseEntity.ts" />
/// <reference path="./../../helpers.ts" />

namespace mdBusinessLogic.dataAccess.entities {
    export enum fileTypeEnum {
        image = 1,
        video = 2,
        audio = 3,
        application = 4,
        text = 5
    }

    export class file extends base.BaseEntity implements base.IBaseEntity<file> {
        public path: string;
        public fileType: fileTypeEnum;
        public data: any;

        constructor(obj?: file) {
            super(obj);
            this.path = '';
            this.fileType = null;
            this.data = null;
            if (obj != undefined && obj != null) {
                this.construct(obj);
            }
        }

        construct(data: any): void {
            this.path = this.getValue<string>(data, "path", '');
            this.fileType = this.getValue<fileTypeEnum>(data, "fileType", null);
            this.data = this.getValue<any>(data, "data", null);
        }

        public clone(): file {
            return new file(this);
        }

        public getFileType(): string {
            switch (this.fileType) {
                case fileTypeEnum.video:
                    return 'video';
                case fileTypeEnum.application:
                    return 'application';
                case fileTypeEnum.audio:
                    return 'audio';
                case fileTypeEnum.image:
                    return 'image';
                default:
                    return 'text';
            }
        }
    }
}
