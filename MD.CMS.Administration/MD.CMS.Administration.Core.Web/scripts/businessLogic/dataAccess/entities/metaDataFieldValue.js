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
/// <reference path="./base/baseEntity.ts" />
/// <reference path="./metaDataField.ts" />
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var entities;
        (function (entities) {
            var metaDataFieldValue = (function (_super) {
                __extends(metaDataFieldValue, _super);
                function metaDataFieldValue() {
                    var _this = _super.call(this) || this;
                    _this.ContentId = 0;
                    _this.LCID = 0;
                    _this.DateCreated = new Date();
                    _this.Value = '';
                    return _this;
                }
                metaDataFieldValue.prototype.construct = function (data) {
                    _super.prototype.construct.call(this, data);
                    this.ContentId = this.getValue(data, 'ContentId', 0);
                    this.LCID = this.getValue(data, 'LCID', 0);
                    this.DateCreated = this.getValue(data, 'DateCreated', new Date());
                    this.Value = this.getValue(data, 'Value', null);
                };
                return metaDataFieldValue;
            }(entities.metaDataField));
            entities.metaDataFieldValue = metaDataFieldValue;
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
//# sourceMappingURL=metaDataFieldValue.js.map