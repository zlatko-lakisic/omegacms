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
            var templateFile = (function (_super) {
                __extends(templateFile, _super);
                function templateFile() {
                    var _this = _super.call(this) || this;
                    _this.Path = '';
                    _this.Name = '';
                    return _this;
                }
                templateFile.prototype.construct = function (data) {
                    _super.prototype.construct.call(this, data);
                    this.Path = this.getValue(data, "Path", '');
                    this.Name = this.getValue(data, "Name", '');
                };
                return templateFile;
            }(entities.base.BaseEntity));
            entities.templateFile = templateFile;
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
//# sourceMappingURL=templateFile.js.map