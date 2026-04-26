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
            var folderMetaDataField = (function (_super) {
                __extends(folderMetaDataField, _super);
                function folderMetaDataField() {
                    var _this = _super.call(this) || this;
                    _this.FolderId = 0;
                    _this.MetaDataFieldId = 0;
                    _this.IsRequired = false;
                    _this.Checked = false;
                    _this.Name = '';
                    return _this;
                }
                folderMetaDataField.prototype.construct = function (data) {
                    _super.prototype.construct.call(this, data);
                    this.FolderId = this.getValue(data, "FolderId", 0);
                    this.MetaDataFieldId = this.getValue(data, "MetaDataFieldId", 0);
                    this.IsRequired = this.getValue(data, "IsRequired", false);
                    this.Checked = this.getValue(data, "Checked", false);
                    this.Name = this.getValue(data, "Name", '');
                };
                return folderMetaDataField;
            }(entities.base.BaseEntity));
            entities.folderMetaDataField = folderMetaDataField;
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
//# sourceMappingURL=folderMetaDataField.js.map