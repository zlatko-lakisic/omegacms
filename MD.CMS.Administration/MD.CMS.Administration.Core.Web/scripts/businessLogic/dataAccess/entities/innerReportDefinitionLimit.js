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
            var innerReportDefinitionLimit = (function (_super) {
                __extends(innerReportDefinitionLimit, _super);
                function innerReportDefinitionLimit() {
                    var _this = _super.call(this) || this;
                    _this.From = 0;
                    _this.To = 0;
                    return _this;
                }
                innerReportDefinitionLimit.prototype.construct = function (data) {
                    _super.prototype.construct.call(this, data);
                    this.From = this.getValue(data, "From", 0);
                    this.To = this.getValue(data, "To", 0);
                };
                return innerReportDefinitionLimit;
            }(entities.base.BaseEntity));
            entities.innerReportDefinitionLimit = innerReportDefinitionLimit;
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
//# sourceMappingURL=innerReportDefinitionLimit.js.map