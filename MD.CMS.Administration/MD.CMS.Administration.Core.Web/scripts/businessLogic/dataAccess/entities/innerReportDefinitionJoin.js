/// <reference path="./base/iBaseEntity.ts" />
/// <reference path="./base/baseEntity.ts" />
/// <reference path="./innerReportDefinitionJoinInner.ts" />
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
            var innerReportDefinitionJoin = (function (_super) {
                __extends(innerReportDefinitionJoin, _super);
                function innerReportDefinitionJoin() {
                    var _this = _super.call(this) || this;
                    _this.Left = new entities.innerReportDefinitionJoinInner();
                    _this.Right = new entities.innerReportDefinitionJoinInner();
                    _this.Type = 0;
                    return _this;
                }
                innerReportDefinitionJoin.prototype.construct = function (data) {
                    _super.prototype.construct.call(this, data);
                    this.Left = this.getConstructEntityValue(data, "Left", new entities.innerReportDefinitionJoinInner());
                    this.Right = this.getConstructEntityValue(data, "Right", new entities.innerReportDefinitionJoinInner());
                    this.Type = this.getValue(data, "Type", 0);
                };
                return innerReportDefinitionJoin;
            }(entities.base.BaseEntity));
            entities.innerReportDefinitionJoin = innerReportDefinitionJoin;
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
//# sourceMappingURL=innerReportDefinitionJoin.js.map