/// <reference path="./base/iBaseEntity.ts" />
/// <reference path="./base/baseEntity.ts" />
/// <reference path="./../../helpers.ts" />
/// <reference path="./menuContent.ts" />

namespace mdBusinessLogic.dataAccess.entities {
    export class menu extends base.BaseEntity implements base.IBaseEntity<menu> {
        public ParentId: number;
        public Name: string;
        public Description: string;
        public Parent: menu;
        public Children: Array<menu>;
        public Items: Array<menuContent>;
        public FreeTextField: string;
        public Lcid: number;
        public FolderId: number;
        public MenuPath: string;
        public Contents: Array<content>;
        public Options: string;
        public ParentArray: Array<menu>;
        public ChildrenTotalCount: number;
        public ContentsTotalCount: number;

        constructor(obj?: menu) {
            super(obj);
            this.ParentId = 0;
            this.Name = '';
            this.Description = '';
            this.Parent = null;
            this.Children = [];
            this.Items = [];
            this.FreeTextField = '';
            this.Lcid = 0;
            this.FolderId = 0;
            this.MenuPath = '';
            this.Contents = [];
            this.Options = '';
            this.ParentArray = new Array<menu>();
            this.ChildrenTotalCount = 0;
            this.ContentsTotalCount = 0;
            if (obj != undefined && obj != null) {
                this.construct(obj);
            }
        }

        construct(data: any): void {
            super.construct(data);
            this.ParentId = this.getValue<number>(data, "ParentId", 0);
            this.Name = this.getValue<string>(data, "Name", '');
            this.Description = this.getValue<string>(data, "Description", '');
            this.Parent = this.getConstructEntityValue<menu>(data, "Parent", new menu());
            this.Children = this.getArrayConstructEntityValue<menu>(data, "Children", new Array<menu>(), new menu());
            this.Items = this.getArrayConstructEntityValue<menuContent>(data, "Items", new Array<menuContent>(), new menuContent());
            this.Contents = this.getArrayConstructEntityValue<content>(data, "Contents", new Array<content>(), new content());
            this.FreeTextField = this.getValue<string>(data, "FreeTextField", '');
            this.Lcid = this.getValue<number>(data, "Lcid", 0);
            this.FolderId = this.getValue<number>(data, "FolderId", 0);
            this.MenuPath = this.getValue<string>(data, "MenuPath", '');
            this.Options = this.getValue<string>(data, "Options", '');
            this.ParentArray = helpers.loadParentArray(this, "Name", "MenuPath");
            this.ChildrenTotalCount = this.getValue<number>(this, "ChildrenTotalCount", 0);
            this.ContentsTotalCount = this.getValue<number>(this, "ContentsTotalCount", 0);
        }

        public clone(): menu {
            return new menu(this);
        }

    }
}