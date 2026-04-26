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
            var rwdPermission = (function (_super) {
                __extends(rwdPermission, _super);
                function rwdPermission() {
                    var _this = _super.call(this) || this;
                    _this.Read = false;
                    _this.Write = false;
                    _this.Delete = false;
                    _this.Target = 0;
                    _this.TargetPrimaryKey = '';
                    return _this;
                }
                rwdPermission.prototype.construct = function (data) {
                    _super.prototype.construct.call(this, data);
                    this.Read = this.getValue(data, "Read", false);
                    this.Write = this.getValue(data, "Write", false);
                    this.Delete = this.getValue(data, "Delete", false);
                    this.Target = this.getValue(data, "Target", 0);
                    this.TargetPrimaryKey = this.getValue(data, "TargetPrimaryKey", '');
                };
                return rwdPermission;
            }(entities.base.BaseEntity));
            entities.rwdPermission = rwdPermission;
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
//# sourceMappingURL=rwdPermission.js.map