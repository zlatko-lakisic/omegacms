/// <reference path="./base/iBaseEntity.ts" />
/// <reference path="./base/baseEntity.ts" />
/// <reference path="./user.ts" />
/// <reference path="./metaDataFieldValue.ts" />
/// <reference path="./template.ts" />
/// <reference path="./taxonomy.ts" />
/// <reference path="./contentAlias.ts" />
/// <reference path="./contentTypeDefinition.ts" />
/// <reference path="./contentTypeDefinitionFieldValue.ts" />
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
            var content = (function (_super) {
                __extends(content, _super);
                function content() {
                    var _this = _super.call(this) || this;
                    _this.LCID = 0;
                    _this.DateCreated = new Date();
                    _this.AuthorId = 0;
                    _this.FolderId = 0;
                    _this.Title = "";
                    _this.Path = "";
                    _this.Html = null;
                    _this.Author = null;
                    _this.ContentType = null;
                    _this.Taxonomy = new Array();
                    _this.MetaDataFieldValues = new Array();
                    _this.ContentAliases = new Array();
                    _this.Template = null;
                    _this.IsNew = false;
                    _this.IsPublished = false;
                    return _this;
                }
                content.prototype.construct = function (data) {
                    _super.prototype.construct.call(this, data);
                    this.LCID = this.getValue(data, 'LCID', 0);
                    this.DateCreated = this.getValue(data, 'DateCreated', null);
                    this.AuthorId = this.getValue(data, 'AuthorId', 0);
                    this.FolderId = this.getValue(data, 'FolderId', 0);
                    this.Title = this.getValue(data, 'Title', '');
                    this.Path = this.getValue(data, 'Path', '');
                    this.Html = this.getValue(data, 'Html', '');
                    this.Author = this.getConstructEntityValue(data, 'Author', new entities.user());
                    this.ContentType = this.getConstructEntityValue(data, 'ContentType', new entities.contentTypeDefinition());
                    this.Taxonomy = this.getArrayConstructEntityValue(data, 'Taxonomy', new Array(), new entities.taxonomy());
                    this.MetaDataFieldValues = this.getArrayConstructEntityValue(data, 'MetaDataFieldValues', new Array(), new entities.metaDataFieldValue());
                    this.ContentAliases = this.getArrayConstructEntityValue(data, 'ContentAliases', new Array(), new entities.contentAlias());
                    this.Template = this.getConstructEntityValue(data, 'Template', new entities.template());
                    this.IsNew = this.getValue(data, 'IsNew', false);
                    this.IsPublished = this.getValue(data, 'IsPublished', false);
                };
                return content;
            }(entities.base.BaseEntity));
            entities.content = content;
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
//# sourceMappingURL=content.js.map