/// <reference path="../base/iBaseEntity.ts" />
/// <reference path="../base/BaseEntity.ts" />
/// <reference path="../../../helpers/entityHelper.ts" />
namespace mdBusinessLogic.dataAccess.entities.search {
    export class searchResults implements base.IBaseEntity<searchResults> {
        public Folders: Array<folder>;
        public Taxonomies: Array<taxonomy>;
        public Menus: Array<menu>;
        public Contents: Array<content>;
        public ContentTypes: Array<contentType>;
        public ProfileTypes: Array<profileType>;
        public MediaContents: Array<mediaContent>;

        constructor(obj?: searchResults) {
            this.Folders = new Array<folder>();
            this.Taxonomies = new Array<taxonomy>();
            this.Menus = new Array<menu>();
            this.Contents = new Array<content>();
            this.ContentTypes = new Array<contentType>();
            this.ProfileTypes = new Array<profileType>();
            this.MediaContents = new Array<mediaContent>();
            if (obj !== undefined && obj != null) {
                this.Folders = obj.Folders;
                this.Taxonomies = obj.Taxonomies;
                this.Menus = obj.Menus;
                this.Contents = obj.Contents;
                this.ContentTypes = obj.ContentTypes
                this.ProfileTypes = obj.ProfileTypes;
                this.MediaContents = obj.MediaContents;
            }
        }

        public construct(data: any) {
            this.Folders = mdBusinessLogic.helpers.entityHelper.getArrayConstructValue<folder>(data, 'Folders', new Array<folder>(), new folder());
            this.Taxonomies = mdBusinessLogic.helpers.entityHelper.getArrayConstructValue<taxonomy>(data, 'Taxonomies', new Array<taxonomy>(), new taxonomy());
            this.Menus = mdBusinessLogic.helpers.entityHelper.getArrayConstructValue<menu>(data, 'Menus', new Array<menu>(), new menu());
            this.Contents = mdBusinessLogic.helpers.entityHelper.getArrayConstructValue<content>(data, 'Contents', new Array<content>(), new content());
            this.ContentTypes = mdBusinessLogic.helpers.entityHelper.getArrayConstructValue<contentType>(data, 'ContentTypes', new Array<contentType>(), new contentType());
            this.ProfileTypes = mdBusinessLogic.helpers.entityHelper.getArrayConstructValue<profileType>(data, 'ProfileTypes', new Array<profileType>(), new profileType());
            this.MediaContents = mdBusinessLogic.helpers.entityHelper.getArrayConstructValue<mediaContent>(data, 'MediaContents', new Array<mediaContent>(), new mediaContent());
        }

        public clone(): searchResults {
            return new searchResults(this);
        }
    }
}