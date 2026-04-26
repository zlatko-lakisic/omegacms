/// <reference path="./base/iBaseEntity.ts" />
/// <reference path="./base/baseEntity.ts" />
/// <reference path="./content.ts" />
/// <reference path="./user.ts" />
/// <reference path="./approvalChainStep.ts" />
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
            var approvalChainApproval = (function (_super) {
                __extends(approvalChainApproval, _super);
                function approvalChainApproval() {
                    var _this = _super.call(this) || this;
                    _this.ApprovalType = 1;
                    _this.ReviewDate = null;
                    _this.Content = null;
                    _this.Comment = '';
                    _this.User = null;
                    _this.Step = null;
                    return _this;
                }
                approvalChainApproval.prototype.construct = function (data) {
                    _super.prototype.construct.call(this, data);
                    this.ApprovalType = this.getValue(data, "ApprovalType", 1);
                    this.ReviewDate = this.getValue(data, "ReviewDate", null);
                    this.Content = this.getValue(data, "Content", new entities.content());
                    this.Comment = this.getValue(data, "Comment", '');
                    this.User = this.getValue(data, "User", new entities.user());
                    this.Step = this.getValue(data, "Step", new entities.approvalChainStep());
                };
                return approvalChainApproval;
            }(entities.base.BaseEntity));
            entities.approvalChainApproval = approvalChainApproval;
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
//# sourceMappingURL=approvalChainApproval.js.map