/// <reference path="./base/iBaseEntity.ts" />
/// <reference path="./base/BaseEntity.ts" />
/// <reference path="./../../helpers.ts" />
/// <reference path="./taxonomyContent.ts" />
/// <reference path="./content.ts" />
namespace mdBusinessLogic.dataAccess.entities {
    export class taxonomy extends base.BaseEntity implements base.IBaseEntity<taxonomy>{
        public ParentId: number;
        public Name: string;
        public Description: string;
        public Parent: taxonomy;
        public Children: Array<taxonomy>;
        public Items: Array<taxonomyContent>;
        public FreeTextField: string;
        public Lcid: number;
        public FolderId: number;
        public TaxonomyPath: string;
        public Contents: Array<content>;
        public ParentArray: Array<taxonomy>;
        public ChildrenTotalCount: number;
        public ItemsTotalCount: number
        public Order: number;

        constructor(obj?: taxonomy) {
            super(obj);
            this.ParentId = 0;
            this.Name = '';
            this.Description = '';
            this.Parent = null;
            this.Children = new Array<taxonomy>();
            this.Items = new Array<taxonomyContent>();
            this.FreeTextField = '';
            this.Lcid = 0;
            this.FolderId = 0;
            this.TaxonomyPath = '';
            this.Contents = new Array<content>();
            this.ParentArray = new Array<taxonomy>();
            this.ChildrenTotalCount = 0;
            this.ItemsTotalCount = 0;
            this.Order = 0;
            if (obj != undefined && obj != null) {
                this.construct(obj);
            }
        }

        public construct(data: any) {
            super.construct(data);
            this.ParentId = this.getValue<number>(data, 'ParentId', 0);
            this.Name = this.getValue<string>(data, 'Name', '');
            this.Description = this.getValue<string>(data, 'Description', '');
            this.Parent = this.getValue<taxonomy>(data, 'Parent', new taxonomy());
            this.Children = this.getArrayConstructEntityValue<taxonomy>(data, 'Children', new Array<taxonomy>(), new taxonomy());
            this.Items = this.getArrayConstructEntityValue<taxonomyContent>(data, 'Items', new Array<taxonomyContent>(), new taxonomyContent());
            this.FreeTextField = this.getValue<string>(data, 'FreeTextField', '');
            this.Lcid = this.getValue<number>(data, 'Lcid', 0);
            this.FolderId = this.getValue<number>(data, 'FolderId', 0);
            this.TaxonomyPath = this.getValue<string>(data, 'TaxonomyPath', '');
            this.Contents = this.getArrayConstructEntityValue<content>(data, 'Contents', new Array<content>(), new content());
            this.ParentArray = helpers.loadParentArray(this, "Name", "TaxonomyPath");
            this.ChildrenTotalCount = this.getValue<number>(data, "ChildrenTotalCount", 0);
            this.ItemsTotalCount = this.getValue<number>(data, "ItemsTotalCount", 0);
            this.Order = this.getValue<number>(data, "Order", 0);
        }

        public clone(): taxonomy {
            return new taxonomy(this);
        }
    }
}