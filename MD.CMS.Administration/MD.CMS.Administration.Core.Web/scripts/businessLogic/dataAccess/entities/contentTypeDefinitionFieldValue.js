/// <reference path="./base/iBaseEntity.ts" />
/// <reference path="./base/baseEntity.ts" />
/// <reference path="./contentTypeDefinitionField.ts" />
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
            var contentTypeDefinitionFieldValue = (function (_super) {
                __extends(contentTypeDefinitionFieldValue, _super);
                function contentTypeDefinitionFieldValue() {
                    var _this = _super.call(this) || this;
                    _this.ContentId = 0;
                    _this.LCID = 0;
                    _this.DateCreated = new Date();
                    _this.Value = '';
                    _this.ValueContentTypeDefinitionFieldId = 0;
                    _this.ValueContentTypeDefinitionId = 0;
                    return _this;
                }
                contentTypeDefinitionFieldValue.prototype.construct = function (data) {
                    _super.prototype.construct.call(this, data);
                    this.ContentId = this.getValue(data, 'ContentId', 0);
                    this.LCID = this.getValue(data, 'LCID', 0);
                    this.DateCreated = this.getValue(data, 'DateCreated', new Date());
                    this.Value = this.getValue(data, 'Value', '');
                    this.ValueContentTypeDefinitionFieldId = this.getValue(data, 'ValueContentTypeDefinitionFieldId', 0);
                    this.ValueContentTypeDefinitionId = this.getValue(data, 'ValueContentTypeDefinitionId', 0);
                };
                return contentTypeDefinitionFieldValue;
            }(entities.contentTypeDefinitionField));
            entities.contentTypeDefinitionFieldValue = contentTypeDefinitionFieldValue;
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
//# sourceMappingURL=contentTypeDefinitionFieldValue.js.map