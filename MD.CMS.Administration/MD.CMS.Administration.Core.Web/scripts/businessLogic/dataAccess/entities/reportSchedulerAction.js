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
            var reportSchedulerAction = (function (_super) {
                __extends(reportSchedulerAction, _super);
                function reportSchedulerAction() {
                    var _this = _super.call(this) || this;
                    _this.SchedulerId = 0;
                    _this.Name = '';
                    _this.AuthorId = 0;
                    _this.DateCreated = new Date();
                    _this.DateEdited = null;
                    _this.ActionType = 0;
                    _this.Options = '';
                    _this.IsActive = false;
                    return _this;
                }
                reportSchedulerAction.prototype.construct = function (data) {
                    _super.prototype.construct.call(this, data);
                    this.SchedulerId = this.getValue(data, "SchedulerId", 0);
                    this.Name = this.getValue(data, "Name", '');
                    this.AuthorId = this.getValue(data, "AuthorId", 0);
                    this.DateCreated = this.getValue(data, "DateCreated", new Date());
                    this.DateEdited = this.getValue(data, "DateEdited", null);
                    this.ActionType = this.getValue(data, "ActionType", 0);
                    this.Options = this.getValue(data, "Options", '');
                    this.IsActive = this.getValue(data, "IsActive", false);
                };
                return reportSchedulerAction;
            }(entities.base.BaseEntity));
            entities.reportSchedulerAction = reportSchedulerAction;
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
//# sourceMappingURL=reportSchedulerAction.js.map