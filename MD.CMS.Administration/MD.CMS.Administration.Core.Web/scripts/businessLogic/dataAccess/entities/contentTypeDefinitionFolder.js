/// <reference path="./base/iBaseEntity.ts" />
/// <reference path="./base/baseEntity.ts" />
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
            var contentTypeDefinitionFolder = (function (_super) {
                __extends(contentTypeDefinitionFolder, _super);
                function contentTypeDefinitionFolder() {
                    var _this = _super.call(this) || this;
                    _this.FolderId = 0;
                    _this.ContentTypeDefinitionId = 0;
                    _this.Title = '';
                    return _this;
                }
                contentTypeDefinitionFolder.prototype.construct = function (data) {
                    _super.prototype.construct.call(this, data);
                    this.FolderId = this.getValue(data, "FolderId", 0);
                    this.ContentTypeDefinitionId = this.getValue(data, "ContentTypeDefinitionId", 0);
                    this.Title = this.getValue(data, "Title", '');
                };
                return contentTypeDefinitionFolder;
            }(entities.base.BaseEntity));
            entities.contentTypeDefinitionFolder = contentTypeDefinitionFolder;
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
//# sourceMappingURL=contentTypeDefinitionFolder.js.map