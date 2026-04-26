/// <reference path="./base/iBaseEntity.ts" />
/// <reference path="./base/baseEntity.ts" />
/// <reference path="./content.ts" />
/// <reference path="./mediaContent.ts" />
/// <reference path="./template.ts" />
/// <reference path="./profileType.ts" />
/// <reference path="./folderMetaDataField.ts" />
var __extends = (this && this.__extends) || (function () {
    var extendStatics = Object.setPrototypeOf ||
        ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
        function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var entities;
        (function (entities) {
            var folder = (function (_super) {
                __extends(folder, _super);
                function folder() {
                    var _this = _super.call(this) || this;
                    _this.ParentId = 0;
                    _this.Name = '';
                    _this.Description = '';
                    _this.Parent = null;
                    _this.Children = new Array();
                    _this.Contents = new Array();
                    _this.FolderPath = '';
                    _this.MetaDataFields = new Array();
                    _this.MediaContent = new Array();
                    _this.ProfileTypePermissions = new Array();
                    _this.NotAuthorizedUsers = new Array();
                    _this.FolderMediaContentMetaDataField = new Array();
                    _this.ContentTypeDefinitionFolder = new Array();
                    _this.ContentTypeDefinitions = new Array();
                    _this.ContentTypeDefinitionId = 0;
                    _this.Templates = new Array();
                    _this.Inherit = true;
                    _this.IsNew = true;
                    return _this;
                }
                folder.prototype.construct = function (data) {
                    _super.prototype.construct.call(this, data);
                    this.ParentId = this.getValue(data, "ParentId", 0);
                    this.Name = this.getValue(data, "Name", '');
                    this.Description = this.getValue(data, "Description", '');
                    this.Parent = this.getConstructEntityValue(data, "Parent", null);
                    this.Children = this.getArrayConstructEntityValue(data, "Children", new Array(), new folder());
                    this.Contents = this.getArrayConstructEntityValue(data, "Contents", new Array(), {});
                    this.FolderPath = this.getValue(data, "FolderPath", '');
                    this.MetaDataFields = this.getArrayConstructEntityValue(data, "MetaDataFields", new Array(), new entities.folderMetaDataField());
                    this.MediaContent = this.getArrayConstructEntityValue(data, "MediaContent", new Array(), new entities.mediaContent());
                    this.ProfileTypePermissions = this.getArrayConstructEntityValue(data, "ProfileTypePermissions", new Array(), new entities.profileType());
                    this.NotAuthorizedUsers = this.getArrayConstructEntityValue(data, "NotAuthorizedUsers", new Array(), new entities.user());
                    this.FolderMediaContentMetaDataField = this.getArrayConstructEntityValue(data, "FolderMediaContentMetaDataField", new Array(), new entities.folderMediaContentMetaDataField());
                    this.ContentTypeDefinitionFolder = this.getArrayConstructEntityValue(data, "ContentTypeDefinitionFolder", new Array(), new entities.contentTypeDefinitionFolder());
                    this.ContentTypeDefinitions = this.getArrayConstructEntityValue(data, "ContentTypeDefinitions", new Array(), new entities.contentTypeDefinition());
                    this.ContentTypeDefinitionId = this.getValue(data, "ContentTypeDefinitionId", 0);
                    this.Templates = this.getArrayConstructEntityValue(data, "Templates", new Array(), new entities.template());
                    this.Inherit = this.getValue(data, "Inherit", true);
                    this.IsNew = this.getValue(data, "IsNew", true);
                };
                return folder;
            }(entities.base.BaseEntity));
            entities.folder = folder;
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
//# sourceMappingURL=folder.js.map