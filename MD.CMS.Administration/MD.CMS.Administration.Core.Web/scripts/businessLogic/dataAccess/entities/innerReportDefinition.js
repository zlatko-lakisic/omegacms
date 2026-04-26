/// <reference path="./base/iBaseEntity.ts" />
/// <reference path="./base/baseEntity.ts" />
/// <reference path="./innerReportDefinitionEntity.ts" />
/// <reference path="./innerReportDefinitionJoin.ts" />
/// <reference path="./innerReportDefinitionColumn.ts" />
/// <reference path="./innerReportDefinitionFilter.ts" />
/// <reference path="./innerReportDefinitionGroup.ts" />
/// <reference path="./innerReportDefinitionLimit.ts" />
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
            var innerReportDefinition = (function (_super) {
                __extends(innerReportDefinition, _super);
                function innerReportDefinition() {
                    var _this = _super.call(this) || this;
                    _this.Entities = new Array();
                    _this.Joins = new Array();
                    _this.Columns = new Array();
                    _this.Filters = new Array();
                    _this.Groupings = new Array();
                    _this.Limit = new entities.innerReportDefinitionLimit();
                    return _this;
                }
                innerReportDefinition.prototype.construct = function (data) {
                    _super.prototype.construct.call(this, data);
                    this.Entities = this.getArrayConstructEntityValue(data, "Entities", new Array(), new entities.innerReportDefinitionEntity());
                    this.Joins = this.getArrayConstructEntityValue(data, "Joins", new Array(), new entities.innerReportDefinitionJoin());
                    this.Columns = this.getArrayConstructEntityValue(data, "Columns", new Array(), new entities.innerReportDefinitionColumn());
                    this.Filters = this.getArrayConstructEntityValue(data, "Filters", new Array(), new entities.innerReportDefinitionFilter());
                    this.Groupings = this.getArrayConstructEntityValue(data, "Groupings", new Array(), new entities.innerReportDefinitionGroup());
                    this.Limit = this.getConstructEntityValue(data, "Limit", new entities.innerReportDefinitionLimit());
                };
                return innerReportDefinition;
            }(entities.base.BaseEntity));
            entities.innerReportDefinition = innerReportDefinition;
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
//# sourceMappingURL=innerReportDefinition.js.map