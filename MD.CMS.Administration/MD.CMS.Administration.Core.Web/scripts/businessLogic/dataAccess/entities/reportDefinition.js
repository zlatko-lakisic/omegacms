/// <reference path="./base/iBaseEntity.ts" />
/// <reference path="./base/baseEntity.ts" />
/// <reference path="./user.ts" />
/// <reference path="./innerReportDefinition.ts" />
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
            var reportDefinition = (function (_super) {
                __extends(reportDefinition, _super);
                function reportDefinition() {
                    var _this = _super.call(this) || this;
                    _this.Name = '';
                    _this.Definition = new entities.innerReportDefinition();
                    _this.Sql = '';
                    _this.AuthorId = 0;
                    _this.Author = new entities.user();
                    _this.Json = '';
                    _this.DateCreated = new Date();
                    _this.DateModified = new Date();
                    return _this;
                }
                reportDefinition.prototype.construct = function (data) {
                    _super.prototype.construct.call(this, data);
                    this.Name = this.getValue(data, "Name", '');
                    this.Definition = this.getConstructEntityValue(data, "Definition", new entities.innerReportDefinition());
                    this.Sql = this.getValue(data, "Sql", '');
                    this.AuthorId = this.getValue(data, "AuthorId", 0);
                    this.Author = this.getConstructEntityValue(data, "Author", new entities.user());
                    this.Json = this.getValue(data, "Json", '');
                    this.DateCreated = this.getValue(data, "DateCreated", new Date());
                    this.DateModified = this.getValue(data, "DateModified", new Date());
                };
                return reportDefinition;
            }(entities.base.BaseEntity));
            entities.reportDefinition = reportDefinition;
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
//# sourceMappingURL=reportDefinition.js.map