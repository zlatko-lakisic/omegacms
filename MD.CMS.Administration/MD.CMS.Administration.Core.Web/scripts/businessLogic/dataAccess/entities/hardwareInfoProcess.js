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
            var hardwareInfoProcess = (function (_super) {
                __extends(hardwareInfoProcess, _super);
                function hardwareInfoProcess() {
                    var _this = _super.call(this) || this;
                    _this.Name = '';
                    _this.User = '';
                    _this.ProcessorUsage = 0;
                    _this.MemoryUsageMb = 0;
                    return _this;
                }
                hardwareInfoProcess.prototype.construct = function (data) {
                    _super.prototype.construct.call(this, data);
                    this.Name = this.getValue(data, "Name", '');
                    this.User = this.getValue(data, "User", '');
                    this.ProcessorUsage = this.getValue(data, "ProcessorUsage", 0);
                    this.MemoryUsageMb = this.getValue(data, "MemoryUsageMb", 0);
                };
                return hardwareInfoProcess;
            }(entities.base.BaseEntity));
            entities.hardwareInfoProcess = hardwareInfoProcess;
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
//# sourceMappingURL=hardwareInfoProcess.js.map