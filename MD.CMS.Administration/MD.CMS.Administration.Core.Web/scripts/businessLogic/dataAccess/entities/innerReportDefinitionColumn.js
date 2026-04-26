/// <reference path="./base/iBaseEntity.ts" />
/// <reference path="./innerReportDefinitionUniqueProperty.ts" />
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
            var innerReportDefinitionColumn = (function (_super) {
                __extends(innerReportDefinitionColumn, _super);
                function innerReportDefinitionColumn() {
                    var _this = _super.call(this) || this;
                    _this.Type = 0;
                    _this.Value = '';
                    return _this;
                }
                innerReportDefinitionColumn.prototype.construct = function (data) {
                    _super.prototype.construct.call(this, data);
                    this.Type = this.getValue(data, "Type", 0);
                    this.Value = this.getValue(data, "Value", '');
                };
                return innerReportDefinitionColumn;
            }(entities.innerReportDefinitionUniqueProperty));
            entities.innerReportDefinitionColumn = innerReportDefinitionColumn;
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
//# sourceMappingURL=innerReportDefinitionColumn.js.map