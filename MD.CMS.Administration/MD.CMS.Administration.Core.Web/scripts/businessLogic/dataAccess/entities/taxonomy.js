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
/// <reference path="./base/iBaseEntity.ts" />
/// <reference path="./base/BaseEntity.ts" />
/// <reference path="./taxonomyContent.ts" />
/// <reference path="./content.ts" />
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var entities;
        (function (entities) {
            var taxonomy = (function (_super) {
                __extends(taxonomy, _super);
                function taxonomy() {
                    var _this = _super.call(this) || this;
                    _this.ParentId = 0;
                    _this.Name = '';
                    _this.Description = '';
                    _this.Parent = null;
                    _this.Children = new Array();
                    _this.Items = new Array();
                    _this.FreeTextField = '';
                    _this.Lcid = 0;
                    _this.FolderId = 0;
                    _this.TaxonomyPath = '';
                    _this.Contents = new Array();
                    return _this;
                }
                taxonomy.prototype.construct = function (data) {
                    _super.prototype.construct.call(this, data);
                    this.ParentId = this.getValue(data, 'ParentId', 0);
                    this.Name = this.getValue(data, 'Name', '');
                    this.Description = this.getValue(data, 'Description', '');
                    this.Parent = this.getValue(data, 'Parent', new taxonomy());
                    this.Children = this.getArrayConstructEntityValue(data, 'Children', new Array(), new taxonomy());
                    this.Items = this.getArrayConstructEntityValue(data, 'Items', new Array(), new entities.taxonomyContent());
                    this.FreeTextField = this.getValue(data, 'FreeTextField', '');
                    this.Lcid = this.getValue(data, 'Lcid', 0);
                    this.FolderId = this.getValue(data, 'FolderId', 0);
                    this.TaxonomyPath = this.getValue(data, 'TaxonomyPath', '');
                    this.Contents = this.getArrayConstructEntityValue(data, 'Contents', new Array(), new entities.content());
                };
                return taxonomy;
            }(entities.base.BaseEntity));
            entities.taxonomy = taxonomy;
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
//# sourceMappingURL=taxonomy.js.map