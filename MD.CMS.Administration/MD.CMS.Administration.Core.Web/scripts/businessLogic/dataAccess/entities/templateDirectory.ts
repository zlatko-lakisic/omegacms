/// <reference path="./base/iBaseEntity.ts" />
/// <reference path="./base/baseEntity.ts" />
/// <reference path="./templateFile.ts" />

namespace mdBusinessLogic.dataAccess.entities {
    export class templateDirectory extends base.BaseEntity implements base.IBaseEntity<templateDirectory> {
        public Path: string;
        public Children: Array<templateDirectory>;
        public Files: Array<templateFile>;
        public Name: string;
        public RootPath: string;

        constructor(obj?: templateDirectory) {
            super(obj);
            this.Path = '';
            this.Children = new Array<templateDirectory>();
            this.Files = new Array<templateFile>();
            this.Name = '';
            this.RootPath = '';
            if (obj != undefined && obj != null) {
                this.construct(obj);
            }
        }

        construct(data: any): void {
            super.construct(data);
            this.Path = this.getValue<string>(data, "Path", '');
            this.Children = this.getArrayConstructEntityValue<templateDirectory>(data, "Children", new Array<templateDirectory>(), new templateDirectory());
            this.Files = this.getArrayConstructEntityValue<templateFile>(data, "Files", new Array<templateFile>(), new templateFile());
            this.Name = this.getValue<string>(data, "Name", '');
            this.RootPath = this.getValue<string>(data, "RootPath", '');
        }

        public clone(): templateDirectory {
            return new templateDirectory(this);
        }

    }
}