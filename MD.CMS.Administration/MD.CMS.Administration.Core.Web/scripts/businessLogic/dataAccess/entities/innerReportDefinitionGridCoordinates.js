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
            var innerReportDefinitionGridCoordinates = (function (_super) {
                __extends(innerReportDefinitionGridCoordinates, _super);
                function innerReportDefinitionGridCoordinates() {
                    var _this = _super.call(this) || this;
                    _this.X = 0;
                    _this.Y = 0;
                    return _this;
                }
                innerReportDefinitionGridCoordinates.prototype.construct = function (data) {
                    _super.prototype.construct.call(this, data);
                    this.X = this.getValue(data, "X", 0);
                    this.Y = this.getValue(data, "Y", 0);
                };
                return innerReportDefinitionGridCoordinates;
            }(entities.base.BaseEntity));
            entities.innerReportDefinitionGridCoordinates = innerReportDefinitionGridCoordinates;
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
//# sourceMappingURL=innerReportDefinitionGridCoordinates.js.map