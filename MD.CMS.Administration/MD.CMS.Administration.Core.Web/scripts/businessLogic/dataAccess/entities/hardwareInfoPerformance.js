/// <reference path="./base/iBaseEntity.ts" />
/// <reference path="./base/baseEntity.ts" />
/// <reference path="./hardwareInfoDrive.ts" />
/// <reference path="./hardwareInfoNetworkInterface.ts" />
/// <reference path="./hardwareInfoProcess.ts" />
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
            var hardwareInfoPerformance = (function (_super) {
                __extends(hardwareInfoPerformance, _super);
                function hardwareInfoPerformance() {
                    var _this = _super.call(this) || this;
                    _this.SimpleDateTime = '';
                    _this.CpuUsage = 0;
                    _this.FreeMemoryMb = 0;
                    _this.TotalMemoryMb = 0;
                    _this.UsedMemoryMb = 0;
                    _this.Drives = new Array();
                    _this.NetworkInterfaces = new Array();
                    _this.Processes = new Array();
                    return _this;
                }
                hardwareInfoPerformance.prototype.construct = function (data) {
                    _super.prototype.construct.call(this, data);
                    this.SimpleDateTime = this.getValue(data, "SimpleDateTime", '');
                    this.CpuUsage = this.getValue(data, "CpuUsage", 0);
                    this.FreeMemoryMb = this.getValue(data, "FreeMemoryMb", 0);
                    this.TotalMemoryMb = this.getValue(data, "TotalMemoryMb", 0);
                    this.UsedMemoryMb = this.getValue(data, "UsedMemoryMb", 0);
                    this.Drives = this.getArrayConstructEntityValue(data, "Drives", new Array(), new entities.hardwareInfoDrive());
                    this.NetworkInterfaces = this.getArrayConstructEntityValue(data, "NetworkInterfaces", new Array(), new entities.hardwareInfoNetworkInterface());
                    this.Processes = this.getArrayConstructEntityValue(data, "Processes", new Array(), new entities.hardwareInfoProcess());
                };
                return hardwareInfoPerformance;
            }(entities.base.BaseEntity));
            entities.hardwareInfoPerformance = hardwareInfoPerformance;
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
//# sourceMappingURL=hardwareInfoPerformance.js.map