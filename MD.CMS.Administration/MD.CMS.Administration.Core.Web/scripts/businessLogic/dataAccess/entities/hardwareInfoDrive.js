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
            var hardwareInfoDrive = (function (_super) {
                __extends(hardwareInfoDrive, _super);
                function hardwareInfoDrive() {
                    var _this = _super.call(this) || this;
                    _this.Label = '';
                    _this.TotalSizeMb = 0;
                    _this.AvaliableSizeMb = 0;
                    _this.UsedSizeMb = 0;
                    _this.TotalSizeGb = 0;
                    _this.AvaliableSizeGb = 0;
                    _this.UsedSizeGb = 0;
                    _this.Format = '';
                    return _this;
                }
                hardwareInfoDrive.prototype.construct = function (data) {
                    _super.prototype.construct.call(this, data);
                    this.Label = this.getValue(data, "Label", '');
                    this.TotalSizeMb = this.getValue(data, "TotalSizeMb", 0);
                    this.AvaliableSizeMb = this.getValue(data, "AvaliableSizeMb", 0);
                    this.UsedSizeMb = this.getValue(data, "UsedSizeMb", 0);
                    this.TotalSizeGb = this.getValue(data, "TotalSizeGb", 0);
                    this.AvaliableSizeGb = this.getValue(data, "AvaliableSizeGb", 0);
                    this.UsedSizeGb = this.getValue(data, "UsedSizeGb", 0);
                    this.Format = this.getValue(data, "Format", '');
                };
                return hardwareInfoDrive;
            }(entities.base.BaseEntity));
            entities.hardwareInfoDrive = hardwareInfoDrive;
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
//# sourceMappingURL=hardwareInfoDrive.js.map