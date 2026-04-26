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
            var approvalChain = (function (_super) {
                __extends(approvalChain, _super);
                function approvalChain() {
                    var _this = _super.call(this) || this;
                    _this.FolderId = 0;
                    _this.IsActive = false;
                    _this.Steps = [];
                    _this.ChainId = 0;
                    return _this;
                }
                approvalChain.prototype.construct = function (data) {
                    _super.prototype.construct.call(this, data);
                    this.FolderId = this.getValue(data, "FolderId", 0);
                    this.IsActive = this.getValue(data, "IsActive", false);
                    this.Steps = this.getValue(data, "Steps", []);
                    this.ChainId = this.getValue(data, "ChainId", 0);
                };
                return approvalChain;
            }(entities.base.BaseEntity));
            entities.approvalChain = approvalChain;
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
//# sourceMappingURL=approvalChain.js.map