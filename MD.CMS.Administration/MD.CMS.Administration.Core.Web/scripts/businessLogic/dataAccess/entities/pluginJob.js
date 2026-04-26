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
            var pluginJob = (function (_super) {
                __extends(pluginJob, _super);
                function pluginJob() {
                    var _this = _super.call(this) || this;
                    _this.PluginName = '';
                    _this.Message = '';
                    _this.StartedOn = null;
                    return _this;
                }
                pluginJob.prototype.construct = function (data) {
                    _super.prototype.construct.call(this, data);
                    this.PluginName = this.getValue(data, "PluginName", '');
                    this.Message = this.getValue(data, "Message", '');
                    this.StartedOn = this.getValue(data, "StartedOn", null);
                };
                return pluginJob;
            }(entities.base.BaseEntity));
            entities.pluginJob = pluginJob;
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
//# sourceMappingURL=pluginJob.js.map