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
            var hardwareInfoNetworkInterface = (function (_super) {
                __extends(hardwareInfoNetworkInterface, _super);
                function hardwareInfoNetworkInterface() {
                    var _this = _super.call(this) || this;
                    _this.Name = '';
                    _this.Description = '';
                    _this.SentMb = 0;
                    _this.ReceivedMb = 0;
                    _this.SentGb = 0;
                    _this.ReceivedGb = 0;
                    _this.NetworkUtilization = 0;
                    return _this;
                }
                hardwareInfoNetworkInterface.prototype.construct = function (data) {
                    _super.prototype.construct.call(this, data);
                    this.Name = this.getValue(data, "Name", '');
                    this.Description = this.getValue(data, "Description", '');
                    this.SentMb = this.getValue(data, "SentMb", 0);
                    this.ReceivedMb = this.getValue(data, "ReceivedMb", 0);
                    this.SentGb = this.getValue(data, "SentGb", 0);
                    this.ReceivedGb = this.getValue(data, "ReceivedGb", 0);
                    this.NetworkUtilization = this.getValue(data, "NetworkUtilization", 0);
                };
                return hardwareInfoNetworkInterface;
            }(entities.base.BaseEntity));
            entities.hardwareInfoNetworkInterface = hardwareInfoNetworkInterface;
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
//# sourceMappingURL=hardwareInfoNetworkInterface.js.map