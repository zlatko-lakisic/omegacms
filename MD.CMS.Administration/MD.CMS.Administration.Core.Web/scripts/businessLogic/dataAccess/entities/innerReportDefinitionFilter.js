/// <reference path="./base/iBaseEntity.ts" />
/// <reference path="./innerReportDefinitionUniqueProperty.ts" />
/// <reference path="./innerReportDefinitionEntity.ts" />
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
            var innerReportDefinitionFilter = (function (_super) {
                __extends(innerReportDefinitionFilter, _super);
                function innerReportDefinitionFilter() {
                    var _this = _super.call(this) || this;
                    _this.Type = 0;
                    _this.Value = '';
                    _this.Entity = new entities.innerReportDefinitionEntity();
                    _this.Property = new entities.innerReportDefinitionProperty();
                    _this.IsDynamic = false;
                    return _this;
                }
                innerReportDefinitionFilter.prototype.construct = function (data) {
                    _super.prototype.construct.call(this, data);
                    this.Type = this.getValue(data, "Type", 0);
                    this.Entity = this.getConstructEntityValue(data, "Entity", new entities.innerReportDefinitionEntity());
                    this.Value = this.getValue(data, "Value", '');
                    this.Property = this.getConstructEntityValue(data, "Property", new entities.innerReportDefinitionProperty());
                    this.IsDynamic = this.getValue(data, "IsDynamic", false);
                };
                return innerReportDefinitionFilter;
            }(entities.innerReportDefinitionUniqueProperty));
            entities.innerReportDefinitionFilter = innerReportDefinitionFilter;
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
//# sourceMappingURL=innerReportDefinitionFilter.js.map