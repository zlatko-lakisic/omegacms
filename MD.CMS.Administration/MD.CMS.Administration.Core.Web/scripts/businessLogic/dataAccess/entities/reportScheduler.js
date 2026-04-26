/// <reference path="./base/iBaseEntity.ts" />
/// <reference path="./base/baseEntity.ts" />
/// <reference path="./user.ts" />
/// <reference path="./reportSchedulerAction.ts" />
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
            var reportScheduler = (function (_super) {
                __extends(reportScheduler, _super);
                function reportScheduler() {
                    var _this = _super.call(this) || this;
                    _this.Name = '';
                    _this.AuthorId = 0;
                    _this.DateCreated = new Date();
                    _this.IsRecurring = false;
                    _this.Interval = 0;
                    _this.Start = null;
                    _this.End = null;
                    _this.ReportId = 0;
                    _this.IsActive = false;
                    _this.Actions = new Array();
                    _this.Author = new entities.user();
                    return _this;
                }
                reportScheduler.prototype.construct = function (data) {
                    _super.prototype.construct.call(this, data);
                    this.Name = this.getValue(data, "Name", '');
                    this.AuthorId = this.getValue(data, "AuthorId", 0);
                    this.DateCreated = this.getValue(data, "DateCreated", new Date());
                    this.IsRecurring = this.getValue(data, "IsRecurring", false);
                    this.Interval = this.getValue(data, "Interval", 0);
                    this.Start = this.getValue(data, "Start", null);
                    this.End = this.getValue(data, "End", null);
                    this.ReportId = this.getValue(data, "ReportId", 0);
                    this.IsActive = this.getValue(data, "IsActive", false);
                    this.Actions = this.getArrayConstructEntityValue(data, "Actions", new Array(), new entities.reportSchedulerAction());
                    this.Author = this.getConstructEntityValue(data, "Author", new entities.user());
                };
                return reportScheduler;
            }(entities.base.BaseEntity));
            entities.reportScheduler = reportScheduler;
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
//# sourceMappingURL=reportScheduler.js.map