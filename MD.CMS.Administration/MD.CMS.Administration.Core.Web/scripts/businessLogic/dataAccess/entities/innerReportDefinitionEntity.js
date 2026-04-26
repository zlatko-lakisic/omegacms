/// <reference path="./base/iBaseEntity.ts" />
/// <reference path="./base/baseEntity.ts" />
/// <reference path="./innerReportDefinitionGridCoordinates.ts" />
/// <reference path="./innerReportDefinitionProperty.ts" />
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
            var innerReportDefinitionEntity = (function (_super) {
                __extends(innerReportDefinitionEntity, _super);
                function innerReportDefinitionEntity() {
                    var _this = _super.call(this) || this;
                    _this.EntityType = 0;
                    _this.Name = '';
                    _this.Coordinates = new entities.innerReportDefinitionGridCoordinates();
                    _this.UniqueId = '';
                    _this.Fields = new Array();
                    _this.BaseFields = new Array();
                    _this.ExtendedFields = new Array();
                    return _this;
                }
                innerReportDefinitionEntity.prototype.construct = function (data) {
                    _super.prototype.construct.call(this, data);
                    this.EntityType = this.getValue(data, "EntityType", 0);
                    this.Name = this.getValue(data, "Name", '');
                    this.Coordinates = this.getConstructEntityValue(data, "Coordinates", new entities.innerReportDefinitionGridCoordinates());
                    this.UniqueId = this.getValue(data, "UniqueId", '');
                    this.Fields = this.getArrayConstructEntityValue(data, "Fields", new Array(), new entities.innerReportDefinitionProperty());
                    this.BaseFields = this.getArrayConstructEntityValue(data, "BaseFields", new Array(), new entities.innerReportDefinitionProperty());
                    this.ExtendedFields = this.getArrayConstructEntityValue(data, "ExtendedFields", new Array(), new entities.innerReportDefinitionProperty());
                };
                return innerReportDefinitionEntity;
            }(entities.base.BaseEntity));
            entities.innerReportDefinitionEntity = innerReportDefinitionEntity;
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
//# sourceMappingURL=innerReportDefinitionEntity.js.map