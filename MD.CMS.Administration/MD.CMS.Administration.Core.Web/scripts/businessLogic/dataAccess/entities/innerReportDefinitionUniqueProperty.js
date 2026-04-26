/// <reference path="./base/iBaseEntity.ts" />
/// <reference path="./base/baseEntity.ts" />
/// <reference path="./innerReportDefinitionProperty.ts" />
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
            var innerReportDefinitionUniqueProperty = (function (_super) {
                __extends(innerReportDefinitionUniqueProperty, _super);
                function innerReportDefinitionUniqueProperty() {
                    var _this = _super.call(this) || this;
                    _this.UniqueId = '';
                    _this.Property = new entities.innerReportDefinitionProperty();
                    return _this;
                }
                innerReportDefinitionUniqueProperty.prototype.construct = function (data) {
                    _super.prototype.construct.call(this, data);
                    this.UniqueId = this.getValue(data, "UniqueId", '');
                    this.Property = this.getConstructEntityValue(data, "Property", new entities.innerReportDefinitionProperty());
                };
                return innerReportDefinitionUniqueProperty;
            }(entities.base.BaseEntity));
            entities.innerReportDefinitionUniqueProperty = innerReportDefinitionUniqueProperty;
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
//# sourceMappingURL=innerReportDefinitionUniqueProperty.js.map