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
            var reportData = (function (_super) {
                __extends(reportData, _super);
                function reportData() {
                    var _this = _super.call(this) || this;
                    _this.DateCreated = new Date();
                    _this.Data = '';
                    return _this;
                }
                reportData.prototype.construct = function (data) {
                    _super.prototype.construct.call(this, data);
                    this.ReportId = this.getValue(data, "ReportId", 0);
                    this.DateCreated = this.getValue(data, "DateCreated", new Date());
                    this.Data = this.getValue(data, "Data", null);
                };
                return reportData;
            }(entities.base.BaseEntity));
            entities.reportData = reportData;
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
//# sourceMappingURL=reportData.js.map