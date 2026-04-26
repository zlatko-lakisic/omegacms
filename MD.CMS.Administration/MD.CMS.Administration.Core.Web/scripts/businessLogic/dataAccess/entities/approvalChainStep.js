/// <reference path="./base/iBaseEntity.ts" />
/// <reference path="./base/baseEntity.ts" />
/// <reference path="./approvalChainStepAction.ts" />
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
            var approvalChainStep = (function (_super) {
                __extends(approvalChainStep, _super);
                function approvalChainStep() {
                    var _this = _super.call(this) || this;
                    _this.ApprovalChainId = 0;
                    _this.ComboOperator = 0;
                    _this.Order = 0;
                    _this.UserIds = [];
                    _this.Actions = [];
                    return _this;
                }
                approvalChainStep.prototype.construct = function (data) {
                    _super.prototype.construct.call(this, data);
                    this.ApprovalChainId = this.getValue(data, "ApprovalChainId", 0);
                    this.ComboOperator = this.getValue(data, "ComboOperator", 0);
                    this.Order = this.getValue(data, "Order", 0);
                    this.UserIds = this.getValue(data, "UserIds", new Array());
                    this.Actions = this.getArrayConstructEntityValue(data, "Actions", new Array(), new entities.approvalChainStepAction());
                };
                return approvalChainStep;
            }(entities.base.BaseEntity));
            entities.approvalChainStep = approvalChainStep;
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
//# sourceMappingURL=approvalChainStep.js.map