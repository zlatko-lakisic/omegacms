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
            var reportDirectory = (function (_super) {
                __extends(reportDirectory, _super);
                function reportDirectory() {
                    var _this = _super.call(this) || this;
                    _this.Path = '';
                    _this.Children = new Array();
                    return _this;
                }
                reportDirectory.prototype.construct = function (data) {
                    _super.prototype.construct.call(this, data);
                    this.Path = this.getValue(data, "Path", '');
                    this.Children = this.getArrayConstructEntityValue(data, "Path", new Array(), new reportDirectory());
                };
                return reportDirectory;
            }(entities.base.BaseEntity));
            entities.reportDirectory = reportDirectory;
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
//# sourceMappingURL=reportDirectory.js.map